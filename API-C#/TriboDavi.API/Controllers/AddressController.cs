using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO addressDTO)
        {
            var address = await _addressService.Create(addressDTO);
            return StatusCode(address.Code, address);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateAddress([FromRoute] int id, [FromBody] AddressDTO addressDTO)
        {
            var address = await _addressService.Update(id, addressDTO);
            return StatusCode(address.Code, address);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveAddress([FromRoute] int id)
        {
            var address = await _addressService.Remove(id);
            return StatusCode(address.Code, address);
        }

        [HttpGet("")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GetAddresses()
        {
            var address = await _addressService.GetList();
            return StatusCode(address.Code, address);
        }

        [HttpGet("Listbox")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GetAddressesForListbox()
        {
            var graduation = await _addressService.GetAddressesForListbox();
            return StatusCode(graduation.Code, graduation);
        }
    }
}