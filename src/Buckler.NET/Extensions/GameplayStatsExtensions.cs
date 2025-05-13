using Buckler.NET.Models;

namespace Buckler.NET.Extensions
{
    public static class GameplayStatsExtensions
    {
        public static bool HasNoPlaytime(this PlayerGameplayStats playerProfile) => !playerProfile.Stats.PlaytimeAndLikes.ContentPlayTimeList.Any();
    }
}
