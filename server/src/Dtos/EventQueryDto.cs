using Microsoft.AspNetCore.Mvc;

namespace server.src.Dtos;

public class EventQueryDto : PaginationQueryDto
{
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    [FromQuery(Name = "start_time")]
    public DateTime? StartTime { get; set; }
}
