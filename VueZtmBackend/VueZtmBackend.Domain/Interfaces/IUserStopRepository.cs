using VueZtmBackend.Domain.Entities;

namespace VueZtmBackend.Domain.Interfaces;

public interface IUserStopRepository
{
    Task<UserStop?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserStop>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, int stopId, CancellationToken cancellationToken = default);
    Task AddAsync(UserStop userStop, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserStop userStop, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserStop userStop, CancellationToken cancellationToken = default);
}

