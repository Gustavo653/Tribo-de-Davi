using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class LegalParentController : BaseController
    {
        private readonly ILegalParentService _legalParentService;

        public LegalParentController(ILegalParentService legalParentService)
        {
            _legalParentService = legalParentService;
        }

        [HttpPost("")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> CreateLegalParent([FromBody] LegalParentDTO legalParentDTO)
        {
            var legalParent = await _legalParentService.Create(legalParentDTO);
            return StatusCode(legalParent.Code, legalParent);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> UpdateLegalParent([FromRoute] int id, [FromBody] LegalParentDTO legalParentDTO)
        {
            var legalParent = await _legalParentService.Update(id, legalParentDTO);
            return StatusCode(legalParent.Code, legalParent);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveLegalParent([FromRoute] int id)
        {
            var legalParent = await _legalParentService.Remove(id);
            return StatusCode(legalParent.Code, legalParent);
        }

        [HttpGet("")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GetLegalParents()
        {
            var legalParent = await _legalParentService.GetList();
            return StatusCode(legalParent.Code, legalParent);
        }

        [HttpGet("Listbox")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GetLegalParentsForListbox()
        {
            var graduation = await _legalParentService.GetLegalParentsForListbox();
            return StatusCode(graduation.Code, graduation);
        }
    }
}