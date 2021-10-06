using Biz.Interface;
using sweaWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Logic
{
    public class ApiService : IApiService
    {
        public async Task<bool> CheckIfBankingDay(DateTime dateToCheck)
        {
            var client = new SweaWebServicePortTypeClient();
            var getCalendarDaysResponse = await client.getCalendarDaysAsync(dateToCheck, dateToCheck);
            if (getCalendarDaysResponse.@return != null && getCalendarDaysResponse.@return.FirstOrDefault() != null && getCalendarDaysResponse.@return.FirstOrDefault().bankday == "Y")
            {
                return true;
            }
            return false;
        }
        public async Task<SearchGroupSeries[]> GetSeries(string currency)
        {
            var client = new SweaWebServicePortTypeClient();
            var items = await client.getInterestAndExchangeNamesAsync(11, LanguageType.sv);
            var returnitems = new List<SearchGroupSeries>();

            var item = items.@return.FirstOrDefault(x => x.groupid == "130" && x.seriesid.Contains(currency));
            if (item != null)
                returnitems.Add(new SearchGroupSeries { seriesid = item.seriesid, groupid = item.groupid });
            
            return returnitems.ToArray();
        }
        public async Task<double> GetInterestAndExchangeRates(SearchRequestParameters searchParams)
        {
            var client = new SweaWebServicePortTypeClient();
            var items = await client.getInterestAndExchangeRatesAsync(searchParams);
            return (double)items.@return.groups.FirstOrDefault().series.FirstOrDefault().resultrows.FirstOrDefault().value;
        }
    }
}
