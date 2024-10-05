using Shouldly;
using NSubstitute;
using Buckler.NET.Models;
using NSubstitute.ExceptionExtensions;

namespace Buckler.NET.Tests
{
    public class PlayerProfileTests
    {
        [SetUp]
        public void SetUp() { }

        [Test]
        public void GetPlayer_Throws_If_No_Player_Name_Provided()
        {
            // Arrange
            var subject = Substitute.For<IBucklerClient>();
            subject.GetPlayerAsync("").ThrowsAsync(new ArgumentException());

            // Act
            // Assert
            subject.GetPlayerAsync("").ShouldThrowAsync<ArgumentException>();
        }

        [Test]
        public void GetPlayer_Throws_If_No_User_Code_Provided()
        {
            // Arrange
            var subject = Substitute.For<IBucklerClient>();
            long? nullUserCode = null;
            subject.GetPlayerAsync(nullUserCode).ThrowsAsync(new ArgumentException());

            // Act
            // Assert
            subject.GetPlayerAsync(nullUserCode).ThrowsAsync(new ArgumentException());
        }

        [Test]
        public async Task GetPlayer_Returns_Profiles_On_Success()
        {
            // Arrange
            IEnumerable<PlayerProfile> playerList =
            [
                CreateMockProfile(CreateMockPersonalInfo(playerName: "Harmful")),
                CreateMockProfile(CreateMockPersonalInfo(playerName: "Harmless"))
            ];

            var subject = Substitute.For<IBucklerClient>();
            subject.GetPlayerAsync("Harm").Returns(Task.FromResult(playerList));

            // Act
            var result = await subject.GetPlayerAsync("Harm");

            // Assert
            result.ShouldNotBeEmpty();
            result.Count().ShouldBe(2);
        }

        [Test]
        public async Task GetPlayer_Returns_Empty_Collection_When_No_Results()
        {
            // Arrange
            IEnumerable<PlayerProfile> emptyList = [];
            var subject = Substitute.For<IBucklerClient>();
            subject.GetPlayerAsync("").Returns(Task.FromResult(emptyList));

            // Act
            var result = await subject.GetPlayerAsync("");

            // Assert
            result.ShouldBeEmpty();
        }

        private PlayerProfile CreateMockProfile(PersonalInfo? info = null, string? activeCharacter = null, string? geographicLocation = null)
        {
            return new PlayerProfile()
            {
                PlayerInfo = info ?? CreateMockPersonalInfo(),
                CurrentActiveCharacter = activeCharacter ?? "Test Character",
                GeographicLocation = geographicLocation ?? "Test Location"
            };
        }

        private PersonalInfo CreateMockPersonalInfo(string? playerName = null, long? userCode = null, string? platform = null)
        {
            return new PersonalInfo()
            {
                PlayerName = playerName ?? "Test Player",
                PlayerUserCode = userCode ?? 44445555,
                Platform = platform ?? "Test Platform"
            };
        }
    }
}
