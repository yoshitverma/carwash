using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace carwash.Model{
public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public bool IsActive { get; set; } = true; // Active/Inactive status

    [Required]
    [StringLength(20)]
    public string Role { get; set; } // "Admin", "Customer", "Washer"

    
    public List<Order> Orders { get; set; }
    
    public Profile Profile { get; set; }
}
}