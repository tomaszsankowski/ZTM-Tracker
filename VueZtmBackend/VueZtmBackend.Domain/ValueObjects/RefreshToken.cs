using VueZtmBackend.Domain.Exceptions;

namespace VueZtmBackend.Domain.ValueObjects;

public sealed class RefreshToken(string token, DateTime expiresAt, DateTime createdAt) : IEquatable<RefreshToken>
{
    public string Token { get; } = token;
    public DateTime ExpiresAt { get; } = expiresAt;
    public DateTime CreatedAt { get; } = createdAt;

    public static RefreshToken Create(string token, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("Refresh token nie może być pusty.");

        if (expiresAt <= DateTime.UtcNow)
            throw new DomainException("Data wygaśnięcia refresh tokena musi być w przyszłości.");

        return new RefreshToken(token, expiresAt, DateTime.UtcNow);
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool Equals(RefreshToken? other)
    {
        if (other is null) return false;
        return Token == other.Token;
    }

    public override bool Equals(object? obj) => Equals(obj as RefreshToken);

    public override int GetHashCode() => Token.GetHashCode();

    public override string ToString() => Token;
}
