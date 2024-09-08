using SimpleAuth.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var authorityOptions = builder.Configuration
    .GetSection(AuthorityOptions.Section)
    .Get<AuthorityOptions>()
    ?? throw new InvalidOperationException(
        $"No {AuthorityOptions.Section} section found in configuration.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(authorityOptions);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
