using sweaWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Interface
{
    public interface IExchange
    {
        Task<DateTime> FindClosestBankingDay(DateTime dateToCheck);

        Task<SearchRequestParameters> CreateSearchParamsAsync(DateTime dateToCheck, string currency);
    }
}
