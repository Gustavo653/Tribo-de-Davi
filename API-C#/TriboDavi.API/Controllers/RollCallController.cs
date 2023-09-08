using Common.Functions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class RollCallController : BaseController
    {
        private readonly IRollCallService _rollCallService;

        public RollCallController(IRollCallService rollCallService)
        {
            _rollCallService = rollCallService;
        }

        [HttpGet("")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GetRollCall([FromBody] DateOnly? date)
        {
            int? userId = null;
            var role = User.FindAll(ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            if (role == nameof(RoleName.Teacher))
            {
                userId = Convert.ToInt32(User.GetUserId());
            }
            var graduation = await _rollCallService.GetRollCall(date, userId);
            return StatusCode(graduation.Code, graduation);
        }

        [HttpPost("Presence")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> SetPresence([FromBody] PresenceDTO presenceDTO)
        {
            int? userId = null;
            var role = User.FindAll(ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            if (role == nameof(RoleName.Teacher))
            {
                userId = Convert.ToInt32(User.GetUserId());
            }
            var graduation = await _rollCallService.SetPresence(presenceDTO, userId);
            return StatusCode(graduation.Code, graduation);
        }

        [HttpPost("Generate")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GenerateRollCall()
        {
            int? userId = null;
            var role = User.FindAll(ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            if (role == nameof(RoleName.Teacher))
            {
                userId = Convert.ToInt32(User.GetUserId());
            }
            var graduation = await _rollCallService.GenerateRollCall(userId);
            return StatusCode(graduation.Code, graduation);
        }
    }
}