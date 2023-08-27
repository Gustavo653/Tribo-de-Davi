using Common.DTO;
using Common.Functions;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain.Enum;
using TriboDavi.Domain.Identity;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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