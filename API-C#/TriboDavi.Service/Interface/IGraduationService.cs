using Common.DTO;
using Common.Infrastructure;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IGraduationService : IServiceBase<GraduationDTO>
    {
        Task<ResponseDTO> GetGraduationsForListbox();
    }
}