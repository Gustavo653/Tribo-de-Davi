using AutoMapper;
using Common.DTO;
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
        private readonly ILegalParentRepository _legalParentRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository studentRepository, IMapper mapper, UserManager<User> userManager, ILegalParentRepository legalParentRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _userManager = userManager;
            _legalParentRepository = legalParentRepository;
        }

        public async Task<ResponseDTO> Create(StudentDTO objectDTO)
        {
            ResponseDTO responseDTO = new();

            try
            {
                if (string.IsNullOrEmpty(objectDTO.Password))
                {
                    responseDTO.SetBadInput("A senha deve ser informada!");
                    return responseDTO;
                }

                bool isEmailTaken = await _studentRepository.GetEntities().AnyAsync(x => x.NormalizedEmail == objectDTO.Email.ToUpper());
                if (isEmailTaken)
                {
                    responseDTO.SetBadInput("Já existe um aluno cadastrado com este e-mail!");
                    return responseDTO;
                }

                Student student = _mapper.Map<Student>(objectDTO);
                student.UserName = student.Email;

                if (objectDTO.LegalParentId != null)
                {
                    LegalParent? legalParent = await _legalParentRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == objectDTO.LegalParentId);
                    if (legalParent == null)
                    {
                        responseDTO.SetBadInput("Responsável legal não encontrado!");
                        return responseDTO;
                    }

                    student.LegalParent = legalParent;
                }
                else if (objectDTO.LegalParent == null)
                {
                    responseDTO.SetBadInput("Responsável legal deve estar preenchido!");
                    return responseDTO;
                }

                var userResult = await _userManager.CreateAsync(student, objectDTO.Password);
                if (userResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(student, nameof(RoleName.Student));
                }
                else
                {
                    responseDTO.SetBadInput(userResult.Errors.FirstOrDefault()?.Description);
                }
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
                responseDTO.Object = await _studentRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> Update(int id, StudentDTO objectDTO)
        {
            throw new NotImplementedException();
        }
    }
}