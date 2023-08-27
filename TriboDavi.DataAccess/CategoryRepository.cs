using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class CategoryRepository : BaseRepository<Category, TriboDaviContext>, ICategoryRepository
    {
        public CategoryRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
