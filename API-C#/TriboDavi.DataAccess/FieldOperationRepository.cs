using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class FieldOperationRepository : BaseRepository<FieldOperation, TriboDaviContext>, IFieldOperationRepository
    {
        public FieldOperationRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
