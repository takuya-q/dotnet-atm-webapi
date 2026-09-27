using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Time;
using Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLab5Infrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddSingleton<ISessionRepository, InMemorySessionRepository>();
        services.AddSingleton<IOperationHistoryRepository, InMemoryOperationHistoryRepository>();

        services.AddSingleton<ITimeProvider, InMemoryTimeProvider>();

        return services;
    }
}