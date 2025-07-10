using ManuHub.Libraries.Countries.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ManuHub.Libraries.Countries
{
    public static class CountryProvider
    {
        private static List<Country>? _countries;
        private static Dictionary<string, Country>? _alpha2Index;
        private static Dictionary<string, Country>? _alpha3Index;
        private static Dictionary<string, List<Country>>? _regionIndex;
        private static Dictionary<string, List<Country>>? _dialCodeIndex;

        public static CountryResult GetAllCountries()
        {
            try
            {
                var all = _countries ??= LoadFromEmbeddedJson();
                return CountryResult.Ok(all);
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Failed to load countries: {ex.Message}");
            }
        }

        public static CountryResult GetByAlpha2(string alpha2)
        {
            if (string.IsNullOrWhiteSpace(alpha2))
                return CountryResult.Fail("Alpha2 code is null or empty.");

            try
            {
                EnsureIndexes();
                if (_alpha2Index!.TryGetValue(alpha2.ToUpperInvariant(), out var country))
                    return CountryResult.Ok(new[] { country });

                return CountryResult.Fail($"Country with Alpha2 '{alpha2}' not found.");
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Error retrieving country: {ex.Message}");
            }
        }

        public static CountryResult GetByAlpha3(string alpha3)
        {
            if (string.IsNullOrWhiteSpace(alpha3))
                return CountryResult.Fail("Alpha3 code is null or empty.");

            try
            {
                EnsureIndexes();
                if (_alpha3Index!.TryGetValue(alpha3.ToUpperInvariant(), out var country))
                    return CountryResult.Ok(new[] { country });

                return CountryResult.Fail($"Country with Alpha3 '{alpha3}' not found.");
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Error retrieving country: {ex.Message}");
            }
        }

        public static CountryResult GetByDialCode(string dialCode)
        {
            if (string.IsNullOrWhiteSpace(dialCode))
                return CountryResult.Fail("Dial code is null or empty.");

            try
            {
                EnsureIndexes();
                if (_dialCodeIndex!.TryGetValue(dialCode, out var countries))
                    return CountryResult.Ok(countries);

                return CountryResult.Fail($"Country with dial code '{dialCode}' not found.");
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Error retrieving country: {ex.Message}");
            }
        }

        public static CountryResult GetByRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return CountryResult.Fail("Region is null or empty.");

            try
            {
                EnsureIndexes();
                if (_regionIndex!.TryGetValue(region, out var countries))
                    return CountryResult.Ok(countries);

                return CountryResult.Fail($"No countries found in region '{region}'.");
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Error retrieving countries: {ex.Message}");
            }
        }

        public static CountryResult SearchByName(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return CountryResult.Fail("Search term is null or empty.");

            try
            {
                var results = GetAllCountries().Countries
                    .Where(c => c.Name.Contains(partialName, StringComparison.OrdinalIgnoreCase));
                return CountryResult.Ok(results);
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Search error: {ex.Message}");
            }
        }

        public static CountryResult SearchByCapital(string partialCapital)
        {
            if (string.IsNullOrWhiteSpace(partialCapital))
                return CountryResult.Fail("Search term is null or empty.");

            try
            {
                var results = GetAllCountries().Countries
                    .Where(c => c.Capital.Contains(partialCapital, StringComparison.OrdinalIgnoreCase));
                return CountryResult.Ok(results);
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Search error: {ex.Message}");
            }
        }

        public static CountryResult SearchByDescription(string partialDescription)
        {
            if (string.IsNullOrWhiteSpace(partialDescription))
                return CountryResult.Fail("Search term is null or empty.");

            try
            {
                var results = GetAllCountries().Countries
                    .Where(c => c.Description.Contains(partialDescription, StringComparison.OrdinalIgnoreCase));
                return CountryResult.Ok(results);
            }
            catch (Exception ex)
            {
                return CountryResult.Fail($"Search error: {ex.Message}");
            }
        }

        private static void EnsureIndexes()
        {
            if (_alpha2Index != null) return;

            var countries = GetAllCountries().Countries.ToList();

            _alpha2Index = countries
                .Where(c => !string.IsNullOrWhiteSpace(c.Alpha2))
                .ToDictionary(c => c.Alpha2.ToUpperInvariant(), c => c);

            _alpha3Index = countries
                .Where(c => !string.IsNullOrWhiteSpace(c.Alpha3))
                .ToDictionary(c => c.Alpha3.ToUpperInvariant(), c => c);

            _regionIndex = countries
                .Where(c => !string.IsNullOrWhiteSpace(c.Region))
                .GroupBy(c => c.Region, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

            _dialCodeIndex = countries
                .Where(c => !string.IsNullOrWhiteSpace(c.DialCode))
                .GroupBy(c => c.DialCode)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private static List<Country> LoadFromEmbeddedJson()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith("countries.json", StringComparison.OrdinalIgnoreCase));

            if (resourceName is null)
                throw new FileNotFoundException("Embedded 'countries.json' not found.");

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new InvalidOperationException("Unable to read embedded 'countries.json'.");

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            return JsonSerializer.Deserialize<List<Country>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Country>();
        }
    }
}