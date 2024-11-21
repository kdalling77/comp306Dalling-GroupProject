using _301247589_301276375_bright_aid_API.Models;

namespace _301247589_301276375_bright_aid_API.Services
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(long id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(long id);
        Task<bool> SaveAsync();
        Task<bool> StudentExistsAsync(long id);
    }
}
