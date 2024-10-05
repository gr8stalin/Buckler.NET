using System.Text.Json.Serialization;

namespace Buckler.NET.Models
{
    public class ReplayContainer
    {
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("total_page")]
        public int TotalPages { get; set; }

        [JsonPropertyName("replay_list")]
        public IEnumerable<Replay> ReplayList { get; set; }

        [JsonPropertyName("sid")]
        public long PlayerId { get; set; }

#pragma warning disable CS8618
        public ReplayContainer() { }
#pragma warning restore CS8618
    }

    public class Replay
    {
        [JsonPropertyName("player1_info")]
        public PlayerInfo Player1 { get; set; }

        [JsonPropertyName("player2_info")]
        public PlayerInfo Player2 { get; set; }

        [JsonPropertyName("replay_battle_sub_type")]
        public int ReplayBattleSubType { get; set; }

        [JsonPropertyName("replay_battle_type")]
        public int ReplayBattleType { get; set; }

        [JsonPropertyName("replay_id")]
        public string ReplayId { get; set; }

        [JsonPropertyName("uploaded_at")]
        public int UploadedAt { get; set; }

        [JsonPropertyName("replay_battle_type_name")]
        public string ReplayBattleTypeName { get; set; }

        [JsonPropertyName("replay_battle_sub_type_name")]
        public string ReplayBattleSubTypeName { get; set; }

#pragma warning disable CS8618
        public Replay() { }
#pragma warning restore CS8618
    }

    public class PlayerInfo
    {
        [JsonPropertyName("player")]
        public CFN CFN { get; set; }

        [JsonPropertyName("character_name")]
        public string Character { get; set; }

        [JsonPropertyName("league_rank")]
        public int LeagueRank { get; set; }

        [JsonPropertyName("master_league")]
        public int MasterLeague { get; set; }

        [JsonPropertyName("master_rating")]
        public int MasterRating { get; set; }

        [JsonPropertyName("master_rating_ranking")]
        public int MasterRatingRanking { get; set; }

        [JsonPropertyName("round_results")]
        public int[] RoundResults { get; set; }

#pragma warning disable CS8618
        public PlayerInfo() { }
#pragma warning restore CS8618
    }

    public class CFN
    {
        [JsonPropertyName("fighter_id")]
        public string Name { get; set; }

        [JsonPropertyName("short_id")]
        public long Id { get; set; }

        [JsonPropertyName("platform_name")]
        public string Platform { get; set; }

#pragma warning disable CS8618
        public CFN() { }
#pragma warning restore CS8618
    }
}
