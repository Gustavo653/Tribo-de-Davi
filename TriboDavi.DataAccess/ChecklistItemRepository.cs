using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class ChecklistItemRepository : BaseRepository<ChecklistItem, TriboDaviContext>, IChecklistItemRepository
    {
        public ChecklistItemRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
