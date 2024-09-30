using Shouldly;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Buckler.NET.Models;

namespace Buckler.NET.Tests
{
    public class ClientTests
    {
        [SetUp]
        public void Setup()
        {
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
    }
}