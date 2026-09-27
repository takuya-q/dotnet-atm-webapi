using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.InMemory;

public sealed class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<AccountId, Account> _accountsById = new();
    private readonly Dictionary<AccountNumber, AccountId> _accountIdsByNumber = new();

    public Account? FindById(AccountId accountId)
    {
        return _accountsById.GetValueOrDefault(accountId);
    }

    public Account? FindByNumber(AccountNumber accountNumber)
    {
        if (!_accountIdsByNumber.TryGetValue(accountNumber, out AccountId accountId))
            return null;

        return FindById(accountId);
    }

    public void Add(Account account)
    {
        if (_accountsById.ContainsKey(account.Id))
            throw new InvalidOperationException("Account with same id already exists");

        if (_accountIdsByNumber.ContainsKey(account.Number))
            throw new InvalidOperationException("Account with same number already exists");

        _accountsById.Add(account.Id, account);
        _accountIdsByNumber.Add(account.Number, account.Id);
    }

    public void Update(Account account)
    {
        if (!_accountsById.TryGetValue(account.Id, out Account? existing))
            throw new InvalidOperationException("Account does not exist");

        if (!existing.Number.Equals(account.Number))
        {
            _accountIdsByNumber.Remove(existing.Number);

            if (_accountIdsByNumber.ContainsKey(account.Number))
                throw new InvalidOperationException("Account with same number already exists");

            _accountIdsByNumber.Add(account.Number, account.Id);
        }

        _accountsById[account.Id] = account;
    }
}