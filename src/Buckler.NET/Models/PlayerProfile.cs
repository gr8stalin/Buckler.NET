using Buckler.NET.JsonConverters;
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

    public class FighterBanner
    {
        [JsonPropertyName("fighter_banner_info")]
        public PlayerProfile Info { get; set; }

        [JsonPropertyName("play")]
        public PlayerStats Stats { get; set; }
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

        [JsonPropertyName("battle_stats")]
        [JsonConverter(typeof(BattleStatsConverter))]
        public Battle Battle { get; set; }

        [JsonPropertyName("character_league_infos")]
        public IEnumerable<CharacterRankInfo> CharacterPlacements { get; set; }

        [JsonPropertyName("character_play_point_infos")]
        public IEnumerable<CharacterKudos> CharacterKudos { get; set; }

        [JsonPropertyName("character_win_rates")]
        public IEnumerable<CharacterWinRate> CharacterWinRates { get; set; }

        [JsonPropertyName("character_win_rate_by_rival_character")]
        public IEnumerable<CharacterWinRateByMatchup> CharacterWinRateForMatchup { get; set; }

        [JsonPropertyName("current_season_id")]
        public int CurrentPhase { get; set; }

        [JsonPropertyName("season_ids")]
        public int[] PhaseIds { get; set; }
    }

    /// <summary>
    /// The player's playtime by gamemode and their likes from other
    /// players
    /// </summary>
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

    /// <summary>
    /// The gameplay modes available in Street Fighter 6
    /// </summary>
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

        /// <summary>
        /// The player's usage of drive meter divided between
        /// the game mechanics it powers (i.e., drive impact).
        /// </summary>
        public DriveGaugeUsage DriveGaugeUsage { get; set; }

        /// <summary>
        /// The player's usage of super meter divided between
        /// the three supers every character has plus a separate
        /// value for Critical Art usage.
        /// </summary>
        public SuperGaugeUsage SuperGaugeUsage { get; set; }

        /// <summary>
        /// Average time spent in seconds keeping the opponent
        /// in the corner.
        /// </summary>
        [JsonPropertyName("corner_time")]
        public double TimeSpentCorneringOpponent { get; set; }

        /// <summary>
        /// Average time in seconds the player spent defending 
        /// themselves in the corner.
        /// </summary>
        [JsonPropertyName("cornered_time")]
        public double TimeSpentCornered { get; set; }

        /// <summary>
        /// Average number of times the player used drive impact.
        /// </summary>
        [JsonPropertyName("drive_impact")]
        public double DriveImpactUsage { get; set; }

        /// <summary>
        /// Average number of times the player used drive
        /// impact to counter their opponent's drive impact.
        /// </summary>
        [JsonPropertyName("drive_impact_to_drive_impact")]
        public double DriveImpactCounterUsage { get; set; }

        /// <summary>
        /// Average number of times the player used drive
        /// parry to defend themselves.
        /// </summary>
        [JsonPropertyName("drive_parry")]
        public double DriveParryUsage { get; set; }

        /// <summary>
        /// Average number of times the player used drive
        /// reversal to defend themselves.
        /// </summary>
        [JsonPropertyName("drive_reversal")]
        public double DriveReversalUsage { get; set; }

        /// <summary>
        /// Average number of times the player used perfect
        /// parry to defend themselves.
        /// </summary>
        [JsonPropertyName("just_parry")]
        public double PerfectParryUsage { get; set; }

        /// <summary>
        /// Average number of successful throws made
        /// by the player.
        /// </summary>
        [JsonPropertyName("throw_count")]
        public double SuccessfulThrows { get; set; }

        /// <summary>
        /// Average number of successful defensive throw techs
        /// made by the player to defend themselves.
        /// </summary>

        [JsonPropertyName("throw_tech")]
        public double SuccessfulThrowTechs { get; set; }

        /// <summary>
        /// Average number of successful throws on opponents
        /// tapping or holding drive parry.
        /// </summary>
        [JsonPropertyName("throw_drive_parry")]
        public double SuccessfulThrowsWhileOpponentInDriveParryState { get; set; }

        /// <summary>
        /// Average number of times the player was thrown
        /// by their opponent.
        /// </summary>
        [JsonPropertyName("received_throw_count")]
        public double TimesThrown { get; set; }

        /// <summary>
        /// Average number of times the player was thrown
        /// while holding or tapping drive parry by their
        /// opponent.
        /// </summary>
        [JsonPropertyName("received_throw_drive_parry")]
        public double TimesThrownWhileInDriveParryState { get; set; }

        /// <summary>
        /// Capcom considers normal hits part of their drive
        /// meter usage calculations, this value will be missing
        /// from the <see cref="DriveGaugeUsage"/> child data
        /// transfer object.
        /// </summary>
        [JsonPropertyName("gauge_rate_drive_other")]
        public double PercentageOfDamageFromNormals { get; set; }

        [JsonPropertyName("received_stun")]
        public double SuccessfulStuns { get; set; }

        [JsonPropertyName("stun")]
        public double TimesStunned { get; set; }
    }

    /// <summary>
    /// This class acts as a nametag for each character in
    /// the game. Capcom reuses this structure across several
    /// other data points.
    /// </summary>
    public class CharacterInfo
    {
        /// <summary>
        /// The character's numeric ID
        /// </summary>
        /// <remarks>
        /// This id may represent the order in which the given character
        /// was implemented in the game
        /// </remarks>
        [JsonPropertyName("character_id")]
        public int CharacterId { get; set; }

        /// <summary>
        /// The character's full name
        /// </summary>
        /// <remarks>
        /// The only major difference between this field and 
        /// <see cref="TraditionalName"/> is that they spell out 
        /// E. Honda's name (Edmond) in full in this field. Even
        /// M. Bison remains unaltered outside of capitalization.
        /// </remarks>
        [JsonPropertyName("character_name")]
        public string FullName { get; set; }

        /// <summary>
        /// The character's name (again)
        /// </summary>
        /// <remarks>
        /// This field capitalizes the character's <see cref="FullName"/>
        /// and in the case of Honda, changes his name to his "E. Honda"
        /// abbreviation.
        /// </remarks>
        [JsonPropertyName("character_alpha")]
        public string TraditionalName { get; set; }


        /// <summary>
        /// A truncated version of the character's name removing any
        /// symbols or spaces.
        /// </summary>
        /// <remarks>
        /// Likely used with internal tooling at Capcom as
        /// Akuma's name is displayed in this field as "gouki",
        /// M. Bison's name is displayed as "vega", etc.
        /// </remarks>
        [JsonPropertyName("character_tool_name")]
        public string ToolingName { get; set; }

        /// <summary>
        /// The character's place in the list of characters on
        /// the character select screen.
        /// </summary>
        /// <remarks>
        /// Capcom uses this field as sort order for each tab of the
        /// stats page on Buckler CFN. I think they use <see cref="CharacterId"/>
        /// in conjunction with this field on the Buckler CFN frontend
        /// to put the player's current character at the top of the list
        /// and then sort the rest of the characters by this field.
        /// </remarks>
        [JsonPropertyName("character_sort")]
        public int SelectScreenPlacement { get; set; }
    }

    /// <summary>
    /// The player's rank with a given character
    /// </summary>
    public class CharacterRankInfo : CharacterInfo
    {
        /// <summary>
        /// If the player has never ever touched the character,
        /// even in training mode, this value will be false.
        /// </summary>
        [JsonPropertyName("is_played")]
        public bool IsPlayed { get; set; }

        /// <summary>
        /// Stats for the given character's league.
        /// See <see cref="RankInfo"/> for more details.
        /// </summary>
        [JsonPropertyName("league_info")]
        public RankInfo RankStats { get; set; }
    }

    /// <summary>
    /// The total Kudos amassed by the player
    /// </summary>
    public class CharacterKudos : CharacterInfo
    {
        [JsonPropertyName("play_point")]
        public Kudos Kudos { get; set; }
    }

    /// <summary>
    /// The player's win rate with each character
    /// </summary>
    public class CharacterWinRate : CharacterInfo
    {
        [JsonPropertyName("battle_count")]
        public int TotalBattles { get; set; }

        [JsonPropertyName("win_count")]
        public int Wins { get; set; }
    }

    /// <summary>
    /// Each character's win rate in all possible matchups
    /// within the game, i.e., the win rate of the player's
    /// Ken vs. Ryu, vs. Chun-Li, and so on.
    /// </summary>
    public class CharacterWinRateByMatchup : CharacterInfo
    {

        [JsonPropertyName("rival_character_win_rates")]
        public IEnumerable<CharacterWinRate> Matchups { get; set; }
    }

    /// <remarks>
    /// The <see cref="DriveGaugeUsage"/> DTO does not contain
    /// the property that tracks damage from normals and regular
    /// (non-meter enhanced) special moves. The values in this DTO
    /// will not sum up to 100% without the property 
    /// <see cref="PercentageOfDamageFromNormals"/> in the parent DTO.
    /// </remarks>
    public class DriveGaugeUsage
    {
        [JsonPropertyName("gauge_rate_drive_guard")]
        public double DriveParry { get; set; }

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

    public class RankInfo
    {
        /// <summary>
        /// The player's total LP with a given character
        /// </summary>
        [JsonPropertyName("league_point")]
        public int Points { get; set; }

        /// <summary>
        /// The player's rank
        /// </summary>
        [JsonPropertyName("league_rank")]
        public Rank Rank { get; set; }

        /// <summary>
        /// The player's place within the recently-introduced
        /// subdivisions of Master rank. See <see cref="MasterRank"/>
        /// </summary>
        [JsonPropertyName("master_league")]
        public MasterRank RankWithinMaster { get; set; }

        /// <summary>
        /// The player's MR.
        /// </summary>
        [JsonPropertyName("master_rating")]
        public int MasterRating { get; set; }

        /// <summary>
        /// If the player is in <see cref="MasterRank.Legend"/>,
        /// this value represents their place within Legend rank.
        /// </summary>
        [JsonPropertyName("master_rating_ranking")]
        public int MasterRatingRanking { get; set; }
    }

    /// <summary>
    /// The player's Kudos in each game mode
    /// </summary>
    public class Kudos
    {
        public int BattleHub { get; set; }
        public int FightingGround { get; set; }
        public int WorldTour { get; set; }
    }

    public enum MasterRank
    {
        NotMaster = 0,
        Master = 36,
        Legend = 37,
        HighMaster = 40,
        GrandMaster = 41,
        UltimateMaster = 42
    }

    public enum Rank
    {
        Rookie1 = 1,
        Rookie2 = 2,
        Rookie3 = 3,
        Rookie4 = 4,
        Rookie5 = 5,
        Iron1 = 6,
        Iron2 = 7,
        Iron3 = 8,
        Iron4 = 9,
        Iron5 = 10,
        Bronze1 = 11,
        Bronze2 = 12,
        Bronze3 = 13,
        Bronze4 = 14,
        Bronze5 = 15,
        Silver1 = 16,
        Silver2 = 17,
        Silver3 = 18,
        Silver4 = 19,
        Silver5 = 20,
        Gold1 = 21,
        Gold2 = 22,
        Gold3 = 23,
        Gold4 = 24,
        Gold5 = 25,
        Platinum1 = 26,
        Platinum2 = 27,
        Platinum3 = 28,
        Platinum4 = 29,
        Platinum5 = 30,
        Diamond1 = 31,
        Diamond2 = 32,
        Diamond3 = 33,
        Diamond4 = 34,
        Diamond5 = 35,
        Master = 36,
        NewChallenger = 39
    }
}
#pragma warning restore CS8618
