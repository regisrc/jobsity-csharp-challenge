using Newtonsoft.Json;

namespace Application.Dto
{
    public class UserCreateDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
