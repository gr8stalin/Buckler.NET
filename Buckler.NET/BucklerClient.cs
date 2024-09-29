namespace Buckler.NET
{
    public class BucklerClient
    {
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
