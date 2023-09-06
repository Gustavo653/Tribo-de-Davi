using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class FieldOperationStudentRepository : BaseRepository<FieldOperationStudent, TriboDaviContext>, IFieldOperationStudentRepository
    {
        public FieldOperationStudentRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
