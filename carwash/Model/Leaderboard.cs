using System.ComponentModel.DataAnnotations;

namespace carwash.Model{
public class Leaderboard
{
    public int LeaderboardId { get; set; }

    [Required]
    public int WasherId { get; set; }

    public Washer Washer { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Water saved must be a non-negative value.")]
    public int WaterSavedGallons { get; set; }
}
}
