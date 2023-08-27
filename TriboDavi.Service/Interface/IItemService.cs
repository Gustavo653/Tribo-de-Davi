using Common.DTO;
using Common.Infrastructure;
using TriboDavi.DTO;
using Microsoft.AspNetCore.Http;

namespace TriboDavi.Service.Interface
{
    public interface IItemService : IServiceBase<BasicDTO>
    {
        Task<ResponseDTO> ImportCSV(IFormFile csvFile);
    }
}