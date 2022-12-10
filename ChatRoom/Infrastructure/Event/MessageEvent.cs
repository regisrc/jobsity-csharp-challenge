namespace Infrastructure.Event
{
    public class MessageEvent
    {
        public Guid Guid { get; set; }

        public string Message { get; set; }
    }
}
