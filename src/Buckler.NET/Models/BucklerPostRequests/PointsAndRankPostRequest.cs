namespace Buckler.NET.Models.BucklerPostRequests
{
    public class PointsAndRankPostRequest : PostModelBase
    {
        /// <summary>
        /// The language to fetch the data in. This is a two-letter language code.
        /// </summary>
        /// <remarks>
        /// This signals inconsistency between services for Capcom's websites
        /// </remarks>
        public required string Locale { get; set; }

        /// <summary>
        /// Flag for whether to fetch the user's all-time best data
        /// </summary>
        public required bool Peak { get; set; }
    }
}
