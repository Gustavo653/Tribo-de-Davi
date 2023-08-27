using Common.DataAccess;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.Domain.Identity;
using TriboDavi.Persistence;

namespace TriboDavi.DataAccess
{
    public class StudentRepository : BaseRepository<Student, TriboDaviContext>, IStudentRepository
    {
        public StudentRepository(TriboDaviContext context) : base(context)
        {
        }
    }
}
