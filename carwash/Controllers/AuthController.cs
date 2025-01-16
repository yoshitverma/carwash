using Microsoft.AspNetCore.Mvc;
using carwash.Model;
using carwash.Services;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.Extensions.Configuration;
using carwash.Migrations;
using System.Security.Cryptography;

namespace carwash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginRequest.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            // You should hash passwords in a real-world application.
            // Here, it's just for demonstration.
            if (user.Password != loginRequest.Password)
                return Unauthorized("Invalid credentials");

            // Generate JWT Token
            var token = _jwtService.GenerateToken(user.Email, user.Role);

            return Ok(new { Token = token });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
