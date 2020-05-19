using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Options;
using Moq;
using Regs.Server.Models;
using Regs.Server.Services;
using Regs.Server.Settings;
using Shouldly;
using Xunit;

namespace Tests.Regs.Server
{
    public class RegsDatabaseManagerTests
    {
        [Theory]
        [AutoMoqData]
        public void ConstructorShouldSetCountBasedOnSettingsNumberOfDatabases([Frozen] IOptionsMonitor<RegsSettings> settingsMonitor,
                                                                              RegsDatabaseManager sut)
        {
            // Arrange

            // Act

            // Assert
            sut.Count.ShouldBe(settingsMonitor.CurrentValue.NumberOfDatabases);
        }

        [Theory]
        [AutoMoqData]
        public void ConstructorShouldCallCurrentValueOnMonitor([Frozen] Mock<IOptionsMonitor<RegsSettings>> settingsMonitorMock,
                                                               RegsDatabaseManager sut)
        {
            // Arrange

            // Act

            // Assert
            settingsMonitorMock.VerifyGet(monitor => monitor.CurrentValue);
        }

        [Theory]
        [AutoMoqData]
        public void GetShouldReturnExpectedGivenValidInsert([Frozen] RegsSettings _,
                                                            [Frozen] IOptionsMonitor<RegsSettings> __,
                                                            string keyValue,
                                                            string entryValue,
                                                            RegsDatabaseManager sut)
        {
            // Arrange
            var key = new RegsKey(keyValue, RegsDataType.String);
            var expected = new RegsStringEntry(entryValue);

            // Act
            var wasInserted = sut.Insert(0, key, expected);
            var actual = sut.Get(0, key);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [AutoMoqData]
        public void GetShouldReturnExpectedGivenValidUpdate([Frozen] RegsSettings _,
                                                            [Frozen] IOptionsMonitor<RegsSettings> __,
                                                            string keyValue,
                                                            string insertValue,
                                                            string entryValue,
                                                            RegsDatabaseManager sut)
        {
            // Arrange
            var key = new RegsKey(keyValue, RegsDataType.String);
            var insert = new RegsStringEntry(insertValue);
            var expected = new RegsStringEntry(entryValue);
            sut.Insert(0, key, insert);

            // Act
            sut.Update(0, key, expected);
            var actual = sut.Get(0, key);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [AutoMoqData]
        public void GetShouldReturnExpectedGivenValidUpsert([Frozen] RegsSettings _,
                                                            [Frozen] IOptionsMonitor<RegsSettings> __,
                                                            string keyValue,
                                                            string entryValue,
                                                            RegsDatabaseManager sut)
        {
            // Arrange
            var key = new RegsKey(keyValue, RegsDataType.String);
            var expected = new RegsStringEntry(entryValue);

            // Act
            sut.Upsert(0, key, expected);
            var actual = sut.Get(0, key);

            // Assert
            actual.ShouldBe(expected);
        }

        
        [Theory]
        [AutoMoqData]
        public void GetShouldReturnNullGivenValidDelete([Frozen] RegsSettings _,
                                                        [Frozen] IOptionsMonitor<RegsSettings> __,
                                                        string keyValue,
                                                        string deleteValue,
                                                        RegsDatabaseManager sut)
        {
            // Arrange
            var key = new RegsKey(keyValue, RegsDataType.String);
            var toBeDeleted = new RegsStringEntry(deleteValue);
            sut.Insert(0, key, toBeDeleted);

            // Act
            sut.Delete(0, key);
            var actual = sut.Get(0, key);

            // Assert
            actual.ShouldBeNull();
        }

        [Theory]
        [AutoMoqData]
        public void InsertShouldReturnTrueGivenValidInsert([Frozen] RegsSettings _,
                                                           [Frozen] IOptionsMonitor<RegsSettings> __,
                                                           string keyValue,
                                                           string entryValue,
                                                           RegsDatabaseManager sut)
        {
            // Arrange
            var key = new RegsKey(keyValue, RegsDataType.String);
            var insertValue = new RegsStringEntry(entryValue);

            // Act
            var wasInserted = sut.Insert(0, key, insertValue);

            // Assert
            wasInserted.ShouldBeTrue();
        }
    }
}
