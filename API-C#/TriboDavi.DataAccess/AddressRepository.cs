using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class AddressRepository : BaseRepository<Address, TriboDaviContext>, IAddressRepository
    {
        public AddressRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
