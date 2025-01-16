using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using carwash.Migrations;
using carwash.Model;
using Microsoft.AspNetCore.Identity.Data;

namespace carwash.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: api/User/Register
    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register([FromBody] RegisterRequest registerRequest)
    {
        if (_context.Users.Any(u => u.Email == registerRequest.Email))
        {
            return BadRequest("Email is already in use.");
        }

        var user = new User
        {
            Email = registerRequest.Email,
            Password = HashPassword(registerRequest.Password),
            Name = registerRequest.Name,
            Role = "Customer", // Default role
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProfile", new { userId = user.UserId }, user);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    // GET: api/User/Orders/{userId}
    [HttpGet("Orders/{userId}")]
    public async Task<ActionResult> GetUserOrders(int userId)
    {
        var orders = await _context.Orders
                                    .Where(o => o.UserId == userId)
                                    .Include(o => o.Package)
                                    .Include(o => o.Car)
                                    .Include(o => o.Washer) // Include the washer details
                                    .ToListAsync();

        if (!orders.Any())
        {
            return NotFound("No orders found.");
        }

        return Ok(orders);
    }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}
}
