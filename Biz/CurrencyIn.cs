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
            this.DateToCheck = dateToCheck;
            Amount = amount;
            this.FromCurrency = fromCurrency ?? throw new ArgumentNullException(nameof(fromCurrency));
            this.ToCurrency = toCurrency ?? throw new ArgumentNullException(nameof(toCurrency));
        }
        public void UpdateExchangeRateAndAmount(double exchangeRate)
        {
            ExchangeRate = exchangeRate;
            OutAmount = Amount * exchangeRate;
        }
        public DateTime DateToCheck { get; set; }
        public int Amount { get; private set; }
        public string FromCurrency { get; private set; }
        public string ToCurrency { get; private set; }
        public double OutAmount { get; private set; }
        public double ExchangeRate { get; private set; }
    }
}
