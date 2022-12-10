using Application.Event;
using Application.Interface;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventManager.EventConsumer
{
    public class MessageEventConsumer : Consumer
    {
        private readonly IMessageService _messageService;

        public MessageEventConsumer(IMessageService messageService, ILogger<Consumer> logger) : base(logger)
        {
            _messageService = messageService;
        }

        public override void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var str = Encoding.UTF8.GetString(e.Body.ToArray());
            var message = JsonSerializer.Deserialize<MessageEvent>(str);

            _messageService.SaveMessage(message);
        }
    }
}
