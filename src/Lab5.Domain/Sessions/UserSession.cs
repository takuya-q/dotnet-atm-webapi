using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

public sealed class UserSession : Session
{
    public AccountId AccountId { get; }

    public UserSession(SessionId id, AccountId accountId)
        : base(id)
    {
        AccountId = accountId;
    }
}