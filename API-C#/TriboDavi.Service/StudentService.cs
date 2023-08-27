using Common.DTO;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.Service
{
    public class StudentService : IStudentService
    {
        public StudentService()
        {
        }

        public Task<ResponseDTO> Create(StudentDTO objectDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> Update(int id, StudentDTO objectDTO)
        {
            throw new NotImplementedException();
        }
    }
}