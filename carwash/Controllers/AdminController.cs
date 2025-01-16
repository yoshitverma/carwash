using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using carwash.Migrations;
using carwash.Model;
using carwash.Services;

namespace carwash.Controllers
{

 [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Ensure only Admin can access these routes
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CRUD for Packages

        // GET: api/Admin/Packages
        [HttpGet("Packages")]
        public async Task<ActionResult> GetPackages()
        {
            var packages = await _context.Packages.Include(p => p.AddOns).ToListAsync();
            return Ok(packages);
        }

        // POST: api/Admin/Packages
        [HttpPost("Packages")]
        public async Task<ActionResult> CreatePackage(Package package)
        {
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPackages", new { id = package.PackageId }, package);
        }

        // PUT: api/Admin/Packages/5
        [HttpPut("Packages/{id}")]
        public async Task<ActionResult> UpdatePackage(int id, Package package)
        {
            if (id != package.PackageId)
            {
                return BadRequest();
            }

            _context.Entry(package).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Admin/Packages/5
        [HttpDelete("Packages/{id}")]
        public async Task<ActionResult> DeletePackage(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Admin/Users/5
        [HttpDelete("Users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Admin/Washers/5
        [HttpDelete("Washers/{id}")]
        public async Task<ActionResult> DeleteWasher(int id)
        {
            var washer = await _context.Washers.FindAsync(id);
            if (washer == null)
            {
                return NotFound();
            }

            _context.Washers.Remove(washer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Assign Orders to Washers based on Location (Pending Orders)
        [HttpPost("AssignOrders")]
        public async Task<ActionResult> AssignOrdersToWashers([FromBody] int washerId)
        {
            var washer = await _context.Washers.FindAsync(washerId);
            if (washer == null)
            {
                return NotFound("Washer not found.");
            }

            var pendingOrders = await _context.Orders
                                              .Where(o => o.Status == "Pending" && o.Car.LicensePlate == washer.address) // Match location logic
                                              .ToListAsync();

            if (!pendingOrders.Any())
            {
                return NotFound("No pending orders found for this washer.");
            }

            foreach (var order in pendingOrders)
            {
                order.WasherId = washerId;
                order.Status = "Assigned";
            }

            await _context.SaveChangesAsync();

            return Ok(pendingOrders);
        }
    }
}