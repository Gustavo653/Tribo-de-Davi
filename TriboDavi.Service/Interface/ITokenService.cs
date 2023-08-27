
using TriboDavi.Domain.Identity;

namespace TriboDavi.Service.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(User userDTO);
    }
}