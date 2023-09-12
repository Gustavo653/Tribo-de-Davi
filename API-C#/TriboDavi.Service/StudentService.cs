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
                              IGoogleCloudStorageService googleCloudStorageService)
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
        }


        private async Task<LegalParent> GetOrCreateLegalParent(LegalParentDTO legalParentDTO)
        {
            var legalParent = await _legalParentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.CPF == legalParentDTO.CPF)
                              ?? new LegalParent()
                              {
                                  CPF = legalParentDTO.CPF,
                                  Name = legalParentDTO.Name,
                                  PhoneNumber = legalParentDTO.PhoneNumber,
                                  Relationship = legalParentDTO.Relationship,
                                  RG = legalParentDTO.RG
                              };

            if (legalParent.Id == 0)
            {
                legalParent.SetCreatedAt();
            }

            return legalParent;
        }

        private async Task UpdateSecurityAndRoleAsync(Student student)
        {
            await _userManager.UpdateSecurityStampAsync(student);
            await _userManager.AddToRoleAsync(student, nameof(RoleName.Student));
        }

        private void SetStudentProperties(StudentDTO objectDTO, Student student, Graduation graduation, LegalParent legalParent)
        {
            PropertyCopier<StudentDTO, Student>.Copy(objectDTO, student);
            if (objectDTO.Address != null)
            {
                student.Address = new Address()
                {
                    City = objectDTO.Address.City ?? "",
                    Neighborhood = objectDTO.Address.Neighborhood ?? "",
                    StreetName = objectDTO.Address.StreetName ?? "",
                    StreetNumber = objectDTO.Address.StreetNumber ?? ""
                };
                student.Address.SetCreatedAt();
            }
            else
            {
                student.Address = null;
            }

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

                if (objectDTO.CPF == objectDTO.LegalParent.CPF)
                {
                    responseDTO.SetBadInput("O CPF do estudante deve ser diferente do CPF do responsável legal!");
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

                var legalParent = await GetOrCreateLegalParent(objectDTO.LegalParent);

                var graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.GraduationId);
                if (graduation == null)
                {
                    responseDTO.SetBadInput("A graduação informada não existe!");
                    return responseDTO;
                }

                SetStudentProperties(objectDTO, student, graduation, legalParent);

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
                responseDTO.Object = await _fieldOperationStudentRepository.GetEntities()
                                                                           .Where(x => teacherId == null || x.FieldOperationTeacher.Teacher.Id == teacherId)
                                                                           .Select(x => new
                                                                           {
                                                                               x.Student.Id,
                                                                               x.Student.BirthDate,
                                                                               x.Student.Email,
                                                                               x.Student.RG,
                                                                               x.Student.CPF,
                                                                               x.Student.Name,
                                                                               x.Student.PhoneNumber,
                                                                               x.Student.SchoolGrade,
                                                                               x.Student.Weight,
                                                                               x.Student.Height,
                                                                               x.Student.SchoolName,
                                                                               GraduationId = x.Student.Graduation.Id,
                                                                               x.Student.Address,
                                                                               x.Student.Graduation,
                                                                               x.Student.LegalParent,
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
                responseDTO.Object = await _fieldOperationStudentRepository.GetEntities()
                                                                           .Where(x => (teacherId == null || x.FieldOperationTeacher.Teacher.Id == teacherId))
                                                                           .Select(x => new
                                                                           {
                                                                               Code = x.Student.Id,
                                                                               Name = x.Student.Name,
                                                                           })
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

                var fieldOperationStudent = await _fieldOperationStudentRepository.GetTrackedEntities()
                                                                                  .Include(x => x.Student).ThenInclude(x => x.LegalParent)
                                                                                  .Include(x => x.Student).ThenInclude(x => x.Graduation)
                                                                                  .Include(x => x.Student).ThenInclude(x => x.Address)
                                                                                  .FirstOrDefaultAsync(x => x.Student.Id == id && (teacherId == null || x.FieldOperationTeacher.Teacher.Id == teacherId));
                if (fieldOperationStudent == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                var legalParent = await GetOrCreateLegalParent(objectDTO.LegalParent);
                var graduation = await _graduationRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.GraduationId);
                if (graduation == null)
                {
                    responseDTO.SetBadInput("A graduação informada não existe!");
                    return responseDTO;
                }

                SetStudentProperties(objectDTO, fieldOperationStudent.Student, graduation, legalParent);

                if (objectDTO.File != null)
                {
                    await _googleCloudStorageService.DeleteFileFromGcsAsync(fieldOperationStudent.Student.Url);
                    fieldOperationStudent.Student.Url = await _googleCloudStorageService.UploadFileToGcsAsync(objectDTO.File, $"{Guid.NewGuid()}{Path.GetExtension(objectDTO.File.FileName)}");
                }

                await _studentRepository.SaveChangesAsync();

                responseDTO.Object = fieldOperationStudent.Student;
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