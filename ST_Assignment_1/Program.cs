using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
builder.Services.AddSwaggerGen(); // Add Swagger generator

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<WorkoutJournalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Enable Swagger middleware in all environments
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at root
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
