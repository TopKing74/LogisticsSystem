namespace LogisticsSystem.Shared.Dtos;

public class TrackingHistoryEntryDto
{
    public int Id { get; set; }
    public string Status { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
    public int UpdatedBy { get; set; }
}