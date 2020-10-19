using Domain.Model.Simulator;
using Domain.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Solution
{
    public interface ISolutionService
    {
        Task<List<DataBaseModel>> GetDataBases();
        Task<byte[]> GetSchemeByDataBaseId(int dataBaseId);
        Task<List<ExerciseModel>> GetExercisesByDataBase(int taskDataBaseId);
        Task<ExerciseModel> GetExerciseById(int exerciseId);
        Task<AnswerModel> GetCorrectAnswer(int exerciseId);
        Task<AnswerModel> GetMyLastAnswer(int exerciseId);
        Task<AnswerModel> TryMyAnswer(int exerciseId, Answer answer);
    }
}
