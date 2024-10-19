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

        public string ProfileSearchUrl(long? playerUserCode)
        {
            return $"{ApiUrlPath}/{authToken}/en/fighterslist/search/result.json?short_id={playerUserCode}";
        }

        public string ReplayListUrl(long? playerUserCode, ReplayType replayType)
        {
            var replayListUrl = $"{ApiUrlPath}/{authToken}/en/profile/{playerUserCode}/battlelog";

            switch (replayType)
            {
                case ReplayType.Ranked:
                    replayListUrl += "/rank.json";
                    break;
                case ReplayType.Casual:
                    replayListUrl += "/casual.json";
                    break;
                case ReplayType.CustomRoom:
                    replayListUrl += "/custom.json";
                    break;
                case ReplayType.BattleHub:
                    replayListUrl += "/hub.json";
                    break;
                default:
                    break;
            }

            return replayListUrl;
        }
    }
}
