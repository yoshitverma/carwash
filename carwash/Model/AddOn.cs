using System.ComponentModel.DataAnnotations;
namespace carwash.Model{
public class AddOn
{
    public int AddONId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }
}
}