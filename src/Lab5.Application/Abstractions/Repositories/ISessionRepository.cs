using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;

public interface ISessionRepository
{
    Session? FindById(SessionId sessionId);

    void Add(Session session);
}