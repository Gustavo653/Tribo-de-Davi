using Common.Functions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("Current")]
        public async Task<IActionResult> GetUser()
        {
            var user = await _accountService.GetCurrent(User.GetEmail());
            return StatusCode(user.Code, user);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            var user = await _accountService.Login(userLogin);
            return StatusCode(user.Code, user);
        }
    }
}