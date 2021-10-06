using sweaWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Interface
{
    public interface IApiService
    {
        Task<bool> CheckIfBankingDay(DateTime dateToCheck);
        Task<SearchGroupSeries[]> GetSeries(string currency);
        Task<double> GetInterestAndExchangeRates(SearchRequestParameters searchParams);
    }
}
