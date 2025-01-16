using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace carwash.Model{

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int UserId { get; set; }  // Foreign key for User
    [JsonIgnore]
    public User? User { get; set; }

    
    public int? WasherId { get; set; }  // Foreign key for Washer
    [JsonIgnore]
    public Washer? Washer { get; set; }  // Reference to Washer entity

    [Required]
    public int CarId { get; set; }
    [JsonIgnore]
    public Car? Car { get; set; }

    [Required]
    public int PackageId { get; set; }
    [JsonIgnore]
    public Package? Package { get; set; }

    [Required]
    [StringLength(20)] // e.g., "Pending", "Accepted", "Completed"
    public string Status { get; set; }

    public DateTime? ScheduledTime { get; set; }

    [StringLength(500)]
    public string Notes { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }
}

}
