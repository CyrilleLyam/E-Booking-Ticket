using Microsoft.EntityFrameworkCore;
using server.src.Dtos;
using server.src.Models;
using server.src.Data;

namespace server.src.Repositories;

public class EventRepository : IEventRepository
{
        private readonly AppDbContext _context;
    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Event> Create(Event eventEntity)
    {
         _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();
        return eventEntity;
    }

    public async Task Delete(int id)
    {
        var eventEntity = await _context.Events.FindAsync(id);
        if (eventEntity != null)
        {
            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(IEnumerable<Event> Items, int TotalCount)> GetAll(EventQueryDto queryDto)
    {
        var query = _context.Events.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(queryDto.Name))
        {
            query = query.Where(e => EF.Functions.ILike(e.Name, $"%{queryDto.Name.Trim()}%"));
        }

        if (queryDto.StartDate.HasValue)
        {
            query = query.Where(e => e.StartDate >= queryDto.StartDate.Value);
        }

        if (queryDto.EndDate.HasValue)
        {
            query = query.Where(e => e.EndDate <= queryDto.EndDate.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .ThenBy(e => e.Id)
            .Skip((queryDto.Page - 1) * queryDto.PageSize)
            .Take(queryDto.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Event?> GetById(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task Update(Event eventEntity)
    {
        _context.Events.Update(eventEntity);
        await _context.SaveChangesAsync();
    }
}