using LatencyService.Api.Handlers;
using LatencyService.Domain;
using LatencyService.Infrastructure.LatencyApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ILatencyHandler, LatencyHandler>();
builder.Services.AddTransient<ILatencyCalculator, LatencyCalculator>();
builder.Services.AddHttpClient<LatencyServiceClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
