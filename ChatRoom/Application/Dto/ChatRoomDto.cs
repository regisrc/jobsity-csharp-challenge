using Newtonsoft.Json;

namespace Application.Dto
{
    public class ChatRoomDto
    {
        [JsonProperty("creatorId")]
        public Guid CreatorId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
