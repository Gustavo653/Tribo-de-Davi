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

        public override IQueryable<Teacher> GetEntities()
        {
            return base.GetEntities().Where(x => x.Email != "admin@admin.com");
        }
    }
}
