using BCMS.Application.Interfaces;
using BCMS.Application.Services;
using BCMS.Domain;
using BCMS.Domain.IRepositories;
using BCMS.Infrastructure;
using BCMS.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:62882")  // Angular app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();  
        });
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Repositories
builder.Services.AddScoped<IBusinessCardRepository, BusinessCardRepository>();

// Add MediatR and FluentValidation
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateBusinessCardHandler).Assembly);
});

builder.Services.AddScoped<IFileParserService, FileParserService>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Add controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BCMS API V1");
        c.RoutePrefix = string.Empty; // Swagger at root: https://localhost:7294/
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
