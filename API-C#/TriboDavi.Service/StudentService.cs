using AutoMapper;
using Common.DTO;
using Microsoft.EntityFrameworkCore;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> Create(StudentDTO objectDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                if (objectDTO.Password == null)
                {
                    responseDTO.SetBadInput("A senha deve ser informada!");
                    return responseDTO;
                }

                if (await _studentRepository.GetEntities().AnyAsync(x => x.NormalizedEmail == objectDTO.Email.ToUpper()))
                {
                    responseDTO.SetBadInput("Já existe um aluno cadastrado com este e-mail!");
                    return responseDTO;
                }

                Student student = _mapper.Map<Student>(objectDTO);

                await _studentRepository.InsertAsync(student);
                await _studentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetList()
        {
            throw new NotImplementedException();
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