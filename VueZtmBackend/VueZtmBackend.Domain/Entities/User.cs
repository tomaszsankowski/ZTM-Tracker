using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Login Login { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public RefreshToken? RefreshToken { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<UserStop> _userStops = new();
    public IReadOnlyCollection<UserStop> UserStops => _userStops.AsReadOnly();

    private User() { }

    public User(Login login, PasswordHash passwordHash)
    {
        Id = Guid.NewGuid();
        Login = login;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateRefreshToken(RefreshToken? refreshToken)
    {
        RefreshToken = refreshToken;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(PasswordHash newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStop(UserStop userStop)
    {
        _userStops.Add(userStop);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveStop(UserStop userStop)
    {
        _userStops.Remove(userStop);
        UpdatedAt = DateTime.UtcNow;
    }
}
