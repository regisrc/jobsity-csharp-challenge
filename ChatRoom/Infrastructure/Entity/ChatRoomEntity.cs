namespace Infrastructure.Entity
{
    public class ChatRoomEntity : BaseEntity
    {
        public string Name { get; set; }

        public Guid CreatorId { get; set; }
    }
}
