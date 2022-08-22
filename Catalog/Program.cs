using Catalog;
using Catalog.Controllers;
using Catalog.Models;
using Catalog.Repository;
using Catalog.Repository.Impl;
using Catalog.Service;
using Catalog.Service.Impl;
using Email.Contracts.Commands;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

ConnectionFactory connectionFactory = new() { HostName = "localhost" };

builder.Services.AddSingleton(connectionFactory);

builder.Services.AddScoped<ICommandQueue<SendEmail>>(sp => new CommandQueue<SendEmail>(
    connectionFactory: sp.GetService<ConnectionFactory>() ?? throw new ArgumentException("connectionFactory"),
    exchangeName: "email.commands", 
    routingKey: "send_email"));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRepository<Guid, Product>, Repository<Guid, Product, CatalogContext>>();

builder.Services.AddDbContext<CatalogContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDb")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
