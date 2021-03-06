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

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await _context.Gender.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).FirstOrDefaultAsync(predicate: x => x.Id == studentId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }

        public async Task<bool> HasStudentAsync(Guid studentId)
        {   
            return await _context.Student.FirstOrDefaultAsync(x => x.Id == studentId) != null ? true : false;
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.FirstName = request.FirstName;
                existingStudent.LastName = request.LastName;
                existingStudent.Email = request.Email;
                existingStudent.DateOfBirth = request.DateOfBirth;
                existingStudent.GenderId = request.GenderId;
                existingStudent.Mobile = request.Mobile;
                existingStudent.Address.PhysicalAddress = request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress = request.Address.PostalAddress;

                await _context.SaveChangesAsync();

                return existingStudent;
            }
            else
            {
                return null;
            }
                   
        }
        public async Task<Student> DeleteStudentAsync(Guid studentId)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                 _context.Student.Remove(existingStudent);
                await _context.SaveChangesAsync();
                return existingStudent;
            }
            else
            {
               return  null;
            }
        }

       public async Task<Student> AddStudentAsync(Student request)
        {
           var student = await _context.Student.AddAsync(request);
            await _context.SaveChangesAsync();
            return student.Entity;
           
        }

        public async Task<bool> UpdateProfileImageAsync(Guid studentId, string profileImageUrl)
        {
            var student = _context.Student.FirstOrDefault(s => s.Id == studentId);

            if (student != null)
            {
                student.ProfileImageUrl = profileImageUrl;
                _context.Student.Update(student);
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }
    }
}
