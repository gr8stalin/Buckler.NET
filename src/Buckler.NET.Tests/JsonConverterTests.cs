using System.Text.Json;
using Buckler.NET.Models;
using Shouldly;

namespace Buckler.NET.Tests
{
    public class JsonConverterTests
    {
        private string testData;

        [SetUp]
        public void SetUp() 
        { 
            testData = File.ReadAllText(@"TestData\ConverterData\profile.json");
        }

        [Test]
        public void CanConvert() 
        {
            // Arrange
            var parsedData = JsonDocument.Parse(testData).RootElement.GetProperty("pageProps");

            // Act
            var jsonObj = JsonSerializer.Deserialize<FighterBanner>(parsedData);

            // Assert
            jsonObj.ShouldNotBeNull();
            jsonObj.Info.ShouldNotBeNull();
            jsonObj.Stats.ShouldNotBeNull();
        }
    }
}
