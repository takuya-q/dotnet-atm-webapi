namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

public struct AccountId : IEquatable<AccountId>
{
    private Guid Value { get; }

    public AccountId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("AccountId cannot be empty", nameof(value));

        Value = value;
    }

    public bool Equals(AccountId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is AccountId other && Equals(other);
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