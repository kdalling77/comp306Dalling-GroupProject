using Microsoft.AspNetCore.Mvc;
using _301247589_301276375_bright_aid_API.DTOs;
using _301247589_301276375_bright_aid_API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace _301247589_301276375_bright_aid_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRepository _loginService;

        public AuthController(ILoginRepository loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("/student/login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            // Validate credentials using the login service
            var student = await _loginService.ValidateStudentCredentialsAsync(loginDto.Email, loginDto.Password);

            if (student == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Generate JWT token using the login service
            var token = _loginService.GenerateJwtToken(student);

            // Create the response DTO
            var response = new LoginResponseDto
            {
                StudentId = student.Id,
                AccessToken = token
            };

            return Ok(response);
        }
    }
}