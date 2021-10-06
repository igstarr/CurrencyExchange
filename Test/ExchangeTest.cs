using Biz.Interface;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ExchangeTest
    {
        [Fact]
        public async Task CheckIfBankingDay_SuccessAsync()
        {
            var TestMock = new Mock<IApiService>();
            TestMock.SetupSequence(_ => _.CheckIfBankingDay(
                It.IsAny<DateTime>()))
                .Returns(Task.FromResult(true));  //Returned in the second call

            var sut = new Biz.Logic.Exchange(TestMock.Object);

            var response = await sut.FindClosestBankingDay(DateTime.Today);
            Assert.True(response == DateTime.Today);

        }
        [Fact]

        public async Task CheckIfBankingDayOnRetry_SuccessAsync()
        {
            var TestMock = new Mock<IApiService>();
            TestMock.SetupSequence(_ => _.CheckIfBankingDay(
                It.IsAny<DateTime>()))
                .Returns(Task.FromResult(false))
                .Returns(Task.FromResult(true));  //Returned in the second call

            var sut = new Biz.Logic.Exchange(TestMock.Object);

            var response = await sut.FindClosestBankingDay(DateTime.Today);
            Assert.True(response == DateTime.Today.AddDays(-1));

        }
    }
}