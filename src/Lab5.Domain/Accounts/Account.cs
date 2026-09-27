namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

public class Account
{
    private readonly PinCode _pinCode;

    public AccountId Id { get; }

    public AccountNumber Number { get; }

    public Money Balance { get; private set; }

    public Account(AccountId id, AccountNumber number, PinCode pinCode, Money initialBalance)
    {
        Id = id;
        Number = number;
        _pinCode = pinCode;
        Balance = initialBalance;
    }

    public bool IsPinValid(PinCode pinCode)
    {
        return _pinCode.Equals(pinCode);
    }

    public void Deposit(Money amount)
    {
        Balance = Balance.Add(amount);
    }

    public void Withdraw(Money amount)
    {
        Balance = Balance.Subtract(amount);
    }
}