using sweaWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Logic
{
    public class ApiService
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
            foreach (var item in items.@return.Where(x => x.groupid == "130"))
            {
                if (item.seriesid.Contains(currency))
                {
                    var additem = new SearchGroupSeries
                    {
                        seriesid = item.seriesid,
                        groupid = item.groupid
                    };
                    returnitems.Add(additem);
                }
            }
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
