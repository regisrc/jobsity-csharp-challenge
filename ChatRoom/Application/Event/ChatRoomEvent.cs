namespace Application.Event
{
    public class ChatRoomEvent
    {
        public Guid Guid { get; set; }

        public Guid CreatorId { get; set; }

        public string Name { get; set; }
    }
}
