using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Classes
{
    public static class AllowedCurrencies
    {
        public const string EUR = "EUR";
        public const string SEK = "SEK";
        public const string USD = "USD";
        public const string CAD = "CAD";

        public static bool CheckIfCurrencyIsAllowed(string currency) => currency switch
        {
            USD => true,
            EUR => true,
            SEK => true,
            CAD => true,
            _ => false
        };
    }
}
