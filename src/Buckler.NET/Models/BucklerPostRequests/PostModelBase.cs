namespace Buckler.NET.Models.BucklerPostRequests
{
    /// <summary>
    /// Base class for Buckler POST requests. All Buckler POST requests share
    /// two common properties. There are two different types of POST requests
    /// that get certain data for prior phases, and they cover four sections.
    /// </summary>
    public class PostModelBase
    {
        /// <summary>
        /// The numeric phase/act/season to fetch. This is 1-indexed, so 1 = Phase 1.
        /// </summary>
        /// remarks>
        /// When fetching "all-time best" data for a player, set this to -1.
        /// </remarks>
        public int TargetSeasonId { get; set; }

        /// <summary>
        /// The user code of the user to fetch phase data for.
        /// </summary>
        public long TargetShortId { get; set; }
    }
}
