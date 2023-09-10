using Common.DTO;
using Common.Infrastructure;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IStudentService : IServiceBase<StudentDTO>
    {
        Task<ResponseDTO> GetStudentsForListbox(int? teacherId = null);
        Task<ResponseDTO> Create(StudentDTO objectDTO, int? teacherId = null);
        Task<ResponseDTO> Update(int id, StudentDTO objectDTO, int? teacherId = null);
        Task<ResponseDTO> GetList(int? teacherId = null);
    }
}