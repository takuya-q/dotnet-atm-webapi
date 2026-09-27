namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

public readonly struct Money : IEquatable<Money>
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (value < 0m)
            throw new ArgumentOutOfRangeException(nameof(value), "Amount cannot be negative");

        Value = value;
    }

    public bool IsZero()
    {
        return Value == 0m;
    }

    public bool IsLessThan(Money other)
    {
        return Value < other.Value;
    }

    public Money Add(Money other)
    {
        decimal sum = Value + other.Value;
        return new Money(sum);
    }

    public Money Subtract(Money other)
    {
        decimal result = Value - other.Value;

        if (result < 0m)
            throw new InvalidOperationException("Insufficient funds");

        return new Money(result);
    }

    public bool Equals(Money other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Money other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString("0.##");
    }
}