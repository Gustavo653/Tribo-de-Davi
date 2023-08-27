using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class AmbulanceRepository : BaseRepository<Ambulance, TriboDaviContext>, IAmbulanceRepository
    {
        public AmbulanceRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
