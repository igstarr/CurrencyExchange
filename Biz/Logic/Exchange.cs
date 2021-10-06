using Biz.Interface;
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
    }
}
