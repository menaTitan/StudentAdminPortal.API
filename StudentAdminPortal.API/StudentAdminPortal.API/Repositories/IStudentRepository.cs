﻿using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(Guid studentId);
        Task<List<Gender>> GetGendersAsync();
        Task<bool> HasStudentAsync(Guid studentId);
        Task<Student>UpdateStudent(Guid studentId, Student request);
        Task<Student> DeleteStudentAsync(Guid studentId);
        Task<Student> AddStudentAsync(Student student);
        Task<bool> UpdateProfileImageAsync(Guid studentId, string profileImageUrl);

    }
}
