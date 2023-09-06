using Common.DTO;
using Common.Infrastructure;
using TriboDavi.DTO;

namespace TriboDavi.Service.Interface
{
    public interface IFieldOperationService : IServiceBase<FieldOperationDTO>
    {
        Task<ResponseDTO> GetFieldOperationsForListbox();
    }
}