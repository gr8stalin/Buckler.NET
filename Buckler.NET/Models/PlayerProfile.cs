using System.Text.Json.Serialization;

namespace Buckler.NET.Models
{
    public class PlayerProfile
    {
        [JsonPropertyName("personal_info")]
        public PersonalInfo PlayerInfo { get; set; }

        [JsonPropertyName("favorite_character_name")]
        public string CurrentActiveCharacter { get; set; }

        [JsonPropertyName("home_name")]
        public string GeographicLocation { get; set; }

        public PlayerProfile() { }
    }

    public class PersonalInfo
    {
        [JsonPropertyName("fighter_id")]
        public string PlayerName { get; set; }

        [JsonPropertyName("short_id")]
        public long PlayerUserCode { get; set; }

        [JsonPropertyName("platform_name")]
        public string Platform { get; set; }

        public PersonalInfo() { }
    }
}
