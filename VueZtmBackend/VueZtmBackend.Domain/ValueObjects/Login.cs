using VueZtmBackend.Domain.Exceptions;

namespace VueZtmBackend.Domain.ValueObjects;

public sealed class Login : IEquatable<Login>
{
    public string Value { get; }

    public Login(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Login nie może być pusty.");

        if (value.Length < 3)
            throw new DomainException("Login musi mieć co najmniej 3 znaki.");

        if (value.Length > 50)
            throw new DomainException("Login nie może mieć więcej niż 50 znaków.");

        if (!value.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-'))
            throw new DomainException("Login może zawierać tylko litery, cyfry, podkreślenia i myślniki.");

        Value = value;
    }

    public bool Equals(Login? other)
    {
        if (other is null) return false;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as Login);

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

    public override string ToString() => Value;

    public static implicit operator string(Login login) => login.Value;
}

