using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class GraduationController : BaseController
    {
        private readonly IGraduationService _graduationService;

        public GraduationController(IGraduationService graduationService)
        {
            _graduationService = graduationService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateGraduation([FromForm] GraduationDTO graduationDTO)
        {
            var graduation = await _graduationService.Create(graduationDTO);
            return StatusCode(graduation.Code, graduation);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateGraduation([FromRoute] int id, [FromForm] GraduationDTO graduationDTO)
        {
            var graduation = await _graduationService.Update(id, graduationDTO);
            return StatusCode(graduation.Code, graduation);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveGraduation([FromRoute] int id)
        {
            var graduation = await _graduationService.Remove(id);
            return StatusCode(graduation.Code, graduation);
        }

        [HttpGet("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> GetGraduations()
        {
            var graduation = await _graduationService.GetList();
            return StatusCode(graduation.Code, graduation);
        }

        [HttpGet("Listbox")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GetGraduationsForListbox()
        {
            var graduation = await _graduationService.GetGraduationsForListbox();
            return StatusCode(graduation.Code, graduation);
        }
    }
}