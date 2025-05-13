using Buckler.NET.Extensions;
using Buckler.NET.Models;
using System.Net;
using System.Text.Json;

namespace Buckler.NET
{
    public class BucklerClient(string bucklerId, string bucklerRId, string authToken) : IBucklerClient
    {
        private readonly string bucklerId = bucklerId;
        private readonly string bucklerRId = bucklerRId;
        private HttpClient client = CreateHttpClient();
        private UrlPathGenerator urlPathGenerator = new(authToken);

        /// <summary>
        /// Given a player's name, returns a collection of profiles that contain that name.
        /// </summary>
        /// <param name="playerName">Must be greater than 4 characters as per Buckler CFN requirements</param>
        /// <returns>
        /// A collection of profiles that contain the name. If no players match the search term,
        /// the collection will be empty.
        /// </returns>
        /// /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<PlayerProfile>> GetPlayerAsync(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName)) 
            { 
                throw new ArgumentException("A search term must be provided", nameof(playerName));
            }

            if (playerName.Length < 4)
            {
                throw new ArgumentException("The search term must be greater than or equal to 4 characters in length", nameof(playerName));
            }

            var playerSearchUrl = urlPathGenerator.ProfileSearchUrl(playerName);
            var playerSearchResponse = await GetBucklerDataAsync(playerSearchUrl);

            /*
             * The profile list may be empty (i.e., the user doesn't exist), but there's still
             * a parent JSON object wrapping it which contains metadata such as the search terms
             * that were used. This deserialization will always result in an object and will not
             * return null.
             */ 
            var container = JsonSerializer.Deserialize<PlayerListContainer>(playerSearchResponse)!;

            if (container.PlayerList.Any())
            {
                return container.PlayerList.ToList();
            }

            return [];
        }

        /// <summary>
        /// Given a player's user code, returns their CFN profile.
        /// </summary>
        /// <param name="playerUserCode">
        /// This must be the exact user code as this search endpoint does not perform approximate searches
        /// </param>
        /// <returns>A <see cref="PlayerProfile"/> object representing their relevant CFN information.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PlayerProfile?> GetPlayerAsync(long playerUserCode)
        {
            var playerUserCodeSearchUrl = urlPathGenerator.ProfileSearchUrl(playerUserCode);
            var playerSearchResponse = await GetBucklerDataAsync(playerUserCodeSearchUrl);

            /*
             * The profile list may be empty (i.e., the user doesn't exist), but there's still
             * a parent JSON object wrapping it which contains metadata such as the search terms
             * that were used. This deserialization will always result in an object and will not
             * return null.
             */
            var container = JsonSerializer.Deserialize<PlayerListContainer>(playerSearchResponse)!;
            
            return container.PlayerList.SingleOrDefault();
        }

        /// <summary>
        /// Given a player's user code, retrieves their gameplay statistics and other
        /// metadata like win rates, character usage, etc.
        /// </summary>
        /// <param name="playerUserCode">This must be the player's exact user code</param>
        /// <returns>A <see cref="PlayerGameplayStats"/> object representing the stats
        /// from their CFN profile.</returns>
        public async Task<PlayerGameplayStats?> GetPlayerStatsAsync(long playerUserCode)
        {
            var playerUserCodeStatsUrl = urlPathGenerator.PlayerStatsUrl(playerUserCode);
            var playerStatsResponse = await GetBucklerDataAsync(playerUserCodeStatsUrl);

            var playerStats = JsonSerializer.Deserialize<PlayerGameplayStats>(playerStatsResponse)!;

            if (playerStats.HasNoPlaytime())
            {
                return null;
            }

            return playerStats;
        }

        /// <summary>
        /// Retrieves the most recent replay from the specified user's CFN
        /// </summary>
        /// <param name="playerUserCode"></param>
        /// <param name="gameType"></param>
        /// <returns>The most recent replay as a <see cref="Replay"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Replay?> GetMostRecentReplayAsync(long playerUserCode, GameType gameType)
        {
            var replayListUrl = urlPathGenerator.ReplayListUrl(playerUserCode, gameType);
            var replayJsonData = await GetBucklerDataAsync(replayListUrl);
            var replayData = JsonSerializer.Deserialize<ReplayContainer>(replayJsonData)!;

            if (!replayData.ReplayList.Any())
            {
                return null;
            }

            return replayData.ReplayList.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all of the available replays from the specified user's CFN
        /// </summary>
        /// <param name="playerUserCode"></param>
        /// <param name="gameType"></param>
        /// <returns>A collection of all available replays</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<Replay>> GetAllReplaysForGametypeAsync(long playerUserCode, GameType gameType)
        {
            // Buckler's pagination starts at 1 instead of 0
            int pageOffset = 2;
            bool replaysPresent = true;
            List<Replay> replays = [];

            var replayListUrl = urlPathGenerator.ReplayListUrl(playerUserCode, gameType);
            var replayResponse = await GetBucklerDataAsync(replayListUrl);

           /*
            * The replay list may be empty due to maintenance (i.e., after game updates that break
            * replay compatibility) but because the underlying JSON will always return a player's
            * profile data as part of the replay fetching process, the ReplayContainer object will
            * never be null
            */ 
            var replayData = JsonSerializer.Deserialize<ReplayContainer>(replayResponse)!;

            if (!replayData.ReplayList.Any())
            {
                return replays;
            }

            int totalPages = replayData.TotalPages;
            replays.AddRange(replayData.ReplayList);

            if (totalPages > 1)
            {
                while (totalPages >= pageOffset && replaysPresent)
                {
                    var nextResponse = await GetBucklerDataAsync(replayListUrl + $"?page={pageOffset}");
                    var nextPage = JsonSerializer.Deserialize<ReplayContainer>(nextResponse)!;
                    
                    if (nextPage.ReplayList.Any())
                    {
                        replays.AddRange(nextPage.ReplayList);
                        pageOffset++;
                    }
                    else
                    {
                        replaysPresent = false;
                    }
                }
            }
  
            return replays;
        }

        /// <summary>
        /// Wrapper for <see cref="HttpClient.SendAsync(HttpRequestMessage)"/> with
        /// some exception handling and Buckler-specific parsing to access the actual
        /// JSON data within a container property
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        /// <exception cref="WebException"></exception>
        private async Task<JsonElement> GetBucklerDataAsync(string requestUrl)
        {
            var request = CreateRequest(requestUrl);
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new WebException("The server returned 400: Bad Request");
                    case HttpStatusCode.Unauthorized:
                        throw new WebException("The server returned 401: Unauthorized");
                    case HttpStatusCode.Forbidden:
                        throw new WebException("The server returned 403: Forbidden");
                    case HttpStatusCode.NotFound:
                        throw new WebException("The server returned 404: Not Found");
                }
            }

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(responseString).RootElement.GetProperty("pageProps");
        }

        /// <summary>
        /// Creates the Buckler-specific <see cref="HttpRequestMessage"/> for
        /// accessing the various Buckler endpoints
        /// </summary>
        /// <param name="searchUrl">The endpoint URL on Buckler</param>
        private HttpRequestMessage CreateRequest(string searchUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, searchUrl);
            request.Headers.Add("Cookie", $"buckler_r_id={bucklerRId}; buckler_id={bucklerId}");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9,ja;q=0.8");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:120.0) Gecko/20100101 Firefox/120.0");

            return request;
        }

        /// <summary>
        /// Builds the initial <see cref="HttpClient"/> for this wrapper class
        /// </summary>
        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            return new HttpClient(handler);
        }
    }
}
