using DailyBalances.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureValidations();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program {}
