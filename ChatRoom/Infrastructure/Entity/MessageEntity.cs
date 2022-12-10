namespace Infrastructure.Entity
{
    public class MessageEntity : BaseEntity
    {
        public string Message { get; set; }

        public Guid ChatRoomId { get; set; }

        public Guid UserId { get; set; }
    }
}
