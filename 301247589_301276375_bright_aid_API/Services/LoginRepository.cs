using _301247589_301276375_bright_aid_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

using System.IdentityModel.Tokens.Jwt;
using System.Text;



namespace _301247589_301276375_bright_aid_API.Services
{
    public class LoginRepository : ILoginRepository
    {
        private readonly StudentContext _context;
        private readonly IConfiguration _configuration;

        public LoginRepository(StudentContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Student?> ValidateStudentCredentialsAsync(string email, string password)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email && s.IsActive);
            if (student == null || student.Password != password) // Add proper password hashing in production
            {
                return null;
            }
            return student;
        }

        public string GenerateJwtToken(Student student)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, student.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, student.Email),
            new Claim(ClaimTypes.Name, student.FirstName ?? ""),
            new Claim(ClaimTypes.Role, "Student"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
