using Application.Interface;
using Application.Service;
using EventManager.EventConsumer;
using Infrastructure.Context;
using Infrastructure.Interface;
using Infrastructure.MessageBroker;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMessageEventPublisher, MessageEventPublisher>();
builder.Services.AddTransient<IChatRoomEventPublisher, ChatRoomEventPublisher>();
builder.Services.AddTransient<IUserEventPublisher, UserEventPublisher>();

builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IChatRoomService, ChatRoomService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddHostedService<MessageEventConsumer>();
builder.Services.AddHostedService<ChatRoomEventConsumer>();
builder.Services.AddHostedService<UserEventConsumer>();

builder.Services.AddDbContext<ChatRoomContext>(options =>
{
    options.UseSqlite(Environment.GetEnvironmentVariable("db_file_path"));
});

builder.Services.AddTransient<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

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
