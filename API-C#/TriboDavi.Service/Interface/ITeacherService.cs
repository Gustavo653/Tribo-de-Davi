using Common.DTO;
using Common.Infrastructure;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface ITeacherService : IServiceBase<TeacherDTO>
    {
        Task<ResponseDTO> GetTeacherForListbox();
    }
}