using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.History;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.InMemory;

public sealed class InMemoryOperationHistoryRepository : IOperationHistoryRepository
{
    private readonly Dictionary<AccountId, List<OperationHistoryEntry>> _entriesByAccountId = new();

    public void Add(OperationHistoryEntry entry)
    {
        if (!_entriesByAccountId.TryGetValue(entry.AccountId, out List<OperationHistoryEntry>? entries))
        {
            entries = new List<OperationHistoryEntry>();
            _entriesByAccountId.Add(entry.AccountId, entries);
        }

        entries.Add(entry);
    }

    public IReadOnlyCollection<OperationHistoryEntry> GetByAccountId(AccountId accountId)
    {
        return _entriesByAccountId.TryGetValue(accountId, out List<OperationHistoryEntry>? entries)
            ? entries.ToArray()
            : Array.Empty<OperationHistoryEntry>();
    }
}