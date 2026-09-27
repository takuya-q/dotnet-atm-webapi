using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Time;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.History;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public sealed class DepositCashUseCaseTests
{
    [Fact]
    public void Execute_WhenAmountIsValid_ShouldUpdateAccountAndAddHistoryEntry()
    {
        var sessionRepository = new FakeSessionRepository();
        var accountRepository = new SpyAccountRepository();
        var historyRepository = new SpyOperationHistoryRepository();
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 12, 19, 10, 0, 0, TimeSpan.Zero));

        var useCase = new DepositCashUseCase(
            sessionRepository,
            accountRepository,
            historyRepository,
            timeProvider);

        var accountId = new AccountId(Guid.NewGuid());
        var account = new Account(
            accountId,
            new AccountNumber("123456"),
            new PinCode("1234"),
            new Money(100m));

        accountRepository.Seed(account);

        var sessionId = new SessionId(Guid.NewGuid());
        sessionRepository.Seed(new UserSession(sessionId, accountId));

        var request = new DepositCash.Request(sessionId, 50m);

        Result<decimal> result = useCase.Execute(request);

        Result<decimal>.Success success = Assert.IsType<Result<decimal>.Success>(result);
        Assert.Equal(150m, success.Value);

        Assert.True(accountRepository.UpdateWasCalled);
        Account updatedAccount = Assert.IsType<Account>(accountRepository.LastUpdatedAccount);
        Assert.Equal(150m, updatedAccount.Balance.Value);

        Assert.True(historyRepository.AddWasCalled);
        OperationHistoryEntry entry = Assert.IsType<OperationHistoryEntry>(historyRepository.LastAddedEntry);

        Assert.Equal(accountId, entry.AccountId);
        Assert.IsType<OperationType.CashDeposited>(entry.Type);
        Assert.Equal(50m, entry.Amount.Value);
        Assert.Equal(150m, entry.BalanceAfter.Value);
        Assert.Equal(timeProvider.UtcNow, entry.OccurredAt);
    }

    private sealed class FakeTimeProvider : ITimeProvider
    {
        public FakeTimeProvider(DateTimeOffset utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTimeOffset UtcNow { get; }

        public DateTimeOffset GetUtcNow()
        {
            return UtcNow;
        }
    }

    private sealed class FakeSessionRepository : ISessionRepository
    {
        private readonly Dictionary<SessionId, Session> _sessions = new();

        public void Seed(Session session)
        {
            _sessions[session.Id] = session;
        }

        public Session? FindById(SessionId sessionId)
        {
            return _sessions.GetValueOrDefault(sessionId);
        }

        public void Add(Session session)
        {
            _sessions[session.Id] = session;
        }
    }

    private sealed class SpyAccountRepository : IAccountRepository
    {
        private readonly Dictionary<AccountId, Account> _accountsById = new();
        private readonly Dictionary<AccountNumber, AccountId> _accountIdsByNumber = new();

        public bool UpdateWasCalled { get; private set; }

        public Account? LastUpdatedAccount { get; private set; }

        public void Seed(Account account)
        {
            _accountsById[account.Id] = account;
            _accountIdsByNumber[account.Number] = account.Id;
        }

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
            Seed(account);
        }

        public void Update(Account account)
        {
            UpdateWasCalled = true;
            LastUpdatedAccount = account;

            _accountsById[account.Id] = account;
            _accountIdsByNumber[account.Number] = account.Id;
        }
    }

    private sealed class SpyOperationHistoryRepository : IOperationHistoryRepository
    {
        private readonly Dictionary<AccountId, List<OperationHistoryEntry>> _entries = new();

        public bool AddWasCalled { get; private set; }

        public OperationHistoryEntry? LastAddedEntry { get; private set; }

        public void Add(OperationHistoryEntry entry)
        {
            AddWasCalled = true;
            LastAddedEntry = entry;

            if (!_entries.TryGetValue(entry.AccountId, out List<OperationHistoryEntry>? list))
            {
                list = new List<OperationHistoryEntry>();
                _entries.Add(entry.AccountId, list);
            }

            list.Add(entry);
        }

        public IReadOnlyCollection<OperationHistoryEntry> GetByAccountId(AccountId accountId)
        {
            return _entries.TryGetValue(accountId, out List<OperationHistoryEntry>? list)
                ? list.ToArray()
                : Array.Empty<OperationHistoryEntry>();
        }
    }
}