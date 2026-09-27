namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

public readonly struct SessionId : IEquatable<SessionId>
{
    public Guid Value { get; }

    public SessionId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Session id must not be empty", nameof(value));

        Value = value;
    }

    public bool Equals(SessionId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is SessionId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}