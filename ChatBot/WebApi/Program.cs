using Application.Interface;
using Application.Interfaces;
using Application.Service;
using Application.Services;
using EventManager.EventConsumer;
using Infrastructure.Interface;
using Infrastructure.MessageBroker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMessageEventPublisher, MessageEventPublisher>();

builder.Services.AddTransient<IMessageService, MessageService>();

builder.Services.AddTransient<IStockApiService, StockApiService>();

builder.Services.AddHostedService<MessageEventConsumer>();

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
