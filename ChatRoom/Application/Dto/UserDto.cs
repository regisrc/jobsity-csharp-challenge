using Newtonsoft.Json;

namespace Application.Dto
{
    public class UserDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
