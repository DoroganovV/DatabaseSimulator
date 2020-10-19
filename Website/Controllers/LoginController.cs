using System.Threading.Tasks;
using Domain.Enums;
using Domain.Model.Simulator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Login;

namespace Website.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetPersonGroups()
        {
            var result = await loginService.GetPersonGroups();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            loginRequestModel.Host = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await loginService.Login(loginRequestModel);
            if (result.Item1 == LoginResult.OK)
            {
                HttpContext.Session.SetString("user", result.Item2.Id.ToString());
            }
            return Ok(result);
        }
        [HttpGet("tryLogin")]
        public async Task<IActionResult> TryLogin()
        {
            string userId = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userId))
            {
                var result = await loginService.GetPersonAccessById(int.Parse(userId));
                HttpContext.Session.SetString("user", result.Item2.Id.ToString());
                return Ok(result);
            }
            else
                return Ok((LoginResult.PersonNotFound, null as PersonModel));
        }
        [HttpDelete]
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("user", "");
            return Ok((LoginResult.OK, null as PersonModel));
        }
    }
}