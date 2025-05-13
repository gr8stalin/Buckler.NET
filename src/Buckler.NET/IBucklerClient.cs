using Buckler.NET.Models;

namespace Buckler.NET
{
    public interface IBucklerClient
    {
        Task<IEnumerable<PlayerProfile>> GetPlayerAsync(string playerName);

        Task<PlayerProfile?> GetPlayerAsync(long playerUserCode);

        Task<IEnumerable<Replay>> GetAllReplaysForGametypeAsync(long playerUserCode, GameType replayType);

        Task<PlayerGameplayStats> GetPlayerStatsAsync(long playerUserCode);
    }
}
