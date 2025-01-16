using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace carwash.Model{
public class Washer
{
    [Key]
    public int WasherId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string Contact { get; set; }

    public string address { get; set; }
    [Range(0, 5)]
    public double Rating { get; set; }
}
}