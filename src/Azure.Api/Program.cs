using Azure.Api.Extensions;
using Azure.Persistence;
using Azure.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAzurePersistence(builder.Configuration);
builder.Services.AddAzureApplication();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
// Apply migrations at startup
await app.ApplyMigration(app.Environment);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseAuthorization();

app.MapControllers();


app.Run();
