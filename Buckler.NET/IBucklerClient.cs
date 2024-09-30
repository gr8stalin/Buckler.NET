using Buckler.NET.Models;

namespace Buckler.NET
{
    public interface IBucklerClient
    {
        Task<PlayerProfile?> GetPlayerAsync(string playerName);

        Task<PlayerProfile?> GetPlayerAsync(long? playerUserCode);
    }
}
