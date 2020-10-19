using System.Threading.Tasks;
using Domain.Sql;
using Microsoft.AspNetCore.Mvc;
using Services.Solution;

namespace Website.Controllers
{
    [ApiController]
    [Route("api/solution-report")]
    public class SolutionReportController : ControllerBase
    {
        private readonly ISolutionReportService solutionReportService;

        public SolutionReportController(ISolutionReportService solutionReportService)
        {
            this.solutionReportService = solutionReportService;
        }

        [HttpGet("data-bases/{dataBaseId}/groups")]
        public async Task<IActionResult> GetGroupsByDataBase(int dataBaseId)
        {
            return Ok(await solutionReportService.GetGroupsByDataBase(dataBaseId));
        }

        [HttpGet("group/{groupId}/persons")]
        public async Task<IActionResult> GetPersonsByGroup(int groupId)
        {
            return Ok(await solutionReportService.GetPersonsByGroup(groupId));
        }

        [HttpGet("data-bases/{dataBaseId}/exercise/{personId}")]
        public async Task<IActionResult> GetExercisesByPerson(int dataBaseId, int personId)
        {
            return Ok(await solutionReportService.GetExercisesByPerson(dataBaseId, personId));
        }

        [HttpGet("exercise/{exerciseId}/person/{personId}")]
        public async Task<IActionResult> GetPersonAnswersByPerson(int exerciseId, int personId)
        {
            return Ok(await solutionReportService.GetPersonAnswersByPerson(exerciseId, personId));
        }

        [HttpGet("person-answer/{personAnswerId}")]
        public async Task<IActionResult> GetPersonAnswer(int personAnswerId)
        {
            return Ok(await solutionReportService.GetPersonAnswer(personAnswerId));
        }
    }
}