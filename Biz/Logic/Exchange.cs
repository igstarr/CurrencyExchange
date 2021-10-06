using Biz.Classes;
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
        public async Task<CurrencyIn> Run(DateTime dateToCheck, int amount, string fromCurrency, string toCurrency)
        {
            CurrencyIn? currency = new CurrencyIn(dateToCheck, amount, fromCurrency, toCurrency);
            currency.DateToCheck = await FindClosestBankingDay(dateToCheck);
            currency.UpdateExchangeRateAndAmount(await GetExchangeRate(currency));
            return currency;
        }
        public async Task<double> GetExchangeRate(CurrencyIn currency)
        {
            SearchRequestParameters? searchParamsFromCurrency = await CreateSearchParamsAsync(currency.DateToCheck, currency.FromCurrency);
            double itemFromExchange = currency.FromCurrency == AllowedCurrencies.SEK ? 1 : await _service.GetInterestAndExchangeRates(searchParamsFromCurrency);

            if (currency.ToCurrency == AllowedCurrencies.SEK)
            {
                return Math.Round((double)itemFromExchange, 4);
            }

            SearchRequestParameters? searchParamsToCurrency = await CreateSearchParamsAsync(currency.DateToCheck, currency.ToCurrency);
            double itemToExchange = await _service.GetInterestAndExchangeRates(searchParamsToCurrency);


            return currency.ToCurrency != AllowedCurrencies.SEK ? Math.Round(ConvertUnknownCurrency(itemFromExchange, itemToExchange), 4) : Math.Round((double)itemFromExchange, 4);
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
