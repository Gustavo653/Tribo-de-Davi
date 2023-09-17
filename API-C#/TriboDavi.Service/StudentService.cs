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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IGoogleCloudStorageService _googleCloudStorageService;
        private readonly IFieldOperationStudentRepository _fieldOperationStudentRepository;
        private readonly IFieldOperationTeacherRepository _fieldOperationTeacherRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IGraduationRepository _graduationRepository;
        private readonly ILegalParentRepository _legalParentRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository studentRepository,
                              IMapper mapper,
                              UserManager<User> userManager,
                              ILegalParentRepository legalParentRepository,
                              IGraduationRepository graduation,
                              ITeacherRepository teacherRepository,
                              IFieldOperationStudentRepository fieldOperationStudentRepository,
                              IFieldOperationTeacherRepository fieldOperationTeacherRepository,
                              IGoogleCloudStorageService googleCloudStorageService,
                              IAddressRepository addressRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _userManager = userManager;
            _legalParentRepository = legalParentRepository;
            _graduationRepository = graduation;
            _teacherRepository = teacherRepository;
            _fieldOperationStudentRepository = fieldOperationStudentRepository;
            _fieldOperationTeacherRepository = fieldOperationTeacherRepository;
            _googleCloudStorageService = googleCloudStorageService;
            _addressRepository = addressRepository;
        }

        private async Task UpdateSecurityAndRoleAsync(Student student)
        {
            await _userManager.UpdateSecurityStampAsync(student);
            await _userManager.AddToRoleAsync(student, nameof(RoleName.Student));
        }

        private void SetStudentProperties(StudentDTO objectDTO, Student student, Graduation graduation, Address? address, LegalParent legalParent)
        {
            PropertyCopier<StudentDTO, Student>.Copy(objectDTO, student);

            student.Address = address;
            student.Graduation = graduation;
            student.LegalParent = legalParent;
            student.UserName = student.Email;
            student.NormalizedEmail = student.Email.ToUpper();
            student.NormalizedUserName = student.Email.ToUpper();
            student.CPF ??= "";
            student.RG ??= "";
            if (!string.IsNullOrEmpty(objectDTO.Password))
                student.PasswordHash = _userManager.PasswordHasher.HashPassword(student, objectDTO.Password);
        }

        public async Task<ResponseDTO> Create(StudentDTO objectDTO, int? teacherId = null)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                if (objectDTO.File == null)
                {
                    responseDTO.SetBadInput("Uma foto deve ser enviada!");
                    return responseDTO;
                }

                if (string.IsNullOrEmpty(objectDTO.Password))
                {
                    responseDTO.SetBadInput("A senha deve ser informada!");
                    return responseDTO;
                }

                if (objectDTO.BirthDate > DateTime.Now)
                {
                    responseDTO.SetBadInput("A data de nascimento deve ser menor que o dia de hoje!");
                    return responseDTO;
                }

                var existingTeacherWithEmail = await _teacherRepository.GetEntities().AnyAsync(x => x.NormalizedEmail == objectDTO.Email.ToUpper());
                if (existingTeacherWithEmail)
                {
                    responseDTO.SetBadInput("Já existe um professor cadastrado com este e-mail!");
                    return responseDTO;
                }

                var existingStudentWithEmail = await _studentRepository.GetEntities().AnyAsync(x => x.NormalizedEmail == objectDTO.Email.ToUpper());
                if (existingStudentWithEmail)
                {
                    responseDTO.SetBadInput("Já existe um aluno cadastrado com este e-mail!");
                    return responseDTO;
                }

                var student = _mapper.Map<Student>(objectDTO);

                var legalParent = await _legalParentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.LegalParentId);
                if (legalParent == null)
                {
                    responseDTO.SetBadInput("O responsável legal informado não existe!");
                    return responseDTO;
                }

                var address = await _addressRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.AddressId);

                var graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.GraduationId);
                if (graduation == null)
                {
                    responseDTO.SetBadInput("A graduação informada não existe!");
                    return responseDTO;
                }

                SetStudentProperties(objectDTO, student, graduation, address, legalParent);

                student.Url = await _googleCloudStorageService.UploadFileToGcsAsync(objectDTO.File, $"{Guid.NewGuid()}{Path.GetExtension(objectDTO.File.FileName)}");

                await _studentRepository.InsertAsync(student);
                await _studentRepository.SaveChangesAsync();

                await UpdateSecurityAndRoleAsync(student);

                responseDTO.Object = student;

                if (teacherId != null)
                {
                    var fieldOperationTeacher = await _fieldOperationTeacherRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Teacher.Id == teacherId);
                    FieldOperationStudent fieldOperationStudent = new() { FieldOperationTeacher = fieldOperationTeacher!, Student = student, Enabled = true };
                    await _fieldOperationStudentRepository.InsertAsync(fieldOperationStudent);
                    await _fieldOperationStudentRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetList(int? teacherId = null)
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _studentRepository.GetEntities()
                                                             .Where(x => teacherId == null ||
                                                                         x.FieldOperationStudents.Any(y => y.FieldOperationTeacher.Teacher.Id == teacherId ||
                                                                                                           y.FieldOperationTeacher.Teacher.AssistantTeachers.Any(a => a.Id == teacherId)))
                                                             .Select(x => new
                                                             {
                                                             x.Id,
                                                             x.BirthDate,
                                                             Url = x.GetUrl(),
                                                             x.Email,
                                                             x.RG,
                                                             x.CPF,
                                                             x.Name,
                                                             x.PhoneNumber,
                                                             x.SchoolGrade,
                                                             x.Weight,
                                                             x.Height,
                                                             x.SchoolName,
                                                             GraduationId = x.Graduation.Id,
                                                             LegalParentId = x.LegalParent.Id,
                                                             AddressId = x.Address != null ? x.Address.Id : 0,
                                                             x.Address,
                                                             x.Graduation,
                                                             x.LegalParent,
                                                             })
                                                             .ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetStudentsForListbox(int? teacherId)
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _studentRepository.GetEntities()
                                                             .Where(x => teacherId == null ||
                                                                         x.FieldOperationStudents.Any(y => y.FieldOperationTeacher.Teacher.Id == teacherId ||
                                                                                                           y.FieldOperationTeacher.Teacher.AssistantTeachers.Any(a => a.Id == teacherId)))
                                                             .Select(x => new
                                                             {
                                                                 Code = x.Id,
                                                                 Name = x.Name,
                                                             })
                                                             .OrderBy(x => x.Name)
                                                             .ToListAsync();
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
                var student = await _studentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (student == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                _studentRepository.Delete(student);
                await _studentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, StudentDTO objectDTO, int? teacherId = null)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                if (objectDTO.BirthDate > DateTime.Now)
                {
                    responseDTO.SetBadInput("A data de nascimento deve ser menor que o dia de hoje!");
                    return responseDTO;
                }

                var student = await _studentRepository.GetTrackedEntities()
                                                      .Include(x => x.LegalParent)
                                                      .Include(x => x.Graduation)
                                                      .Include(x => x.Address)
                                                      .FirstOrDefaultAsync(x => teacherId == null || x.FieldOperationStudents.Any(y => y.FieldOperationTeacher.Teacher.Id == teacherId));
                if (student == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                var existingTeacherWithEmail = await _teacherRepository.GetEntities().AnyAsync(x => x.NormalizedEmail == objectDTO.Email.ToUpper());
                if (existingTeacherWithEmail)
                {
                    responseDTO.SetBadInput("Já existe um professor cadastrado com este e-mail!");
                    return responseDTO;
                }

                var existingStudentWithEmail = await _studentRepository.GetEntities().AnyAsync(x => x.Id != id && x.NormalizedEmail == objectDTO.Email.ToUpper());
                if (existingStudentWithEmail)
                {
                    responseDTO.SetBadInput("Já existe um aluno cadastrado com este e-mail!");
                    return responseDTO;
                }

                var legalParent = await _legalParentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.LegalParentId);
                if (legalParent == null)
                {
                    responseDTO.SetBadInput("O responsável legal informado não existe!");
                    return responseDTO;
                }

                var graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.GraduationId);
                if (graduation == null)
                {
                    responseDTO.SetBadInput("A graduação informada não existe!");
                    return responseDTO;
                }

                var address = await _addressRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.AddressId);

                SetStudentProperties(objectDTO, student, graduation, address, legalParent);

                if (objectDTO.File != null)
                {
                    await _googleCloudStorageService.DeleteFileFromGcsAsync(student.GetUrl());
                    student.Url = await _googleCloudStorageService.UploadFileToGcsAsync(objectDTO.File, $"{Guid.NewGuid()}{Path.GetExtension(objectDTO.File.FileName)}");
                }

                await _studentRepository.SaveChangesAsync();

                responseDTO.Object = student;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public Task<ResponseDTO> Create(StudentDTO objectDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> Update(int id, StudentDTO objectDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> GetList()
        {
            throw new NotImplementedException();
        }
    }
}