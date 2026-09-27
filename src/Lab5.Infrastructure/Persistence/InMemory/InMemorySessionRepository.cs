using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.InMemory;

public sealed class InMemorySessionRepository : ISessionRepository
{
    private readonly Dictionary<SessionId, Session> _sessions = new();

    public Session? FindById(SessionId sessionId)
    {
        return _sessions.GetValueOrDefault(sessionId);
    }

    public void Add(Session session)
    {
        if (!_sessions.TryAdd(session.Id, session))
            throw new InvalidOperationException("Session already exists");
    }
}