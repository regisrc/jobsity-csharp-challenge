using Application.Event;
using Application.Interface;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventManager.EventConsumer
{
    public class ChatRoomEventConsumer : Consumer
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatRoomEventConsumer(IChatRoomService chatRoomService, ILogger<Consumer> logger) : base(logger, Environment.GetEnvironmentVariable("message_broker_chatroom_queue") ?? string.Empty)
        {
            _chatRoomService = chatRoomService;
        }

        public override void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var str = Encoding.UTF8.GetString(e.Body.ToArray());
            var message = JsonSerializer.Deserialize<ChatRoomEvent>(str);

            _chatRoomService.SaveChatRoom(message);
        }
    }
}
