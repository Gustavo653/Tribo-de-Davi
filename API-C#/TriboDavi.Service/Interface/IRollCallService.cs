using Common.DTO;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IRollCallService
    {
        Task<ResponseDTO> GenerateRollCall(int? teacherId = null);
        Task<ResponseDTO> GetRollCall(DateOnly? date = null, int? teacherId = null);
        Task<ResponseDTO> SetPresence(PresenceDTO presenceDTO, int? teacherId = null);
    }
}