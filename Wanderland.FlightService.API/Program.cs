using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wanderland.Flight.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails(option =>
{
    option.Map<DomainException>((ctx, exception) =>
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
