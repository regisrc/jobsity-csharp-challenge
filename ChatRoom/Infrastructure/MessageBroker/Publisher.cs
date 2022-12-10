using Infrastructure.Interface;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.MessageBroker
{
    public class Publisher : IPublisher
    {
        private readonly string _url = "localhost";
        private readonly string _queue = "chat_message";

        public void Publish(object message)
        {
            var factory = new ConnectionFactory() { HostName = _url };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _queue,
                                 basicProperties: null,
                                 body: body);
        }
    }
}