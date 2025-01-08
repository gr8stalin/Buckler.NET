using System.Text.Json.Serialization;

namespace Buckler.NET.Models
{
#pragma warning disable CS8618
    public class PlayerListContainer
    {
        [JsonPropertyName("fighter_banner_list")]
        public IEnumerable<PlayerProfile> PlayerList { get; set; }

        public PlayerListContainer() { }
    }

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

    public class PlayerStats
    {
        [JsonPropertyName("base_info")]
        public PlaytimeAndLikes PlaytimeAndLikes { get; set; }
    }

    public class PlaytimeAndLikes
    {
        /// <summary>
        /// Each game mode (battle hub, world tour, etc.) has its
        /// own model in Capcom's schema, but each model is a container
        /// for the game mode name and time in minutes
        /// </summary>
        [JsonPropertyName("content_play_time_list")]
        public IEnumerable<GameMode> ContentPlayTimeList { get; set; }

        /// <summary>
        /// These likes are from the "thumbs up" that appears
        /// at the end of a game or while watching a replay
        /// </summary>
        [JsonPropertyName("enjoy_fight_point")]
        public int LikesViaRoundEndScreenOrReplay { get; set; }

        /// <summary>
        /// These likes are from the "thumbs up" that appears
        /// on a player's in-game CFN profile
        /// </summary>
        [JsonPropertyName("enjoy_user_point")]
        public int LikesViaProfile { get; set; }

        /// <summary>
        /// Capcom tracks the total number of likes as a separate
        /// data point in their schema
        /// </summary>
        [JsonPropertyName("enjoy_total_point")]
        public int TotalLikes { get; set; }
    }

    public class GameMode
    {
        /// <summary>
        /// Capcom's internal numeric identifier for the
        /// current game mode
        /// </summary>
        [JsonPropertyName("content_type")]
        public int ContentType { get; set; }

        /// <summary>
        /// The player's play time in the current game
        /// mode in minutes
        /// </summary>
        [JsonPropertyName("play_time")]
        public int PlayTime { get; set; }

        /// <summary>
        /// The name of the content
        /// </summary>
        [JsonPropertyName("content_type_name")]
        public string ContentName { get; set; }

        public GameMode() { }
    }
}
#pragma warning restore CS8618
