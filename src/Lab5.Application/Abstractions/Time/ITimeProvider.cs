namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Time;

public interface ITimeProvider
{
    DateTimeOffset GetUtcNow();
}