using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.History;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Dtos;
using Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.History;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.ObjectOrientedProgramming.Lab5.Presentation.Controllers;

[ApiController]
[Route("history")]
public sealed class HistoryController : ControllerBase
{
    private readonly GetOperationHistoryUseCase _getOperationHistoryUseCase;

    public HistoryController(GetOperationHistoryUseCase getOperationHistoryUseCase)
    {
        _getOperationHistoryUseCase = getOperationHistoryUseCase;
    }

    [HttpPost("list")]
    public ActionResult<IReadOnlyCollection<OperationHistoryEntryDto>> GetHistory(
        [FromBody] SessionHttpRequest httpRequest)
    {
        var request = new GetOperationHistory.Request(new SessionId(httpRequest.SessionId));
        Result<IReadOnlyCollection<OperationHistoryEntryDto>> result = _getOperationHistoryUseCase.Execute(request);

        return result switch
        {
            Result<IReadOnlyCollection<OperationHistoryEntryDto>>.Success success => Ok(success.Value),
            Result<IReadOnlyCollection<OperationHistoryEntryDto>>.Failure failure => MapFailure(failure.ErrorType),
            _ => BadRequest(),
        };
    }

    private ActionResult<IReadOnlyCollection<OperationHistoryEntryDto>> MapFailure(ErrorType error)
    {
        return error switch
        {
            ErrorType.Unauthorized => Unauthorized(),
            ErrorType.SessionNotFound => Unauthorized(),
            _ => BadRequest(),
        };
    }

    public sealed record SessionHttpRequest(Guid SessionId);
}