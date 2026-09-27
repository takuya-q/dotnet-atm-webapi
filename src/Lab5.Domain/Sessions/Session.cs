namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

public abstract class Session
{
    public SessionId Id { get; }

    protected Session(SessionId id)
    {
        Id = id;
    }
}