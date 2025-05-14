using System.Text.Json;
using Buckler.NET.Models;
using Shouldly;

namespace Buckler.NET.Tests
{
    public class JsonConverterTests
    {
        private string masterProfile;

        [SetUp]
        public void SetUp() 
        { 
            masterProfile = File.ReadAllText(@"TestData\ConverterData\old-self-profile.json");
        }

        [Test]
        public void CanConvert() 
        {
            // Arrange
            var parsedData = JsonDocument.Parse(masterProfile).RootElement.GetProperty("pageProps");

            // Act
            var jsonObj = JsonSerializer.Deserialize<PlayerGameplayStats>(parsedData);

            // Assert
            jsonObj.ShouldNotBeNull();
            jsonObj.Info.ShouldNotBeNull();
            jsonObj.Stats.ShouldNotBeNull();
            jsonObj.Stats.Battle.ShouldNotBeNull();
            jsonObj.Stats.CharacterPlacements.ShouldNotBeNull();
        }
    }
}
