using Application.Event;
using Application.Interface;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventManager.EventConsumer
{
    public class UserEventConsumer : Consumer
    {
        private readonly IUserService _userService;

        public UserEventConsumer(IUserService userService, ILogger<Consumer> logger) : base(logger, Environment.GetEnvironmentVariable("message_broker_user_logoff_queue") ?? string.Empty)
        {
            _userService = userService;
        }

        public override void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var str = Encoding.UTF8.GetString(e.Body.ToArray());
            var message = JsonSerializer.Deserialize<UserLogoffEvent>(str);

            _userService.Logoff(message);
        }
    }
}
