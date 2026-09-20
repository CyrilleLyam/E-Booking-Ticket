using server.src.Dtos;
using server.src.Models;

namespace server.src.Repositories;

public interface IEventRepository
{
    Task<(IEnumerable<Event> Items, int TotalCount)> GetAll(EventQueryDto queryDto);
    Task<Event?> GetById(int id);
    Task<Event> Create(Event eventEntity);
    Task Update(Event eventEntity);
    Task Delete(int id);
}