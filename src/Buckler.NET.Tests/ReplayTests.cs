using Shouldly;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Buckler.NET.Models;

namespace Buckler.NET.Tests
{
    public class ReplayTests
    {
        private int yesterdayAsUnixTimestamp;
        private IEnumerable<Replay> mockReplayList;

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

        private Replay CreateMockReplay(string? replayId = null, int? uploadedAtTime = null, string? replayType = null, PlayerInfo? player1 = null, PlayerInfo? player2 = null)
        {
            return new Replay()
            {
                ReplayId = replayId ?? "ABC123Z4",
                UploadedAt = uploadedAtTime ?? yesterdayAsUnixTimestamp,
                Player1 = player1 ?? CreateMockPlayerInfo(playerName: "Player 1"),
                Player2 = player2 ?? CreateMockPlayerInfo(playerName: "Player 2"),
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
