using ManuHub.Libraries.Countries;

Console.Clear();
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("==========================================");

var byAlpha2 = CountryProvider.GetByAlpha2("IN");
if (byAlpha2.Success)
    Console.WriteLine(byAlpha2.Countries.First().Name);
else
    Console.WriteLine(byAlpha2.ErrorMessage);

Console.WriteLine("==========================================");

var byRegion = CountryProvider.GetByRegion("Asia");
if (byRegion.Success)
    foreach (var c in byRegion.Countries)
        Console.WriteLine(c.Name);

Console.WriteLine("==========================================");

var search = CountryProvider.SearchByName("land");
if (search.Success)
    foreach (var c in search.Countries)
        Console.WriteLine(c.Name);

Console.WriteLine("==========================================");
var india = CountryProvider.GetByAlpha2("IN").Countries.First();
Console.WriteLine(india);
