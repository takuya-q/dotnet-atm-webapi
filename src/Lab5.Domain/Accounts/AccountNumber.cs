namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

public struct AccountNumber : IEquatable<AccountNumber>
{
    private const int MinimumLength = 6;
    private const int MaximumLength = 32;

    public string Value { get; }

    public AccountNumber(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string trimmed = value.Trim();

        if (trimmed.Length < MinimumLength)
        {
            throw new ArgumentException(
                $"Account number is too short (min {MinimumLength})",
                nameof(value));
        }

        if (trimmed.Length > MaximumLength)
        {
            throw new ArgumentException(
                $"Account number is too long (max {MaximumLength}).",
                nameof(value));
        }

        if (!trimmed.All(char.IsDigit))
            throw new ArgumentException("Account number must contain only digits", nameof(value));

        Value = trimmed;
    }

    public override string ToString()
    {
        return Value;
    }

    public bool Equals(AccountNumber other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is AccountNumber other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.Ordinal);
    }
}