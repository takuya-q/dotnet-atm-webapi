using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Sessions;

public sealed class CreateAdminSessionUseCase
{
    private readonly ISessionRepository _sessionRepository;
    private readonly string _systemPassword;

    public CreateAdminSessionUseCase(ISessionRepository sessionRepository, string systemPassword)
    {
        ArgumentNullException.ThrowIfNull(systemPassword);

        _sessionRepository = sessionRepository;
        _systemPassword = systemPassword;
    }

    public Result<SessionId> Execute(string? systemPassword)
    {
        if (systemPassword is null)
            return new Result<SessionId>.Failure(new ErrorType.InvalidPassword());

        if (!string.Equals(systemPassword, _systemPassword, StringComparison.Ordinal))
            return new Result<SessionId>.Failure(new ErrorType.InvalidPassword());

        var sessionId = new SessionId(Guid.NewGuid());
        var session = new AdminSession(sessionId);

        _sessionRepository.Add(session);

        return new Result<SessionId>.Success(sessionId);
    }
}