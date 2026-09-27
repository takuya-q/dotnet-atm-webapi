using Itmo.ObjectOrientedProgramming.Lab5.Application.DependencyInjection;
using Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string systemPassword =
    builder.Configuration["SystemPassword"] ??
    Environment.GetEnvironmentVariable("ATM_SYSTEM_PASSWORD") ??
    "ChangeMe";

builder.Services.AddLab5Infrastructure();
builder.Services.AddLab5Application(systemPassword);

WebApplication app = builder.Build();

app.MapControllers();

app.Run();