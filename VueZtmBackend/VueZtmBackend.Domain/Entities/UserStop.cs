using VueZtmBackend.Domain.Exceptions;
using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Domain.Entities;

public class UserStop
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public StopId StopId { get; private set; } = null!;
    public string? CustomName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public User User { get; private set; } = null!;

    private UserStop() { }

    public UserStop(Guid userId, StopId stopId, string? customName = null)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId nie może być pusty.");

        Id = Guid.NewGuid();
        UserId = userId;
        StopId = stopId;
        CustomName = customName?.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateCustomName(string? customName)
    {
        CustomName = customName?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStopId(StopId stopId)
    {
        StopId = stopId;
        UpdatedAt = DateTime.UtcNow;
    }
}

