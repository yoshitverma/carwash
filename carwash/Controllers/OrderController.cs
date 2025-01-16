using System;
using System.Linq;
using System.Threading.Tasks;
using carwash.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using carwash.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using carwash.Migrations;

namespace carwash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public OrderController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // Order Now (Immediate Order)
        [HttpPost("OrderNow")]
        public async Task<ActionResult<Order>> OrderNow([FromBody] OrderRequest orderRequest)
        {
            if (orderRequest == null)
            {
                return BadRequest("Invalid order data.");
            }

            // Check if the user is valid
            var user = await _context.Users.FindAsync(orderRequest.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Create new order object
            var order = new Order
            {
                UserId = orderRequest.UserId,
                WasherId = orderRequest.WasherId,
                CarId = orderRequest.CarId,
                PackageId = orderRequest.PackageId,
                Status = "Pending",
                ScheduledTime = DateTime.Now, // Immediate order
                Amount = orderRequest.Amount,
                Notes = orderRequest.Notes,
                CreatedAt = DateTime.Now
            };

            // Add the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // Schedule Later (Order with scheduled time)
        [HttpPost("ScheduleLater")]
        public async Task<ActionResult<Order>> ScheduleLater([FromBody] OrderRequest orderRequest)
        {
            if (orderRequest == null)
            {
                return BadRequest("Invalid order data.");
            }

            // Check if the user is valid
            var user = await _context.Users.FindAsync(orderRequest.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Ensure scheduled time is in the future
            if (orderRequest.ScheduledTime <= DateTime.Now)
            {
                return BadRequest("Scheduled time must be in the future.");
            }

            // Create new order object
            var order = new Order
            {
                UserId = orderRequest.UserId,
                WasherId = orderRequest.WasherId,
                CarId = orderRequest.CarId,
                PackageId = orderRequest.PackageId,
                Status = "Scheduled",
                ScheduledTime = orderRequest.ScheduledTime, // Scheduled time
                Amount = orderRequest.Amount,
                Notes = orderRequest.Notes,
                CreatedAt = DateTime.Now
            };

            // Add the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // Optionally, you can create a Get method to fetch orders by ID (for testing purposes)
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }

    // Helper class for receiving order requests
    public class OrderRequest
    {
        public int UserId { get; set; }
        public int WasherId { get; set; }
        public int CarId { get; set; }
        public int PackageId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public DateTime? ScheduledTime { get; set; } // Only used for "ScheduleLater" endpoint
    }
}