using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain.Identity;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class UserRepository : BaseRepository<User, TriboDaviContext>, IUserRepository
    {
        public UserRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
