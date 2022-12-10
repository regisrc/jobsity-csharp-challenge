using Newtonsoft.Json;

namespace Application.Dto
{
    public class MessageDto
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("chatRoomId")]
        public Guid ChatRoomId { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }
}
