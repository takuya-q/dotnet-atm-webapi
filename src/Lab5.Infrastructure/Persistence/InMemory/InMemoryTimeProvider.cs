using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Time;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.InMemory;

public sealed class InMemoryTimeProvider : ITimeProvider
{
    public DateTimeOffset GetUtcNow()
    {
        return DateTimeOffset.UtcNow;
    }
}