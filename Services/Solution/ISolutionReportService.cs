using Domain.Model;
using Domain.Model.SimulatorReport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Solution
{
    public interface ISolutionReportService
    {
        Task<List<GroupModel>> GetGroupsByDataBase(int dataBaseId);
        Task<List<PersonModel>> GetPersonsByGroup(int groupId);
        Task<List<PersonExerciseModel>> GetExercisesByPerson(int dataBaseId, int personId);
        Task<List<PersonAnswerModel>> GetPersonAnswersByPerson(int exerciseId, int personId);
        Task<PersonAnswerModel> GetPersonAnswer(int personAnswerId);
    }
}
