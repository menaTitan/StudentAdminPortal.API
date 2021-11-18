using StudentAdminPortal.API.DataModels;
using System.Linq;

namespace StudentAdminPortal.API.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext  _context;
        public SqlStudentRepository(StudentAdminContext context)
        {
            _context = context;
        }

        public List<Student> GetStudents()
        {
            return _context.Student.ToList();
        }
    }
}
