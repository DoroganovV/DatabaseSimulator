using System.Threading.Tasks;
using Domain.Sql;
using Microsoft.AspNetCore.Mvc;
using Services.Solution;

namespace Website.Controllers
{
    [ApiController]
    [Route("api/solution")]
    public class SolutionController : ControllerBase
    {
        private readonly ISolutionService solutionService;

        public SolutionController(ISolutionService solutionService)
        {
            this.solutionService = solutionService;
        }

        [HttpGet("data-bases")]
        public async Task<IActionResult> GetDataBases()
        {
            return Ok(await solutionService.GetDataBases());
        }

        [HttpGet("scheme/{dataBaseId}")]
        public async Task<IActionResult> GetFile(int dataBaseId)
        {
            var buff = await solutionService.GetSchemeByDataBaseId(dataBaseId);
            if (buff != null)
                return File(buff, "image/png", "Scheme");
            else
                return NotFound();
        }

        [HttpGet("data-bases/{dataBaseId}/exercises")]
        public async Task<IActionResult> GetExercisesByDataBase(int dataBaseId)
        {
            return Ok(await solutionService.GetExercisesByDataBase(dataBaseId));
        }

        [HttpGet("exercise/{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(int exerciseId)
        {
            return Ok(await solutionService.GetExerciseById(exerciseId));
        }

        [HttpGet("exercise/{exerciseId}/correct-solution")]
        public async Task<IActionResult> GetCorrectSolution(int exerciseId)
        {
            return Ok(await solutionService.GetCorrectAnswer(exerciseId));
        }

        [HttpGet("exercise/{exerciseId}/my-solution")]
        public async Task<IActionResult> GetMySolution(int exerciseId)
        {
            return Ok(await solutionService.GetMyLastAnswer(exerciseId));
        }

        [HttpPost("exercise/{exerciseId}/my-solution")]
        public async Task<IActionResult> TryMySolution(int exerciseId, [FromBody] Answer answer)
        {
            return Ok(await solutionService.TryMyAnswer(exerciseId, answer));
        }
    }
}