# ManuHub.Libraries.Countries

![NuGet](https://img.shields.io/badge/ManuHub.Libraries.Countries-red) ![NuGet Version](https://img.shields.io/nuget/v/ManuHub.Libraries.Countries) ![NuGet Downloads](https://img.shields.io/nuget/dt/ManuHub.Libraries.Countries)

A comprehensive .NET Standard library providing detailed country data, including:

- Country names and emoji flags
- ISO Alpha-2 and Alpha-3 codes
- International dialing (ISD) codes
- Timezone information with human-readable UTC offsets
- Capital cities, geographic coordinates, regions, and descriptions

## Installation

You can install the package via NuGet:

```bash
dotnet add package ManuHub.Libraries.Countries
````

Or via the NuGet Package Manager:

```powershell
Install-Package ManuHub.Libraries.Countries
```

## Usage

### Get all countries

```csharp
using ManuHub.Libraries.Countries;

var allCountriesResult = CountryProvider.GetAllCountries();
if (allCountriesResult.Success)
{
    foreach (var country in allCountriesResult.Countries)
    {
        Console.WriteLine(country);
    }
}
else
{
    Console.WriteLine($"Error: {allCountriesResult.ErrorMessage}");
}
```

### Lookup by Alpha-2 code

```csharp
var indiaResult = CountryProvider.GetByAlpha2("IN");
if (indiaResult.Success)
{
    var india = indiaResult.Countries.First();
    Console.WriteLine(india);  // 🇮🇳 India (+91, UTC+05:30)
}
else
{
    Console.WriteLine(indiaResult.ErrorMessage);
}
```

### Search countries by partial name

```csharp
var searchResult = CountryProvider.SearchByName("land");
if (searchResult.Success)
{
    foreach (var country in searchResult.Countries)
    {
        Console.WriteLine(country.Name);
    }
}
```

### Filter countries by region

```csharp
var asiaResult = CountryProvider.GetByRegion("Asia");
if (asiaResult.Success)
{
    foreach (var country in asiaResult.Countries)
    {
        Console.WriteLine($"{country.Name} ({country.Capital})");
    }
}
```

### ToString() Output

Every `Country` object has a clean string representation for display:

```csharp
var india = CountryProvider.GetByAlpha2("IN").Countries.First();
Console.WriteLine(india);
```

#### Result
```scss
🇮🇳 India (+91, UTC+05:30)
```

## Features

* Embedded JSON data for fast and offline access
* ISO standard codes (Alpha-2 and Alpha-3)
* Dialing codes with '+' sign
* Timezone IDs converted to UTC offsets automatically
* Latitude/longitude for mapping
* Description fields for additional context
* Search and filter by multiple criteria with caching and indexing
* Clean `CountryResult` wrapper for success/error handling

## Requirements

* .NET Standard 2.0+ compatible (works in .NET Core, .NET 5+, .NET Framework 4.7.2+)

## Contributing

Contributions, issues, and feature requests are welcome! Feel free to open a pull request or issue.

## License

MIT License. See [LICENSE](https://github.com/manusoft/manuhub-libraries/blob/master/LICENSE.txt) for details.

---

Made with ❤️ by ManuHub.


