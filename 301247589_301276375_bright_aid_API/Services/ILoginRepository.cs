using _301247589_301276375_bright_aid_API.Models;

namespace _301247589_301276375_bright_aid_API.Services
{
    public interface ILoginRepository
    {
        Task<Student?> ValidateStudentCredentialsAsync(string email, string password);
        string GenerateJwtToken(Student student);

    }
}