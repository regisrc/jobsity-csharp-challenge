using Application.Event;
using Application.Interfaces;
using Application.Service;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using System.Text;

namespace Test.Application.Service
{
    public class MessageServiceTest
    {
        private readonly Mock<ILogger<MessageService>> _logger;
        private readonly Mock<IStockApiService> _stockApiService;
        private readonly Mock<IMessageEventPublisher> _messageEventPublisher;
        private readonly MessageService _messageService;

        public MessageServiceTest()
        {
            _logger = new Mock<ILogger<MessageService>>();
            _stockApiService = new Mock<IStockApiService>();
            _messageEventPublisher = new Mock<IMessageEventPublisher>();

            _messageService = new MessageService(_logger.Object, _messageEventPublisher.Object, _stockApiService.Object);
        }

        [Fact]
        public async Task ConsumeMessage_Should_Not_Consume_Message()
        {
            var messageEvent = new MessageEvent
            {
                ChatRoomId = Guid.NewGuid(),
                Guid = Guid.NewGuid(),
                Message = string.Empty,
                UserId = Guid.NewGuid()
            };

            await _messageService.ConsumeMessage(messageEvent);

            _stockApiService.Verify(x => x.Request(It.IsAny<string>(), It.IsAny<RestClient>()), Times.Never);
            _messageEventPublisher.Verify(x => x.Publish(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task ConsumeMessage_Should_Consume_Message()
        {
            var content = "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL.US,2022-12-09,22:00:03,142.34,145.57,140.9,142.16,76097011\r\n";

            var messageEvent = new MessageEvent
            {
                ChatRoomId = Guid.NewGuid(),
                Guid = Guid.NewGuid(),
                Message = "/stock=aapl.us",
                UserId = Guid.NewGuid()
            };

            _stockApiService.Setup(x => x.Request(It.IsAny<string>(), It.IsAny<RestClient>())).ReturnsAsync(Encoding.ASCII.GetBytes(content));

            var botMessageEvent = new MessageEvent
            {
                ChatRoomId = messageEvent.ChatRoomId,
                Guid = Guid.NewGuid(),
                Message = $"AAPL.US quote is $142.34 per share",
                UserId = messageEvent.UserId
            };

            await _messageService.ConsumeMessage(messageEvent);

            _stockApiService.Verify(x => x.Request(It.IsAny<string>(), It.IsAny<RestClient>()), Times.Once);
            _stockApiService.Verify(x => x.Request(messageEvent.Message.Replace("/stock=", string.Empty), It.IsAny<RestClient>()), Times.Once);

            _messageEventPublisher.Verify(x => x.Publish(It.IsAny<object>()), Times.Once);
            _messageEventPublisher.Verify(x => x.Publish(It.Is<MessageEvent>(
                x => x.UserId == botMessageEvent.UserId
                && x.Message == botMessageEvent.Message
                && x.ChatRoomId == botMessageEvent.ChatRoomId)), Times.Once);
        }
    }
}
