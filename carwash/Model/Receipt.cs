using System.ComponentModel.DataAnnotations;
namespace carwash.Model{

public class Receipt
{
    public int ReceiptId { get; set; }

    [Required]
    public int OrderId { get; set; }

    public Order Order { get; set; }

    [Required]
    [Url]
    public string ImageUrl { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}
}