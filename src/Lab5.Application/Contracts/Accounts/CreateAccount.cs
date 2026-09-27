using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;

public class CreateAccount
{
    public record struct Request(
        SessionId SessionId,
        string AccountNumber,
        string PinCode,
        decimal InitialBalance);
}