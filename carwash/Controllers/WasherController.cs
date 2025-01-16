using Microsoft.AspNetCore.Mvc;
using carwash.Model;
using System.Linq;
using System.Threading.Tasks;
using carwash.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using carwash.Services;
using Microsoft.VisualBasic;
using System.Reflection.Emit;

namespace carwash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WasherController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _iconfiguration;

        public WasherController(ApplicationDbContext context, IConfiguration _iconfiguration)
        {
            _context = context;
            _iconfiguration = _iconfiguration;
        }
        // {
        //     _context = context;
        //     _emailService = emailService;
        // }

        // POST: api/Washer/Register
        [HttpPost("Register")]
        public async Task<ActionResult<Washer>> Register([FromBody] RegisterWasherRequest registerRequest)
        {
            if (_context.Washers.Any(w => w.Email == registerRequest.Email))
            {
                return BadRequest("Email is already in use.");
            }

            var washer = new Washer
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                Contact = registerRequest.Contact,
                address = registerRequest.Address,
                Rating = 0 // Initial rating
            };

            _context.Washers.Add(washer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWasherProfile), new { washerId = washer.WasherId }, washer);
        }

        // GET: api/Washer/Profile/{washerId}
        [HttpGet("Profile/{washerId}")]
        public async Task<ActionResult> GetWasherProfile(int washerId)
        {
            var washer = await _context.Washers.FindAsync(washerId);

            if (washer == null)
            {
                return NotFound("Washer not found.");
            }

            return Ok(washer);
        }

        // POST: api/Washer/AcceptOrder/{orderId}
        [HttpPost("AcceptOrder/{orderId}")]
        public async Task<ActionResult> AcceptOrder(int orderId, [FromBody] string action)
        {
            var order = await _context.Orders.FindAsync(orderId);

            var user = _context.Orders.Include(o => o.User).FirstOrDefault(o => o.OrderId == orderId).User;

            if (order == null || order.Status != "Pending" )
            {
                return NotFound("Order not found or not in pending state.");
            }

            if (action == "Accept" )
            {
                order.Status = "Accepted";
                await _context.SaveChangesAsync();

                // Send email to user
                // var user = await _context.Users.FindAsync(order.UserId);
                // if (user != null)
                // {
                //     var subject = "Your Order Has Been Accepted";
                //     var body = $"Dear {user.Name},<br/><br/>Your order with ID {order.OrderId} has been accepted by the washer.<br/><br/>Thank you for using our service.";
                //     await _emailService.SendEmailAsync(user.Email, subject, body);
                // }
                var emailService = new EmailService(_iconfiguration);
                string subject="Your Order Has Been Accepted";
                string body = $"Dear User,<br/><br/>Your order with ID {order.OrderId} has been accepted by the washer.<br/><br/>Thank you for using our service.";
                await emailService.SendEmailAsync(user.Email, subject, body);
                return Ok("Order accepted.");
            }

            if (action == "Decline")
            {
                order.Status = "Declined";
                await _context.SaveChangesAsync();
                return Ok("Order declined.");
            }

            return BadRequest("Invalid action.");
        }

        // GET: api/Washer/Orders/{washerId}
        [HttpGet("Orders/{washerId}")]
        public async Task<ActionResult> GetWasherOrders(int washerId)
        {
            var orders = await _context.Orders
                                       .Where(o => o.WasherId == washerId)
                                       .Include(o => o.User)
                                       .Include(o => o.Car)
                                       .Include(o => o.Package)
                                       .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("No orders found for this washer.");
            }

            return Ok(orders);
        }
    }

    public class RegisterWasherRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
    }
}
