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

//namespace carwash.Controllers
//{
// [Route("api/cars")]
// [ApiController]
// public class CarController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;
//     private readonly JwtService _jwtService;

//     public CarController(ApplicationDbContext context, JwtService jwtService)
//     {
//         _context = context;
//         _jwtService = jwtService;
//     }

//     [HttpPost("add")]
//     public ActionResult<Car> AddCar([FromBody] Car car)
//     {
//         var userId = _jwtService.GetUserIdFromJwtToken(Request.Headers["Authorization"]);
//         car.UserId = userId;
//         _context.Cars.Add(car);
//         _context.SaveChanges();
//         return Ok(car);
//     }
// }
// }

namespace carwash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Car/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Car>> CreateCar([FromBody] Car car)
        {
            // Check if the user exists
            var user = await _context.Users.FindAsync(car.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Add the car to the database
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { carId = car.CarId }, car);
        }

        // GET: api/Car/{carId}
        [HttpGet("{carId}")]
        public async Task<ActionResult<Car>> GetCar(int carId)
        {
            var car = await _context.Cars.Include(c => c.User)
                                          .FirstOrDefaultAsync(c => c.CarId == carId);

            if (car == null)
            {
                return NotFound("Car not found.");
            }

            return Ok(car);
        }

        // GET: api/Car/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Car>>> GetUserCars(int userId)
        {
            var cars = await _context.Cars.Where(c => c.UserId == userId).ToListAsync();

            if (!cars.Any())
            {
                return NotFound("No cars found for this user.");
            }

            return Ok(cars);
        }

        // PUT: api/Car/{carId}
        [HttpPut("{carId}")]
        public async Task<ActionResult> UpdateCar(int carId, [FromBody] Car car)
        {
            if (carId != car.CarId)
            {
                return BadRequest("Car ID mismatch.");
            }

            // Check if the car exists
            var existingCar = await _context.Cars.FindAsync(carId);
            if (existingCar == null)
            {
                return NotFound("Car not found.");
            }

            // Update car details
            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.LicensePlate = car.LicensePlate;
            existingCar.ImageUrl = car.ImageUrl;
            existingCar.UserId = car.UserId;

            await _context.SaveChangesAsync();
            return NoContent(); // Return a successful response with no content
        }

        // DELETE: api/Car/{carId}
        [HttpDelete("{carId}")]
        public async Task<ActionResult> DeleteCar(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return NotFound("Car not found.");
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent(); // Return a successful response with no content
        }
    }
}

