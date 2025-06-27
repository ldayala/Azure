using Azure.Api.Extensions;
using Azure.Persistence;
using Azure.Application;
using Core.Mappy.Interfaces;
using Core.Mappy.Extensions;
using Azure.Application.Categories.DTOs;
using Azure.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAzurePersistence(builder.Configuration);
builder.Services.AddAzureApplication();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
//agregamos la configuration de mapeo
var mapper = app.Services.GetRequiredService<IMapper>();
mapper.RegisterMappings(typeof(CategoryMappingProfile).Assembly);

// Apply migrations at startup
await app.ApplyMigration(app.Environment);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseAuthorization();

app.MapControllers();

//registramos el midleware que hemos creado para el manejo de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
