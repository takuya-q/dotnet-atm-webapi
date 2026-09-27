using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.History;

public class GetOperationHistory
{
    public record struct Request(SessionId SessionId);
}