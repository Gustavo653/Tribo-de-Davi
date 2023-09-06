using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class RollCallRepository : BaseRepository<RollCall, TriboDaviContext>, IRollCallRepository
    {
        public RollCallRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
