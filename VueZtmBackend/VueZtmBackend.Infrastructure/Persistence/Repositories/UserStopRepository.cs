using Microsoft.EntityFrameworkCore;
using VueZtmBackend.Domain.Entities;
using VueZtmBackend.Domain.Interfaces;

namespace VueZtmBackend.Infrastructure.Persistence.Repositories;

public class UserStopRepository : IUserStopRepository
{
    private readonly AppDbContext _context;

    public UserStopRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserStop?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserStops
            .FirstOrDefaultAsync(us => us.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<UserStop>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserStops
            .Where(us => us.UserId == userId)
            .OrderBy(us => us.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid userId, int stopId, CancellationToken cancellationToken = default)
    {
        return await _context.UserStops
            .AnyAsync(us => us.UserId == userId && us.StopId == stopId, cancellationToken);
    }

    public async Task AddAsync(UserStop userStop, CancellationToken cancellationToken = default)
    {
        await _context.UserStops.AddAsync(userStop, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserStop userStop, CancellationToken cancellationToken = default)
    {
        _context.UserStops.Update(userStop);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(UserStop userStop, CancellationToken cancellationToken = default)
    {
        _context.UserStops.Remove(userStop);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

