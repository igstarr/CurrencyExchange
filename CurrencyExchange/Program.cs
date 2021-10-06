// See https://aka.ms/new-console-template for more information

using Biz.Classes;
using Biz.Interface;
using Biz.Logic;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddTransient<IExchange, Exchange>()
    .AddTransient<IApiService, ApiService>()
    .BuildServiceProvider();


bool keeprunning = true;

while (keeprunning)
{

    Console.WriteLine($"Input your exchange options  ([Value] [From Currency] [To Currency] [Date YYYY-MM-DD). Available currencies: {AllowedCurrencies.SEK}, {AllowedCurrencies.USD}, {AllowedCurrencies.EUR}, {AllowedCurrencies.CAD}");

    List<string> Errors = new List<string>();
    var fromCurrency = "";
    var toCurrency = "";

    string input = Console.ReadLine().ToUpper();
    var splitstring = input.Split(' ');
    if (splitstring.Length < 4)
        Errors.Add("To few inputs");
    if (splitstring.Length > 4)
        Errors.Add("To many inputs");

    if (Errors.Count == 0)
    {
        if (!int.TryParse(splitstring[0], out int amount))
            Errors.Add("Cant parse amount");
        if (AllowedCurrencies.CheckIfCurrencyIsAllowed(splitstring[1]))
            fromCurrency = splitstring[1];
        else
            Errors.Add("From currency is not supported");

        if (AllowedCurrencies.CheckIfCurrencyIsAllowed(splitstring[2]))
            toCurrency = splitstring[2];
        else
            Errors.Add("To currency is not supported");

        if (toCurrency == fromCurrency)
            Errors.Add("Cant use the same currencies");

        if (!DateTime.TryParse(splitstring[3], out DateTime dateToCheck))
            Errors.Add("Cant parse date");
        else if (dateToCheck > DateTime.Now)
            Errors.Add("Cant check currency in the future");


        if (Errors.Count == 0)
        {
            var bar = serviceProvider.GetService<IExchange>();
            var currency = await bar.Run(dateToCheck, amount, fromCurrency, toCurrency);
            Console.WriteLine($"Your {currency.Amount} {currency.FromCurrency} will become: {Math.Round(currency.OutAmount, 4)} " +
                $"{currency.ToCurrency} which gives an exchange rate of: {currency.ExchangeRate} @Banking day: {currency.DateToCheck.Date}");
        }
    }
    foreach (var error in Errors)
        Console.WriteLine(error);

}