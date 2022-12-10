using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventManager.EventConsumer
{
    public abstract class Consumer : BackgroundService
    {
        protected readonly ILogger<Consumer> _logger;
        private readonly string _url = "localhost";
        private readonly string _queue = "chat_message";

        protected Consumer(ILogger<Consumer> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = _url };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ConsumerReceived;
            channel.BasicConsume(queue: _queue,
                autoAck: true,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                await Task.Delay(5000, stoppingToken);
            }
        }

        public abstract void ConsumerReceived(object sender, BasicDeliverEventArgs e);
    }
}
