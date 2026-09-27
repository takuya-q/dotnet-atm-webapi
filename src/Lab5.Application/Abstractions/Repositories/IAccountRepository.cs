using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;

public interface IAccountRepository
{
    Account? FindById(AccountId accountId);

    Account? FindByNumber(AccountNumber accountNumber);

    void Add(Account account);

    void Update(Account account);
}