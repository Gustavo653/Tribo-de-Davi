using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class ChecklistAdjustedItemRepository : BaseRepository<ChecklistAdjustedItem, TriboDaviContext>, IChecklistAdjustedItemRepository
    {
        public ChecklistAdjustedItemRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
