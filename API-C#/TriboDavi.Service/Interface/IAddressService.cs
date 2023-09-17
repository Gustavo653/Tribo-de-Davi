using Common.DTO;
using Common.Infrastructure;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IAddressService : IServiceBase<AddressDTO>
    {
        Task<ResponseDTO> GetAddressesForListbox();
    }
}