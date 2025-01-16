using System.ComponentModel.DataAnnotations;

namespace carwash.Model{
public class Profile
{
    public int ProfileId { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
}