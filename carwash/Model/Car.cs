using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace carwash.Model{
public class Car
{
    public int CarId { get; set; }

    [Required]
    [StringLength(100)]
    public string Make { get; set; }

    [Required]
    [StringLength(100)]
    public string Model { get; set; }

    [Required]
    [StringLength(15)] // Depending on the license plate format
    public string LicensePlate { get; set; }

    [Url]
    public string ImageUrl { get; set; } // Image URL for the car photo

    public int UserId { get; set; }
[JsonIgnore]
    public User? User { get; set; }
}
}