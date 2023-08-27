using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class LegalParentRepository : BaseRepository<LegalParent, TriboDaviContext>, ILegalParentRepository
    {
        public LegalParentRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
