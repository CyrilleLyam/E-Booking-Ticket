namespace server.src.Dtos;

public class UpdateEventDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public int? TotalSeats { get; set; }
    public int? AvailableSeats { get; set; }
}