using Microsoft.AspNetCore.Mvc;

namespace server.src.Dtos;

public class EventQueryDto : PaginationQueryDto
{
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    [FromQuery(Name = "start_date")]
    public DateTime? StartDate { get; set; }

    [FromQuery(Name = "end_date")]
    public DateTime? EndDate { get; set; }
}
