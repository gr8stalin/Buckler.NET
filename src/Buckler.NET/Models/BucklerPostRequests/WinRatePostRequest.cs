namespace Buckler.NET.Models.BucklerPostRequests
{
    /// <summary>
    /// Represents a POST request to fetch win rate data for a player.
    /// This covers character win rate and matchup win rate.
    /// </summary>
    public class WinRatePostRequest : PostModelBase
    {
        /// <summary>
        /// The language to fetch the data in. This is a two-letter language code.
        /// </summary>
        public required string Lang { get; set; }

        /// <summary>
        /// TODO: Figure out what this property is used for
        /// </summary>
        public int TargetModeID { get; set; }
    }
}
