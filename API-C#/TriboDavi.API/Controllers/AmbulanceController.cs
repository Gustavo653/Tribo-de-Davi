using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TriboDavi.API.Controllers
{
    public class AmbulanceController : BaseController
    {
        private readonly IAmbulanceService _ambulanceService;

        public AmbulanceController(IAmbulanceService ambulanceService)
        {
            _ambulanceService = ambulanceService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateAmbulance([FromBody] AmbulanceDTO ambulanceDTO)
        {
            var ambulance = await _ambulanceService.Create(ambulanceDTO);
            return StatusCode(ambulance.Code, ambulance);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateAmbulance([FromRoute] int id, [FromBody] AmbulanceDTO ambulanceDTO)
        {
            var ambulance = await _ambulanceService.Update(id, ambulanceDTO);
            return StatusCode(ambulance.Code, ambulance);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveAmbulance([FromRoute] int id)
        {
            var ambulance = await _ambulanceService.Remove(id);
            return StatusCode(ambulance.Code, ambulance);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCategories()
        {
            var ambulance = await _ambulanceService.GetList();
            return StatusCode(ambulance.Code, ambulance);
        }
    }
}