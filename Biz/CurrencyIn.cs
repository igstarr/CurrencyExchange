using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz
{
    public class CurrencyIn
    {
        public CurrencyIn(DateTime dateToCheck, int amount, string fromCurrency, string toCurrency)
        {
            this.dateToCheck = dateToCheck;
            Amount = amount;
            this.fromCurrency = fromCurrency ?? throw new ArgumentNullException(nameof(fromCurrency));
            this.toCurrency = toCurrency ?? throw new ArgumentNullException(nameof(toCurrency));
        }

        public DateTime dateToCheck { get; set; }
        public int Amount { get; }
        public string fromCurrency { get; }
        public string toCurrency { get; }
    }
}
