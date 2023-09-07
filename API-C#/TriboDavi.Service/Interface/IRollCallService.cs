using Common.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IRollCallService
    {
        Task<ResponseDTO> GenerateRollCall();
    }
}