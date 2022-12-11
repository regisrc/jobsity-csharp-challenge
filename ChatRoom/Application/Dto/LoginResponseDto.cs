using Newtonsoft.Json;

namespace Application.Dto
{
    public class LoginResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("token")]
        public Guid? Token { get; set; }

        [JsonProperty("userId")]
        public Guid? UserId { get; set; }
    }
}
