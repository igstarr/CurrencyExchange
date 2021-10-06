using Biz.Interface;
using sweaWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Logic
{
    public class Exchange : IExchange
    {
        private readonly IApiService _service;
        public Exchange(IApiService service)
        {
            _service = service;
        }
        public async Task<DateTime> FindClosestBankingDay(DateTime dateToCheck)
        {
            bool bankingDay = false;
            while (!bankingDay)
            {

                if (await _service.CheckIfBankingDay(dateToCheck))
                    break;
                dateToCheck = dateToCheck.AddDays(-1);
            }
            return dateToCheck;
        }
        public async Task<string> Run(DateTime dateToCheck, int amount, string fromCurrency, string toCurrency)
        {
            var currency = new CurrencyIn(dateToCheck, amount, fromCurrency, toCurrency);
            var exchangeRate = GetExchangeRate(currency);
            return "";
        }
        public async Task<double> GetExchangeRate(CurrencyIn currency) 
        {
            var searchParamsToCurrency = await CreateSearchParamsAsync(currency.dateToCheck, currency.toCurrency);
            var searchParamsFromCurrency = await CreateSearchParamsAsync(currency.dateToCheck, currency.fromCurrency);

            var itemToExchange = await _service.GetInterestAndExchangeRates(searchParamsToCurrency);

            var itemFromExchange = currency.fromCurrency == "SEK" ? 1 : await _service.GetInterestAndExchangeRates(searchParamsFromCurrency);

            return currency.toCurrency != "SEK" ? ConvertUnknownCurrency((double)itemFromExchange, (double)itemToExchange) : (double)itemFromExchange;
        }
        private static double ConvertUnknownCurrency(double from, double to)
        {
            return from / to;
        }
        public async Task<SearchRequestParameters> CreateSearchParamsAsync(DateTime dateToCheck, string currency)
        {
            return new SearchRequestParameters
            {
                datefrom = dateToCheck,
                dateto = dateToCheck,
                aggregateMethod = AggregateMethodType.D,
                languageid = LanguageType.en,
                min = true,
                max = true,
                ultimo = false,
                searchGroupSeries = await _service.GetSeries(currency)
            };
        }
    }
}
