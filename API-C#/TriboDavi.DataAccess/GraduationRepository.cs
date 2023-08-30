using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class GraduationRepository : BaseRepository<Graduation, TriboDaviContext>, IGraduationRepository
    {
        public GraduationRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
