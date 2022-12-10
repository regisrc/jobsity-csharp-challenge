using Infrastructure.Event;
using Infrastructure.MessageBroker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.EventConsumer
{
    public class MessageEventConsumer : Consumer
    {
        public MessageEventConsumer(ILogger<Consumer> logger) : base(logger)
        {
        }

        public override void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var str = Encoding.UTF8.GetString(e.Body.ToArray());
            var message = JsonSerializer.Deserialize<MessageEvent>(str);

            _logger.LogInformation(
                $"[new message | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] " + message.Message);
        }
    }
}
