using Newtonsoft.Json;

namespace Application.Dto
{
    public class MessageDto
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
