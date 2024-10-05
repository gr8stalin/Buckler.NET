using Shouldly;

namespace Buckler.NET.Tests
{
    public class ClientTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Constructor_Creates_Client()
        {
            // Arrange
            var testId = "test id";
            var testRId = "test r_id";
            var authUrl = "test url";

            // Act
            var subject = new BucklerClient(testId, testRId, authUrl);

            // Assert
            subject.ShouldNotBeNull();
        }
    }
}