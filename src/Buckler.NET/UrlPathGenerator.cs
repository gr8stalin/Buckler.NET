using Buckler.NET.Models;

namespace Buckler.NET
{
    public class UrlPathGenerator
    {
        private const string ApiUrlPath = "https://www.streetfighter.com/6/buckler/_next/data";
        private readonly string authToken;

        public UrlPathGenerator(string authToken) => this.authToken = authToken;

        public string ProfileSearchUrl(string playerName)
        {
            return $"{ApiUrlPath}/{authToken}/en/fighterslist/search/result.json?fighter_id={playerName}";
        }

        public string ProfileSearchUrl(long playerUserCode)
        {
            return $"{ApiUrlPath}/{authToken}/en/fighterslist/search/result.json?short_id={playerUserCode}";
        }

        public string ReplayListUrl(long playerUserCode, GameType replayType)
        {
            var replayListUrl = $"{ApiUrlPath}/{authToken}/en/profile/{playerUserCode}/battlelog";

            switch (replayType)
            {
                case GameType.Ranked:
                    replayListUrl += "/rank.json";
                    break;
                case GameType.Casual:
                    replayListUrl += "/casual.json";
                    break;
                case GameType.CustomRoom:
                    replayListUrl += "/custom.json";
                    break;
                case GameType.BattleHub:
                    replayListUrl += "/hub.json";
                    break;
                default:
                    break;
            }

            return replayListUrl;
        }

        public string PlayerStatsUrl(long playerUserCode)
        {
            return $"{ApiUrlPath}/{authToken}/en/profile/{playerUserCode}/play.json";
        }

        public string PlayerRankInfo() => "https://www.streetfighter.com/6/buckler/api/profile/play/act/leagueinfo";
    }
}
