using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class FieldOperationStudentController : BaseController
    {
        private readonly IFieldOperationStudentService _fieldOperationStudentService;

        public FieldOperationStudentController(IFieldOperationStudentService fieldOperationStudentService)
        {
            _fieldOperationStudentService = fieldOperationStudentService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateFieldOperationStudent([FromBody] FieldOperationStudentDTO fieldOperationStudentDTO)
        {
            var fieldOperationStudent = await _fieldOperationStudentService.Create(fieldOperationStudentDTO);
            return StatusCode(fieldOperationStudent.Code, fieldOperationStudent);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateFieldOperationStudent([FromRoute] int id, [FromBody] FieldOperationStudentDTO fieldOperationStudentDTO)
        {
            var fieldOperationStudent = await _fieldOperationStudentService.Update(id, fieldOperationStudentDTO);
            return StatusCode(fieldOperationStudent.Code, fieldOperationStudent);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveFieldOperationStudent([FromRoute] int id)
        {
            var fieldOperationStudent = await _fieldOperationStudentService.Remove(id);
            return StatusCode(fieldOperationStudent.Code, fieldOperationStudent);
        }

        [HttpGet("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> GetFieldOperationStudents()
        {
            var fieldOperationStudent = await _fieldOperationStudentService.GetList();
            return StatusCode(fieldOperationStudent.Code, fieldOperationStudent);
        }
    }
}