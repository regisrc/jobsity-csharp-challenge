using Newtonsoft.Json;

namespace Application.Dto
{
    public class LoginResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("token")]
        public Guid? Token { get; set; }
    }
}
