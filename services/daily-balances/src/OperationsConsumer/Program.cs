using DailyBalances.Api.Configuration;
using DailyBalances.Worker.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureMessageBrokerIntegration(builder.Configuration);
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureValidations();
builder.Services.ConfigureDbContext(builder.Configuration);

var host = builder.Build();

host.Run();
