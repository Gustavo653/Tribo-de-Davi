using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class ChecklistReviewRepository : BaseRepository<ChecklistReview, TriboDaviContext>, IChecklistReviewRepository
    {
        public ChecklistReviewRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
