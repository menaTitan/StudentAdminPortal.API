using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext  _context;
        public SqlStudentRepository(StudentAdminContext context)
        {
            _context = context;
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).FirstOrDefaultAsync(predicate: x => x.Id == studentId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }

    }
}
