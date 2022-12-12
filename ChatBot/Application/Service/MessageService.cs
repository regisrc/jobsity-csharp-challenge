using Application.Dto;
using Application.Event;
using Application.Interface;
using Application.Interfaces;
using Application.Map;
using CsvHelper;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace Application.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger _logger;
        private readonly IMessageEventPublisher _publisher;
        private readonly IStockApiService _stockApiService;

        public MessageService(
            ILogger<MessageService> logger,
            IMessageEventPublisher publisher,
            IStockApiService stockApiService)
        {
            _logger = logger;
            _publisher = publisher;
            _stockApiService = stockApiService;
        }

        public async Task ConsumeMessage(MessageEvent messageEvent)
        {
            var stockQueryParam = "/stock=";

            if (messageEvent.Message.Contains(stockQueryParam))
            {
                var result = await _stockApiService.Request(messageEvent.Message.Replace(stockQueryParam, string.Empty));
                var stringResult = Encoding.Default.GetString(result);

                using var stringReader = new StringReader(stringResult);
                using var csvReader = new CsvReader(stringReader, CultureInfo.InvariantCulture);
                csvReader.Context.RegisterClassMap<StockMap>();
                var record = csvReader.GetRecords<StockDto>().FirstOrDefault();

                var botMessageEvent = new MessageEvent
                {
                    ChatRoomId = messageEvent.ChatRoomId,
                    Guid = Guid.NewGuid(),
                    Message = $"{record.Symbol} quote is ${record.Open} per share",
                    UserId = messageEvent.UserId
                };

                _publisher.Publish(botMessageEvent);
            }
        }
    }
}
