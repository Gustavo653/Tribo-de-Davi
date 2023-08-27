using Common.DTO;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IAccountService
    {
        Task<ResponseDTO> GetCurrent(string email);
        Task<ResponseDTO> Login(UserLoginDTO userLoginDTO);
    }
}