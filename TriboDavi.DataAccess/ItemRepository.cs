using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class ItemRepository : BaseRepository<Item, TriboDaviContext>, IItemRepository
    {
        public ItemRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
