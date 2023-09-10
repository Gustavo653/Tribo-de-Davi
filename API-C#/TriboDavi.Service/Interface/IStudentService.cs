using Common.DTO;
using Common.Infrastructure;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IStudentService : IServiceBase<StudentDTO>
    {
        Task<ResponseDTO> GetStudentsForListbox(int? teacherId = null);
    }
}