namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

public struct PinCode : IEquatable<PinCode>
{
    private const int RequiredLength = 4;

    public string Value { get; }

    public PinCode(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string trimmed = value.Trim();

        if (trimmed.Length != RequiredLength)
            throw new ArgumentException($"PIN must be {RequiredLength} digits", nameof(value));

        if (!trimmed.All(char.IsDigit))
            throw new ArgumentException("PIN must contain only digits", nameof(value));

        Value = trimmed;
    }

    public override string ToString()
    {
        return "****";
    }

    public bool Equals(PinCode other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is PinCode other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.Ordinal);
    }
}