using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class TeacherRepository : BaseRepository<Teacher, TriboDaviContext>, ITeacherRepository
    {
        public TeacherRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
