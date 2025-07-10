using System.Collections.Generic;
using System.Linq;

namespace ManuHub.Libraries.Countries.Models
{
    public class CountryResult
    {
        public bool Success { get; }
        public string? ErrorMessage { get; }
        public IEnumerable<Country> Countries { get; }

        private CountryResult(IEnumerable<Country> countries)
        {
            Success = true;
            Countries = countries;
        }

        private CountryResult(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
            Countries = Enumerable.Empty<Country>();
        }

        public static CountryResult Ok(IEnumerable<Country> countries) => new CountryResult(countries);
        public static CountryResult Fail(string message) => new CountryResult(message);
    }
}
