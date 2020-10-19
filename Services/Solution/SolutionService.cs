using Domain.Model;
using Domain.Model.Simulator;
using Domain.Sql;
using Microsoft.EntityFrameworkCore;
using Repositories.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Solution
{
    public class SolutionService : ISolutionService
    {
        private readonly IDatabaseRepository<DataBase> dataBaseRepository;
        private readonly IDatabaseRepository<Exercise> exerciseRepository;
        private readonly IDatabaseRepository<Answer> answerRepository;

        public SolutionService(IDatabaseRepository<DataBase> dataBaseRepository, IDatabaseRepository<Exercise> exerciseRepository, IDatabaseRepository<Answer> answerRepository)
        {
            this.dataBaseRepository = dataBaseRepository;
            this.exerciseRepository = exerciseRepository;
            this.answerRepository = answerRepository;
        }

        public async Task<List<DataBaseModel>> GetDataBases()
        {
            //var aa1 = System.IO.File.ReadAllBytes("D:\\Shop.png");
            //var aa2 = File.ReadAllBytes("D:\\2.png");
            //var a1 = await dataBaseRepository.GetByIdAsync(1);
            //a1.Scheme = aa1;
            //var a2 = await dataBaseRepository.GetByIdAsync(2);
            //a2.Scheme = aa2;
            //await dataBaseRepository.SaveAsync();

            return await dataBaseRepository.Find()
                .OrderBy(x=>x.Name)
                .Select(x => new DataBaseModel() {Id = x.Id, Name = x.Name })
                .ToListAsync();
        }

        public Task<byte[]> GetSchemeByDataBaseId(int dataBaseId)
        {
            return dataBaseRepository.Find()
                .Where(x => x.Id == dataBaseId)
                .Select(x => x.Scheme)
                .FirstOrDefaultAsync();
        }

        public Task<List<ExerciseModel>> GetExercisesByDataBase(int dataBaseId)
        {
            return exerciseRepository.Find()
                .OrderBy(x => x.TaskLevel).ThenBy(x => x.Name)
                .Where(x => x.DataBaseId == dataBaseId)
                .Select(x => new ExerciseModel(x))
                .ToListAsync();
        }

        public Task<ExerciseModel> GetExerciseById(int exerciseId)
        {
            return exerciseRepository.Find()
                .Select(x => new ExerciseModel(x))
                .FirstOrDefaultAsync(x => x.Id == exerciseId);
        }

        public async Task<AnswerModel> GetCorrectAnswer(int exerciseId)
        {
            var result = new AnswerModel();
            var selExercise = await exerciseRepository.GetByIdAsync(exerciseId, x => x.DataBase);
            result.SqlResult = DatabaseSimulatorContext.TryAnswer(selExercise.DataBase.ConnectingString, selExercise.CorrectSql);
            return result;
        }

        public async Task<AnswerModel> GetMyLastAnswer(int exerciseId)
        {
            //var personId = await userIdentityService.GetCurrentUser();
            var personId = 1;
            var myLastAnswerSql = await answerRepository.Find().OrderByDescending(x => x.Date).FirstOrDefaultAsync(x => x.PersonId == personId && x.ExerciseId == exerciseId);
            if (myLastAnswerSql != null)
            {
                return await ExuteSql(myLastAnswerSql.SqlAnswer, personId, exerciseId);
            }
            return new AnswerModel();
        }


        public Task<AnswerModel> TryMyAnswer(int exerciseId, Answer answer)
        {
            //var personId = await userIdentityService.GetCurrentUser();
            var personId = 1;
            return ExuteSql(answer.SqlAnswer, personId, exerciseId);
        }

        private async Task<AnswerModel> ExuteSql(string answerSql, int personId, int exerciseId)
        {
            var result = new AnswerModel() { SqlResult = new SqlResultModel() };
            result.SqlAnswer = answerSql;

            string fixedAnswerBefore;
            string fixedAnswer = answerSql.ToUpper();
            do
            {
                fixedAnswerBefore = fixedAnswer.Clone().ToString();
                fixedAnswer = fixedAnswer.Replace("DROP", "").Replace("DELETE", "").Replace("INSERT", "").Replace("UPDATE", "").Replace("TRUNCATE", "");
            }
            while (fixedAnswerBefore.Length != fixedAnswer.Length);

            var personAnswer = new Answer();
            var isExistAnwer = await answerRepository.Find().AnyAsync(o => o.ExerciseId == exerciseId && o.PersonId == personId && o.SqlAnswer == answerSql);
            if (!isExistAnwer)
            {
                personAnswer.PersonId = personId;
                personAnswer.Date = DateTime.UtcNow;
                personAnswer.SqlAnswer = answerSql;
                personAnswer.ExerciseId = exerciseId;
                personAnswer.IsCorrectAnswer = false;
                answerRepository.Add(personAnswer);
                await answerRepository.SaveAsync();
            }

            var selExercise = await exerciseRepository.GetByIdAsync(exerciseId, x => x.DataBase);
            result.SqlResult = DatabaseSimulatorContext.TryAnswer(selExercise.DataBase.ConnectingString, fixedAnswer);

            if (!result.SqlResult.HasException)
            {
                var correctAnswer = DatabaseSimulatorContext.TryAnswer(selExercise.DataBase.ConnectingString, selExercise.CorrectSql);

                if (result.SqlResult.CountColumns != correctAnswer.CountColumns)
                {
                    result.Result = "Количество колонок не совпадает";
                    result.IsDone = false;
                }
                else if (result.SqlResult.CountRows != correctAnswer.CountRows)
                {
                    result.Result = "Количество строк не совпадает";
                    result.IsDone = false;
                }
                else
                {
                    {
                        bool isColumnNameCorrect = true;
                        for (int i = 0; i < correctAnswer.CountColumns; i++)
                        {
                            string value1 = result.SqlResult.Columns[i];
                            string value2 = correctAnswer.Columns[i];
                            if (value1 != value2)
                            {
                                isColumnNameCorrect = false;
                                break;
                            }
                        }

                        if (!isColumnNameCorrect)
                        {
                            result.Result = "Названия колонок или их порядок не совпадают";
                            result.IsDone = false;
                        }
                        else
                        {
                            bool isValuesCorrect = true;
                            for (int i = 0; i < correctAnswer.CountRows; i++)
                            {
                                for (int j = 0; j < correctAnswer.CountColumns; j++)
                                {
                                    string value1 = result.SqlResult.DataTable[i][j];
                                    string value2 = correctAnswer.DataTable[i][j];
                                    if (value1 != value2)
                                    {
                                        isValuesCorrect = false;
                                        break;
                                    }
                                }
                            }

                            if (!isValuesCorrect)
                            {
                                result.Result = "Результаты запросов не идентичны";
                                result.IsDone = false;
                            }
                            else
                            {
                                personAnswer.IsCorrectAnswer = true;
                                await answerRepository.SaveAsync();
                                result.Result = "Задача выполнена";
                                result.IsDone = true;
                            }
                        }
                    }
                }
            }
            else
            {
                result.Result = result.SqlResult.Exception;
            }
            return result;
        }
    }
}
