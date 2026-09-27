using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;

public class GetBalance
{
    public record struct Request(SessionId SessionId);
}