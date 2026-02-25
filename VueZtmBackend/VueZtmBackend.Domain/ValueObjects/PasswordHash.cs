using VueZtmBackend.Domain.Exceptions;

namespace VueZtmBackend.Domain.ValueObjects;

public sealed class PasswordHash : IEquatable<PasswordHash>
{
    public string Value { get; }

    public PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Hash hasła nie może być pusty.");

        if (value.Length < 20)
            throw new DomainException("Nieprawidłowy format hasha hasła.");

        Value = value;
    }

    public bool Equals(PasswordHash? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as PasswordHash);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => "***";

    public static implicit operator string(PasswordHash hash) => hash.Value;
}

