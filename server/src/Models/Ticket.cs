namespace server.src.Models;

public enum TicketStatus
{
    Available,
    Held,
    Booked,
    Used,
    Canceled
}

public class Ticket
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public int? OrderId { get; set; }
    public Order? Order { get; set; }
    public string SeatIdentifier { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Available;
    public DateTime? HeldUntil { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}