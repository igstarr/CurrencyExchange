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
    }
}
