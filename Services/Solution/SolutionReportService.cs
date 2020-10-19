using Domain.Model;
using Domain.Model.SimulatorReport;
using Domain.Sql;
using Microsoft.EntityFrameworkCore;
using Repositories.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Services.Solution
{
    public class SolutionReportService : ISolutionReportService
    {
        private readonly IDatabaseRepository<Answer> answerRepository;
        private readonly IDatabaseRepository<Person> personRepository;

        public SolutionReportService(
            IDatabaseRepository<Answer> answerRepository,
            IDatabaseRepository<Person> personRepository)
        {
            this.answerRepository = answerRepository;
            this.personRepository = personRepository;
        }

        public Task<List<GroupModel>> GetGroupsByDataBase(int dataBaseId)
        {
            return answerRepository.Find()
                .Where(x => x.Exercise.DataBaseId == dataBaseId)
                .Include(x => x.Person).ThenInclude(x => x.Group)
                .Select(x => x.Person.Group)
                .Distinct()
                .Select(x => new GroupModel() { Id = x.Id, Name = x.Name })
                .ToListAsync();
        }

        public Task<List<PersonModel>> GetPersonsByGroup(int groupId)
        {
            return personRepository.Find()
                .Where(x => x.GroupId == groupId)
                .Include(x => x.LoginHistories)
                .Select(x => new PersonModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    LastLogin = x.LoginHistories.Max(l => l.DateLogin)
                }).ToListAsync();
        }

        public async Task<List<PersonExerciseModel>> GetExercisesByPerson(int dataBaseId, int personId)
        {
            var result = await answerRepository.Find()
                .Include(x => x.Exercise)
                .Where(x => x.Exercise.DataBaseId == dataBaseId && x.PersonId == personId)
                .ToListAsync();

            return result.GroupBy(x => x.Exercise).Select(x => new PersonExerciseModel()
            {
                Id = x.Key.Id,
                Name = x.Key.Name,
                TaskLevel = x.Key.TaskLevel,
                Text = x.Key.Text,
                LastAttempt = x.Max(l => l.Date),
                Attempts = x.Count()
            }).ToList();
        }

        public Task<List<PersonAnswerModel>> GetPersonAnswersByPerson(int exerciseId, int personId)
        {
            return answerRepository.Find()
                .Where(x => x.ExerciseId == exerciseId && x.PersonId == personId && x.IsCorrectAnswer.HasValue)
                .Select(x => new PersonAnswerModel()
                {
                    Id = x.Id,
                    Result = x.IsCorrectAnswer.Value
                }).ToListAsync();
        }

        public async Task<PersonAnswerModel> GetPersonAnswer(int personAnswerId)
        {
            var personAnswer = await answerRepository.Find()
                .Include(x => x.Exercise).ThenInclude(x => x.DataBase)
                .FirstOrDefaultAsync(x => x.Id == personAnswerId);

            var result = new PersonAnswerModel()
            {
                Id = personAnswer.Id,
                Result = personAnswer.IsCorrectAnswer.Value,
                SqlAnswer = personAnswer.SqlAnswer,
                SqlCorrectAnswer = personAnswer.Exercise.CorrectSql
            };

            result.SqlResult = DatabaseSimulatorContext.TryAnswer(personAnswer.Exercise.DataBase.ConnectingString, result.SqlAnswer);
            result.SqlCorrectResult = DatabaseSimulatorContext.TryAnswer(personAnswer.Exercise.DataBase.ConnectingString, result.SqlCorrectAnswer);

            return result;
        }
    }
}
