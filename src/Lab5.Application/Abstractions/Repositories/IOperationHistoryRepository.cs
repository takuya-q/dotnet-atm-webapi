using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.History;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;

public interface IOperationHistoryRepository
{
    void Add(OperationHistoryEntry entry);

    IReadOnlyCollection<OperationHistoryEntry> GetByAccountId(AccountId accountId);
}