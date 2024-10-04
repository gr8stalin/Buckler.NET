using Shouldly;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Buckler.NET.Models;

namespace Buckler.NET.Tests
{
    public class ClientTests
    {
        private IEnumerable<Replay> mockReplayList;
        private int yesterdayAsUnixTimestamp;

        [SetUp]
        public void Setup()
        {
            yesterdayAsUnixTimestamp = (int)DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)).Subtract(DateTime.UnixEpoch).TotalSeconds;

            mockReplayList = new List<Replay>()
            {
                CreateMockReplay()
            };
        }

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
        public async Task GetPlayer_Returns_Profile_On_Success()
        {
            // Arrange
            var subject = Substitute.For<IBucklerClient>();
            subject.GetPlayerAsync("Player").Returns(Task.FromResult(new PlayerProfile() { PlayerInfo = new PersonalInfo() { PlayerName = "Player" } } ?? null));

            // Act
            var result = await subject.GetPlayerAsync("Player");

            // Assert
            result.ShouldNotBeNull();
            result.PlayerInfo.PlayerName.ShouldBe("Player");
        }

        [Test]
        public async Task GetReplays_Returns_Replay_Data_On_Success()
        {
            // Arrange
            var subject = Substitute.For<IBucklerClient>();
            subject.GetReplaysAsync(44445555, ReplayType.CustomRoom).Returns(Task.FromResult(mockReplayList));

            // Act
            var result = await subject.GetReplaysAsync(44445555, ReplayType.CustomRoom);

            // Assert
            result.ShouldNotBeEmpty();
            var resultReplay = result.First();
            resultReplay.ReplayId.ShouldNotBeNullOrEmpty();
            resultReplay.ReplayBattleTypeName.ShouldNotBeNullOrEmpty();
            resultReplay.ReplayBattleTypeName.ShouldBe("Custom Room");
        }

        private Replay CreateMockReplay(string? replayId = null, int? uploadedAtTime = null, string? replayType = null)
        {
            return new Replay()
            {
                ReplayId = replayId ?? "ABC123Z4",
                UploadedAt = uploadedAtTime ?? yesterdayAsUnixTimestamp,
                Player1 = CreateMockPlayerInfo(playerName: "Player 1"),
                Player2 = CreateMockPlayerInfo(playerName: "Player 2"),
                ReplayBattleTypeName = replayType ?? "Custom Room"
            };
        }

        private PlayerInfo CreateMockPlayerInfo(string? playerName = null, long? userCode = null, string? platform = null, string? characterName = null)
        {
            return new PlayerInfo()
            {
                CFN = CreateMockCFNData(playerName, userCode, platform),
                Character = characterName ?? "Test Character 1"
            };
        }

        private CFN CreateMockCFNData(string? playerName = null, long? userCode = null, string? platform = null)
        {
            return new CFN()
            {
                Name = playerName ?? "Test Player",
                Id = userCode ?? 44445555,
                Platform = platform ?? "Test Platform"
            };
        }
    }
}