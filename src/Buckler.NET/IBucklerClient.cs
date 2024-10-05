using Buckler.NET.Models;

namespace Buckler.NET
{
    public interface IBucklerClient
    {
        Task<IEnumerable<PlayerProfile>> GetPlayerAsync(string playerName);

        Task<PlayerProfile?> GetPlayerAsync(long? playerUserCode);

        Task<IEnumerable<Replay>> GetReplaysAsync(long? playerUserCode, ReplayType replayType);
    }
}
