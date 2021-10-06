using Biz;
using Biz.Classes;
using Biz.Interface;
using Moq;
using sweaWebService;
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
        [Theory]
        [InlineData(AllowedCurrencies.EUR, AllowedCurrencies.USD, 10.159, 8.7812, 1.1569)]
        [InlineData(AllowedCurrencies.USD, AllowedCurrencies.EUR, 8.7812, 10.159,  0.8644)]
        [InlineData(AllowedCurrencies.SEK, AllowedCurrencies.EUR, 10.159, 10.159, 0.0984)]
        [InlineData(AllowedCurrencies.EUR, AllowedCurrencies.SEK, 10.159, 10.159, 10.159)]
        [InlineData(AllowedCurrencies.USD, AllowedCurrencies.SEK, 8.7812, 8.7812, 8.7812)]
        [InlineData(AllowedCurrencies.SEK, AllowedCurrencies.USD, 8.7812, 8.7812, 0.1139)]

        public async Task TestExchange(string from, string to, double rate1, double rate2, double expected)
        {
            //Arrange             
            var exchangerates = new double[] { rate1, rate2 };

            var TestMock = new Mock<IApiService>();
            TestMock.SetupSequence(_ => _.GetInterestAndExchangeRates(
                It.IsAny<SearchRequestParameters>()))
                .Returns(Task.FromResult(exchangerates[0]))  //Returned in the first call
                .Returns(Task.FromResult(exchangerates[1])); //Returned in the second call

            var mockTestParams = new Mock<IExchange>();
            mockTestParams.Setup(m => m.CreateSearchParamsAsync(
                It.IsAny<DateTime>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(new SearchRequestParameters()));

            var sut = new Biz.Logic.Exchange(TestMock.Object);
            var currency = new CurrencyIn(DateTime.Now, 100, from, to);


            var item = await sut.GetExchangeRate(currency);

            Assert.True(item == expected);
        }
    }
}