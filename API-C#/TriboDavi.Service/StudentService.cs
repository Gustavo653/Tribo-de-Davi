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
        private readonly IFieldOperationStudentRepository _fieldOperationStudentRepository;
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
                              IFieldOperationStudentRepository fieldOperationStudentRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _userManager = userManager;
            _legalParentRepository = legalParentRepository;
            _graduationRepository = graduation;
            _teacherRepository = teacherRepository;
            _fieldOperationStudentRepository = fieldOperationStudentRepository;
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

        public async Task<ResponseDTO> Create(StudentDTO objectDTO)
        {
            var responseDTO = new ResponseDTO();

            try
            {
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

                await _studentRepository.InsertAsync(student);
                await _studentRepository.SaveChangesAsync();

                await UpdateSecurityAndRoleAsync(student);

                responseDTO.Object = student;
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
                responseDTO.Object = await _studentRepository.GetEntities()
                                                             .Include(x => x.Address)
                                                             .Include(x => x.LegalParent)
                                                             .Include(x => x.Graduation)
                                                             .Select(x => new
                                                             {
                                                                 x.Id,
                                                                 x.BirthDate,
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

        public async Task<ResponseDTO> Update(int id, StudentDTO objectDTO)
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
                                                      .FirstOrDefaultAsync(x => x.Id == id);
                if (student == null)
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

                SetStudentProperties(objectDTO, student, graduation, legalParent);

                var teste = _studentRepository.GetChanges();

                await _studentRepository.SaveChangesAsync();

                responseDTO.Object = student;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }
    }
}