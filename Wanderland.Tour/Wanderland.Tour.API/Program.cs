using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Wanderland.Tour.API;
using Wanderland.Tour.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configs) =>
{
    configs
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs.txt",LogEventLevel.Debug)
        .CreateLogger();
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails(option =>
{
   
    option.Map<ApplicationException>((ctx, exception) =>
    {
        return new ProblemDetails
        {
            Title = exception.Message,
            Detail = exception.InnerException?.Message,
            Status = 400,
            Type = exception.GetType().ToString(),
            Instance = exception.ToString()
        };
    });
    option.Map<Exception>((ctx, exception) =>
    {
        return new ProblemDetails
        {
            Title = exception.Message,
            Detail = exception.InnerException?.Message,
            Status = 500,
            Type = exception.ToString(),
            Instance = exception.ToString()
        };
    });

});
builder.Services.ConfigureMassTransit();
builder.Services.AddTransient<TourReservationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
