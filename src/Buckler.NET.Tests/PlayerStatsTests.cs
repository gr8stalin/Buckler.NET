using Buckler.NET.Models;
using NSubstitute;
using Shouldly;
using System.Text.Json;

namespace Buckler.NET.Tests
{
    public class PlayerStatsTests
    {
        private string masterProfile;
        private string highMasterProfile;

        [SetUp]
        public void SetUp()
        {
            masterProfile = File.ReadAllText(@"TestData\PlayerStatsData\self-profile-5-12-2025.json");
            highMasterProfile = File.ReadAllText(@"TestData\PlayerStatsData\high-master-player-profile-5-12-2025.json");
        }

        [Test]
        public void Master_Rank_Subdivision_Should_Be_Separate_From_Rank()
        {
            // Arrange
            var parsedData = JsonDocument.Parse(highMasterProfile).RootElement.GetProperty("pageProps");

            // Act
            var jsonObj = JsonSerializer.Deserialize<PlayerGameplayStats>(parsedData)!;
            var highMasterCharacter = jsonObj.Stats.CharacterPlacements.Where(c => c.CharacterId == 26).Single();

            // Assert
            highMasterCharacter.RankStats.Rank.ShouldBe(Rank.Master);
            highMasterCharacter.RankStats.RankWithinMaster.ShouldBe(MasterRank.HighMaster);
        }

        [Test]
        public async Task Get_Player_Stats_Should_Return_All_Stats_For_Player()
        {
            // Arrange
            var parsedData = JsonDocument.Parse(masterProfile).RootElement.GetProperty("pageProps");
            var profileJson = JsonSerializer.Deserialize<PlayerGameplayStats>(parsedData);

            var subject = Substitute.For<IBucklerClient>();
            subject.GetPlayerStatsAsync(3084788835).Returns(Task.FromResult(profileJson));

            var expectedCharacterName = "Chun-Li";
            var expectedCharacterRank = Rank.Master;
            var expectedCharacterMasterRating = 1473;

            // Act
            var result = await subject.GetPlayerStatsAsync(3084788835);

            // Assert
            result.ShouldNotBeNull();
            result.Info.ShouldNotBeNull();
            result.Stats.ShouldNotBeNull();

            result.Info.PlayerInfo.PlayerName.ShouldBe("gr8stalin");
            result.Info.PlayerInfo.UserCode.ShouldBe(3084788835);
            result.Info.PlayerInfo.Platform.ShouldBe("Steam");

            var actualCharacter = result.Stats.CharacterPlacements.Where(c => c.CharacterId == 4).Single();
            actualCharacter.FullName.ShouldBe(expectedCharacterName);
            actualCharacter.RankStats.Rank.ShouldBe(expectedCharacterRank);
            actualCharacter.RankStats.MasterRating.ShouldBe(expectedCharacterMasterRating);
        }
    }
}
