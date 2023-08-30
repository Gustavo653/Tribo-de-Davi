using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherDTO teacherDTO)
        {
            var teacher = await _teacherService.Create(teacherDTO);
            return StatusCode(teacher.Code, teacher);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateTeacher([FromRoute] int id, [FromBody] TeacherDTO teacherDTO)
        {
            var teacher = await _teacherService.Update(id, teacherDTO);
            return StatusCode(teacher.Code, teacher);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveTeacher([FromRoute] int id)
        {
            var teacher = await _teacherService.Remove(id);
            return StatusCode(teacher.Code, teacher);
        }

        [HttpGet("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> GetTeachers()
        {
            var teacher = await _teacherService.GetList();
            return StatusCode(teacher.Code, teacher);
        }
    }
}