namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.History;

public readonly struct OperationHistoryEntryId : IEquatable<OperationHistoryEntryId>
{
    public Guid Value { get; }

    public OperationHistoryEntryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("OperationHistoryEntryId cannot be empty", nameof(value));

        Value = value;
    }

    public bool Equals(OperationHistoryEntryId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is OperationHistoryEntryId other && Equals(other);
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