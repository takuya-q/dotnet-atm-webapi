using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.History;
using Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLab5Application(this IServiceCollection services, string systemPassword)
    {
        ArgumentNullException.ThrowIfNull(systemPassword);

        services.AddTransient(provider =>
            new CreateAdminSessionUseCase(
                provider.GetRequiredService<ISessionRepository>(),
                systemPassword));

        services.AddTransient<CreateUserSessionUseCase>();

        services.AddTransient<CreateAccountUseCase>();
        services.AddTransient<GetBalanceUseCase>();
        services.AddTransient<DepositCashUseCase>();
        services.AddTransient<WithdrawCashUseCase>();

        services.AddTransient<GetOperationHistoryUseCase>();

        return services;
    }
}