using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class FieldOperationTeacherRepository : BaseRepository<FieldOperationTeacher, TriboDaviContext>, IFieldOperationTeacherRepository
    {
        public FieldOperationTeacherRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
