// See https://aka.ms/new-console-template for more information
bool keeprunning = true;

while (keeprunning)
{

    Console.WriteLine("Input your exchange options  ([Value] [From Currency] [To Currency] [Date YYYY-MM-DD). Available currencies: EUR, SEK, USD");

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
            Errors.Add("Cante parse amount");


        if (splitstring[1] == "EUR" || splitstring[1] == "SEK" || splitstring[1] == "USD")
            fromCurrency = splitstring[1];
        else
            Errors.Add("From currency is not supported");

        if (splitstring[2] == "EUR" || splitstring[2] == "SEK" || splitstring[2] == "USD")
            toCurrency = splitstring[2];
        else
            Errors.Add("To currency is not supported");

        if (!DateTime.TryParse(splitstring[3], out DateTime dateToCheck))
            Errors.Add("Cant parse date");
        else if (dateToCheck > DateTime.Now)
            Errors.Add("Cant check currency in the future");


        if (Errors.Count == 0)
        {
            Console.WriteLine("Exchanged");
        }
    }
    foreach (var error in Errors)
        Console.WriteLine(error);

}