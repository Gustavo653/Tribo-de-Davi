using Common.Functions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriboDavi.Domain.Enum;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.API.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> CreateStudent([FromForm] StudentDTO studentDTO)
        {
            var student = await _studentService.Create(studentDTO, User.GetIdStudentTeacher());
            return StatusCode(student.Code, student);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromForm] StudentDTO studentDTO)
        {
            var student = await _studentService.Update(id, studentDTO, User.GetIdStudentTeacher());
            return StatusCode(student.Code, student);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveStudent([FromRoute] int id)
        {
            var student = await _studentService.Remove(id);
            return StatusCode(student.Code, student);
        }

        [HttpGet("")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}")]
        public async Task<IActionResult> GetStudents()
        {
            var student = await _studentService.GetList(User.GetIdStudentTeacher());
            return StatusCode(student.Code, student);
        }

        [HttpGet("Listbox")]
        [Authorize(Roles = $"{nameof(RoleName.Admin)}, {nameof(RoleName.Teacher)}, {nameof(RoleName.AssistantTeacher)}")]
        public async Task<IActionResult> GetStudentsForListbox()
        {
            var students = await _studentService.GetStudentsForListbox(User.GetIdStudentTeacher());
            return StatusCode(students.Code, students);
        }
    }
}