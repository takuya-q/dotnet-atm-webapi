using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Dtos;
using Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.ObjectOrientedProgramming.Lab5.Presentation.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly CreateAccountUseCase _createAccountUseCase;
    private readonly GetBalanceUseCase _getBalanceUseCase;
    private readonly DepositCashUseCase _depositCashUseCase;
    private readonly WithdrawCashUseCase _withdrawCashUseCase;

    public AccountsController(
        CreateAccountUseCase createAccountUseCase,
        GetBalanceUseCase getBalanceUseCase,
        DepositCashUseCase depositCashUseCase,
        WithdrawCashUseCase withdrawCashUseCase)
    {
        _createAccountUseCase = createAccountUseCase;
        _getBalanceUseCase = getBalanceUseCase;
        _depositCashUseCase = depositCashUseCase;
        _withdrawCashUseCase = withdrawCashUseCase;
    }

    [HttpPost("create")]
    public ActionResult<AccountDto> CreateAccount([FromBody] CreateAccountHttpRequest httpRequest)
    {
        var request = new CreateAccount.Request(
            new SessionId(httpRequest.SessionId),
            httpRequest.AccountNumber,
            httpRequest.PinCode,
            httpRequest.InitialBalance);

        Result<AccountDto> result = _createAccountUseCase.Execute(request);

        return result switch
        {
            Result<AccountDto>.Success success => Ok(success.Value),
            Result<AccountDto>.Failure failure => MapFailure<AccountDto>(failure.ErrorType),
            _ => BadRequest(),
        };
    }

    [HttpPost("balance")]
    public ActionResult<BalanceResponse> GetBalance([FromBody] SessionHttpRequest httpRequest)
    {
        var request = new GetBalance.Request(new SessionId(httpRequest.SessionId));
        Result<decimal> result = _getBalanceUseCase.Execute(request);

        return result switch
        {
            Result<decimal>.Success success => Ok(new BalanceResponse(success.Value)),
            Result<decimal>.Failure failure => MapFailure<BalanceResponse>(failure.ErrorType),
            _ => BadRequest(),
        };
    }

    [HttpPost("deposit")]
    public ActionResult<BalanceResponse> Deposit([FromBody] AmountHttpRequest httpRequest)
    {
        var request = new DepositCash.Request(new SessionId(httpRequest.SessionId), httpRequest.Amount);
        Result<decimal> result = _depositCashUseCase.Execute(request);

        return result switch
        {
            Result<decimal>.Success success => Ok(new BalanceResponse(success.Value)),
            Result<decimal>.Failure failure => MapFailure<BalanceResponse>(failure.ErrorType),
            _ => BadRequest(),
        };
    }

    [HttpPost("withdraw")]
    public ActionResult<BalanceResponse> Withdraw([FromBody] AmountHttpRequest httpRequest)
    {
        var request = new WithdrawCash.Request(new SessionId(httpRequest.SessionId), httpRequest.Amount);
        Result<decimal> result = _withdrawCashUseCase.Execute(request);

        return result switch
        {
            Result<decimal>.Success success => Ok(new BalanceResponse(success.Value)),
            Result<decimal>.Failure failure => MapFailure<BalanceResponse>(failure.ErrorType),
            _ => BadRequest(),
        };
    }

    private ActionResult<T> MapFailure<T>(ErrorType error)
    {
        return error switch
        {
            ErrorType.Unauthorized => Unauthorized(),
            ErrorType.SessionNotFound => Unauthorized(),
            _ => BadRequest(),
        };
    }

    public record CreateAccountHttpRequest(
        Guid SessionId,
        string AccountNumber,
        string PinCode,
        decimal InitialBalance);

    public record SessionHttpRequest(Guid SessionId);

    public record AmountHttpRequest(Guid SessionId, decimal Amount);

    public record BalanceResponse(decimal Balance);
}