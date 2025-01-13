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


    /// <summary>
    /// All statistics concerning combat (i.e. Drive Impact usage,
    /// Super Art usage, Perfect Parry usage, etc.) are reported as
    /// an average over the past 100 games regardless of game mode.
    /// </summary>
    public class Battle
    {
        [JsonPropertyName("battle_hub_match_play_count")]
        public int TotalMatchesInBattleHub { get; set; }

        [JsonPropertyName("casual_match_play_count")]
        public int TotalMatchesInCasualMatchmaking { get; set; }

        [JsonPropertyName("rank_match_play_count")]
        public int TotalMatchesInRankedMatchmaking { get; set; }

        [JsonPropertyName("custom_room_match_play_count")]
        public int TotalMatchesInCustomRoom { get; set; }

        public DriveGaugeUsage DriveGaugeUsage { get; set; }

        public SuperGaugeUsage SuperGaugeUsage { get; set; }

        /// <summary>
        /// Average time spent keeping the opponent in the corner
        /// </summary>
        [JsonPropertyName("corner_time")]
        public double TimeSpentCorneringOpponent { get; set; }

        /// <summary>
        /// Average time the player spent defending themselves
        /// in the corner
        /// </summary>
        [JsonPropertyName("cornered_time")]
        public double TimeSpentCornered { get; set; }

        /// <summary>
        /// Average number of times the player used drive impact
        /// </summary>
        [JsonPropertyName("drive_impact")]
        public double DriveImpactUsage { get; set; }

        /// <summary>
        /// Average number of times the player used drive
        /// impact to counter their opponent's drive impact
        /// </summary>
        [JsonPropertyName("drive_impact_to_drive_impact")]
        public double DriveImpactCounterUsage { get; set; }

        /// <summary>
        /// Average number of times the player used drive
        /// parry to defend themselves
        /// </summary>
        [JsonPropertyName("drive_parry")]
        public double DriveParryUsage { get; set; }

        /// <summary>
        /// Average number of times the player used drive
        /// reversal to defend themselves
        /// </summary>
        [JsonPropertyName("drive_reversal")]
        public double DriveReversalUsage { get; set; }

        /// <summary>
        /// Average number of times the player used perfect
        /// parry to defend themselves
        /// </summary>
        [JsonPropertyName("just_parry")]
        public double PerfectParryUsage { get; set; }

        public double SuccessfulThrows { get; set; }

        public double SuccessfulThrowTechs { get; set; }
        public double TimesThrown { get; set; }
        public double TimesThrownWhileInDriveParryState { get; set; }
        public double SuccessfulThrowsWhileOpponentInDriveParryState { get; set; }
        public double PercentageOfDamageFromNormals { get; set; }
    }

    public class CharacterLeagueInfo
    {

    }

    public class CharacterPlayPointInfo
    {

    }

    public class CharacterWinRate
    {

    }

    public class CharacterWinRateByMatchup
    {

    }

    public class DriveGaugeUsage
    {
        [JsonPropertyName("gauge_rate_drive_impact")]
        public double DriveImpact { get; set; }
        [JsonPropertyName("gauge_rate_drive_reversal")]
        public double DriveReversal { get; set; }
        [JsonPropertyName("gauge_rate_drive_rush_from_parry")]
        public double DriveRushFromParry { get; set; }
        [JsonPropertyName("gauge_rate_drive_rush_from_cancel")]
        public double DriveRushFromCancel { get; set; }
        [JsonPropertyName("gauge_rate_drive_arts")]
        public double OverdriveArts { get; set; }
    }

    public class SuperGaugeUsage
    {
        [JsonPropertyName("gauge_rate_sa_lv1")]
        public double Level1 { get; set; }
        [JsonPropertyName("gauge_rate_sa_lv2")]
        public double Level2 { get; set; }
        [JsonPropertyName("gauge_rate_sa_lv3")]
        public double Level3 { get; set; }
        [JsonPropertyName("gauge_rate_ca")]
        public double CriticalArt { get; set; }
    }
}
#pragma warning restore CS8618
