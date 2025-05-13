using System.Text.Json;
using Buckler.NET.Models;
using Shouldly;

namespace Buckler.NET.Tests
{
    public class JsonConverterTests
    {
        private string masterProfile;
        private string highMasterProfile;

        [SetUp]
        public void SetUp() 
        { 
            masterProfile = File.ReadAllText(@"TestData\ConverterData\self-profile-5-12-2025.json");
            highMasterProfile = File.ReadAllText(@"TestData\ConverterData\high-master-player-profile-5-12-2025.json");
        }

        [Test]
        public void CanConvert() 
        {
            // Arrange
            var parsedData = JsonDocument.Parse(masterProfile).RootElement.GetProperty("pageProps");

            // Act
            var jsonObj = JsonSerializer.Deserialize<FighterBanner>(parsedData);

            // Assert
            jsonObj.ShouldNotBeNull();
            jsonObj.Info.ShouldNotBeNull();
            jsonObj.Stats.ShouldNotBeNull();
            jsonObj.Stats.Battle.ShouldNotBeNull();
            jsonObj.Stats.CharacterPlacements.ShouldNotBeNull();
        }

        [Test]
        public void MasterRankSubdivisionShouldBeSeparateFromRank()
        {
            // Arrange
            var parsedData = JsonDocument.Parse(highMasterProfile).RootElement.GetProperty("pageProps");

            // Act
            var jsonObj = JsonSerializer.Deserialize<FighterBanner>(parsedData)!;
            var highMasterCharacter = jsonObj.Stats.CharacterPlacements.Where(c => c.CharacterId == 26).Single();

            // Assert
            highMasterCharacter.RankStats.Rank.ShouldBe(Rank.Master);
            highMasterCharacter.RankStats.RankWithinMaster.ShouldBe(MasterRank.HighMaster);
        }
    }
}
