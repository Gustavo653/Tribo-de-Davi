using AutoMapper;
using Common.DTO;
using Common.Functions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Domain.Enum;
using TriboDavi.Domain.Identity;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IGraduationRepository _graduationRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public TeacherService(ITeacherRepository teacherRepository,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IGraduationRepository graduation)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
            _userManager = userManager;
            _graduationRepository = graduation;
        }

        private async Task UpdateSecurityAndRoleAsync(Teacher teacher, RoleName role)
        {
            await _userManager.UpdateSecurityStampAsync(teacher);
            await _userManager.AddToRoleAsync(teacher, role.ToString());
        }

        private void SetTeacherProperties(TeacherDTO objectDTO, Teacher teacher, Graduation graduation)
        {
            PropertyCopier<TeacherDTO, Teacher>.Copy(objectDTO, teacher);

            teacher.Graduation = graduation;
            teacher.UserName = teacher.Email;
            teacher.NormalizedEmail = teacher.Email.ToUpper();
            teacher.NormalizedUserName = teacher.Email.ToUpper();
            if (!string.IsNullOrEmpty(objectDTO.Password))
                teacher.PasswordHash = _userManager.PasswordHasher.HashPassword(teacher, objectDTO.Password);
        }

        public async Task<ResponseDTO> Create(TeacherDTO objectDTO)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                if (string.IsNullOrEmpty(objectDTO.Password))
                {
                    responseDTO.SetBadInput("A senha deve ser informada!");
                    return responseDTO;
                }

                var existingStudentWithEmail = await _teacherRepository.GetEntities().AnyAsync(x => x.NormalizedEmail == objectDTO.Email.ToUpper());
                if (existingStudentWithEmail)
                {
                    responseDTO.SetBadInput("Já existe um professor cadastrado com este e-mail!");
                    return responseDTO;
                }

                var teacher = _mapper.Map<Teacher>(objectDTO);

                var graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.GraduationId);
                if (graduation == null)
                {
                    responseDTO.SetBadInput("A graduação informada não existe!");
                    return responseDTO;
                }

                if (objectDTO.TeacherId != null)
                {
                    var assistantTeacher = await _teacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.TeacherId && x.AssistantTeacher == null);
                    if (assistantTeacher == null)
                    {
                        responseDTO.SetBadInput("O professor assistente não existe!");
                        return responseDTO;
                    }
                    else
                    {
                        teacher.AssistantTeacher = assistantTeacher;
                    }
                }

                SetTeacherProperties(objectDTO, teacher, graduation);

                await _teacherRepository.InsertAsync(teacher);
                await _teacherRepository.SaveChangesAsync();

                await UpdateSecurityAndRoleAsync(teacher, teacher.AssistantTeacher == null ? RoleName.Teacher : RoleName.AssistantTeacher);

                responseDTO.Object = teacher;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetList()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _teacherRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Remove(int id)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                var student = await _teacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (student == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                _teacherRepository.Delete(student);
                await _teacherRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, TeacherDTO objectDTO)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                var teacher = await _teacherRepository.GetTrackedEntities()
                                                      .Include(x => x.Graduation)
                                                      .FirstOrDefaultAsync(x => x.Id == id);
                if (teacher == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                var graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.GraduationId);
                if (graduation == null)
                {
                    responseDTO.SetBadInput("A graduação informada não existe!");
                    return responseDTO;
                }

                if (objectDTO.TeacherId != null)
                {
                    var assistantTeacher = await _teacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.TeacherId && x.AssistantTeacher == null);
                    if (assistantTeacher == null)
                    {
                        responseDTO.SetBadInput("O professor assistente não existe!");
                        return responseDTO;
                    }
                    else
                    {
                        teacher.AssistantTeacher = assistantTeacher;
                    }
                }

                SetTeacherProperties(objectDTO, teacher, graduation);

                await _teacherRepository.SaveChangesAsync();

                await _userManager.RemoveFromRolesAsync(teacher, new List<string>() { nameof(RoleName.AssistantTeacher), nameof(RoleName.Teacher) });
                await UpdateSecurityAndRoleAsync(teacher, teacher.AssistantTeacher == null ? RoleName.Teacher : RoleName.AssistantTeacher);

                responseDTO.Object = teacher;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }
    }
}