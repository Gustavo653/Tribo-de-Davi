using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class ChecklistRepository : BaseRepository<Checklist, TriboDaviContext>, IChecklistRepository
    {
        public ChecklistRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
