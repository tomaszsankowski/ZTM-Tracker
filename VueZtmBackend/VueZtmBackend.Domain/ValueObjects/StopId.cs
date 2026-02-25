using VueZtmBackend.Domain.Exceptions;

namespace VueZtmBackend.Domain.ValueObjects;

public sealed class StopId : IEquatable<StopId>
{
    public int Value { get; }

    public StopId(int value)
    {
        if (value <= 0)
            throw new DomainException("StopId musi być dodatnią liczbą całkowitą.");

        Value = value;
    }

    public bool Equals(StopId? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as StopId);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static implicit operator int(StopId stopId) => stopId.Value;
}

