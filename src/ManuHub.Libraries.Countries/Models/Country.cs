using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ManuHub.Libraries.Countries.Models
{
    public class Country
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("emoji")]
        public string Emoji { get; set; } = default!;

        [JsonPropertyName("alpha2")]
        public string Alpha2 { get; set; } = default!;

        [JsonPropertyName("alpha3")]
        public string Alpha3 { get; set; } = default!;

        [JsonPropertyName("dialCode")]
        public string DialCode { get; set; } = default!;

        [JsonPropertyName("timezones")]
        public List<string> Timezones { get; set; } = new List<string>();

        [JsonPropertyName("capital")]
        public string Capital { get; set; } = default!;

        [JsonPropertyName("region")]
        public string Region { get; set; } = default!;

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonIgnore]
        public List<string> UTCOffsets =>
            Timezones
                ?.Select(tz =>
                {
                    try
                    {
                        TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(tz);
                        TimeSpan offset = tzInfo.BaseUtcOffset;
                        return $"UTC{(offset >= TimeSpan.Zero ? "+" : "-")}{offset:hh\\:mm}";
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(x => x != null)
                .Distinct()
                .ToList() ?? new List<string>();

        public override string ToString() =>
            $"{Emoji} {Name} ({DialCode}, {UTCOffsets.FirstOrDefault()})";
    }
}
