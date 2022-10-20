using FluentValidation.AspNetCore;
using LatencyService.Api.Handlers;
using LatencyService.Api.Validators;
using LatencyService.Domain;
using LatencyService.Infrastructure.LatencyApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LatencyRequestModalValidator>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ILatencyHandler, LatencyHandler>();
builder.Services.AddTransient<ILatencyDataProcessor, LatencyDataProcessor>();
builder.Services.AddTransient<ILatencyServiceClient, LatencyServiceClient>();
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
