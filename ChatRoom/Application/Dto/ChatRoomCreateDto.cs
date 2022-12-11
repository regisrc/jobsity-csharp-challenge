using Newtonsoft.Json;

namespace Application.Dto
{
    public class ChatRoomCreateDto
    {
        [JsonProperty("creatorId")]
        public Guid CreatorId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
