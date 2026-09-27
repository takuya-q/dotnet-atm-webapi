using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Sessions;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.ObjectOrientedProgramming.Lab5.Presentation.Controllers;

[ApiController]
[Route("sessions")]
public class SessionsController : ControllerBase
{
    private readonly CreateAdminSessionUseCase _createAdminSessionUseCase;
    private readonly CreateUserSessionUseCase _createUserSessionUseCase;

    public SessionsController(
        CreateAdminSessionUseCase createAdminSessionUseCase,
        CreateUserSessionUseCase createUserSessionUseCase)
    {
        _createAdminSessionUseCase = createAdminSessionUseCase;
        _createUserSessionUseCase = createUserSessionUseCase;
    }

    [HttpPost("admin")]
    public ActionResult<CreateSessionResponse> CreateAdminSession([FromBody] CreateAdminSessionHttpRequest httpRequest)
    {
        Result<SessionId> result = _createAdminSessionUseCase.Execute(httpRequest.SystemPassword);

        return MapSessionResult(result);
    }

    [HttpPost("user")]
    public ActionResult<CreateSessionResponse> CreateUserSession([FromBody] CreateUserSessionHttpRequest httpRequest)
    {
        Result<SessionId> result = _createUserSessionUseCase.Execute(httpRequest.AccountNumber, httpRequest.PinCode);

        return MapSessionResult(result);
    }

    private ActionResult<CreateSessionResponse> MapSessionResult(Result<SessionId> result)
    {
        return result switch
        {
            Result<SessionId>.Success success => Ok(new CreateSessionResponse(success.Value.Value)),
            Result<SessionId>.Failure failure => MapFailure(failure.ErrorType),
            _ => BadRequest(),
        };
    }

    private ActionResult<CreateSessionResponse> MapFailure(ErrorType error)
    {
        return error switch
        {
            ErrorType.InvalidPassword => Unauthorized(),
            ErrorType.InvalidCredentials => Unauthorized(),
            ErrorType.AccountNotFound => Unauthorized(),
            _ => BadRequest(),
        };
    }

    public record CreateAdminSessionHttpRequest(string SystemPassword);

    public record CreateUserSessionHttpRequest(string AccountNumber, string PinCode);

    public record CreateSessionResponse(Guid SessionId);
}