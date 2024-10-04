using Buckler.NET.Models;
using System.Text.Json;

namespace Buckler.NET
{
    public class BucklerClient : IBucklerClient
    {
        private const string ApiUrlPath = "https://www.streetfighter.com/6/buckler/_next/data";

        private readonly string bucklerId;
        private readonly string bucklerRId;
        private readonly string authToken;
        private HttpClient client;

        public BucklerClient(string bucklerId, string bucklerRId, string authToken)
        {
            this.bucklerId = bucklerId;
            this.bucklerRId = bucklerRId;
            this.authToken = authToken;

            client = CreateHttpClient();
        }

        public async Task<PlayerProfile?> GetPlayerAsync(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName)) 
            { 
                throw new ArgumentException("A search term must be provided", nameof(playerName));
            }

            PlayerProfile? playerProfile = null;

            var searchByPlayerNameUrl = $"{ApiUrlPath}/{authToken}/en/fighterslist/search/result.json?fighter_id={playerName}";

            var request = CreateRequest(searchByPlayerNameUrl);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonDocument.Parse(responseString).RootElement.GetProperty("pageProps");
                var profiles = JsonSerializer.Deserialize<List<PlayerProfile>>(jsonData);

                playerProfile = profiles?.SingleOrDefault();
            }

            return playerProfile;
        }

        public async Task<PlayerProfile?> GetPlayerAsync(long? playerUserCode)
        {
            if (playerUserCode is null)
            {
                throw new ArgumentException("A search term must be provided", nameof(playerUserCode));
            }

            PlayerProfile? playerProfile = null;

            var searchByPlayerUserCodeUrl = $"{ApiUrlPath}/{authToken}/en/fighterslist/search/result.json?short_id={playerUserCode}";

            var request = CreateRequest(searchByPlayerUserCodeUrl);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonDocument.Parse(responseString).RootElement.GetProperty("pageProps");
                var profiles = JsonSerializer.Deserialize<List<PlayerProfile>>(jsonData);

                playerProfile = profiles?.SingleOrDefault();
            }

            return playerProfile;
        }

        public async Task<IEnumerable<Replay>> GetReplaysAsync(long? playerUserCode, ReplayType replayType)
        {
            if (playerUserCode is null)
            {
                throw new ArgumentException("Player identification must be provided", nameof(playerUserCode));
            }

            List<Replay> replays = new();

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

            var request = CreateRequest(replayListUrl);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonDocument.Parse(responseString).RootElement.GetProperty("pageProps");
                /*
                 * The replay list may be empty due to maintenance (i.e., after game updates that break
                 * replay compatibility) but because the underlying JSON will always return a player's
                 * profile data as part of the replay fetching process, the ReplayContainer object will
                 * never be null
                 */ 
                var replayData = JsonSerializer.Deserialize<ReplayContainer>(jsonData)!;

                if (replayData.ReplayList.Count == 0)
                {
                    return replays;
                }

                replays.AddRange(replayData.ReplayList);

                if (replayData.TotalPages > 1)
                {
                    for (var i = 2; i <= replayData.TotalPages; i++)
                    {
                        var nextPageRequest = CreateRequest(replayListUrl + $"?page={i}");
                        var nextPageResponse = await client.SendAsync(nextPageRequest);

                        if (nextPageResponse.IsSuccessStatusCode)
                        {
                            var nextPageAsString = await response.Content.ReadAsStringAsync();
                            var nextPageJson = JsonDocument.Parse(nextPageAsString).RootElement.GetProperty("pageProps");
                            var nextPage = JsonSerializer.Deserialize<ReplayContainer>(nextPageJson)!;

                            if (nextPage.ReplayList.Count > 0)
                            {
                                replays.AddRange(nextPage.ReplayList);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return replays;
        }

        private HttpRequestMessage CreateRequest(string searchUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, searchUrl);
            request.Headers.Add("Cookie", $"buckler_r_id={bucklerRId}; buckler_id={bucklerId}");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9,ja;q=0.8");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:120.0) Gecko/20100101 Firefox/120.0");

            return request;
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };

            return new HttpClient(handler);
        }
    }
}
