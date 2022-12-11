using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.MessageBroker
{
    public class Publisher
    {
        public void Publish(object message, string queue)
        {
            var factory = new ConnectionFactory() { HostName = Environment.GetEnvironmentVariable("message_broker_url") };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: queue,
                                 basicProperties: null,
                                 body: body);
        }
    }
}