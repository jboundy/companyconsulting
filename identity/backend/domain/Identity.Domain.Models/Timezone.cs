using System.Text.RegularExpressions;

namespace Identity.Domain.Models
{
    public readonly partial record struct TimeZone
    {
        public static readonly TimeZone Utc = new("UTC");
        public static readonly TimeZone Default = new("America/Chicago");
        public string Value { get; }
        private TimeZone(string value) => Value = value;
        public static bool TryCreate(string? input, out TimeZone timeZone, out string? error)
        {
            timeZone = default;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Time zone is required.";
                return false;
            }

            string trimmed = input.Trim();

            if (string.Equals(trimmed, "UTC", StringComparison.OrdinalIgnoreCase))
            {
                timeZone = Utc;
                return true;
            }

            if (!IanaZonePattern().IsMatch(trimmed))
            {
                error = $"'{input}' is not a valid IANA time zone ID (expected e.g. 'America/Chicago').";
                return false;
            }

            try
            {
                TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(trimmed);

                string canonicalId = tzInfo.HasIanaId && TimeZoneInfo.TryConvertWindowsIdToIanaId(tzInfo.Id, out string? ianaId)
                    ? ianaId
                    : tzInfo.Id;

                timeZone = new TimeZone(canonicalId);
                return true;
            }
            catch (TimeZoneNotFoundException)
            {
                error = $"'{input}' is not a recognized system time zone.";
                return false;
            }
            catch (InvalidTimeZoneException)
            {
                error = $"'{input}' maps to an invalid time zone definition.";
                return false;
            }
        }

        public TimeZoneInfo ToTimeZoneInfo() => TimeZoneInfo.FindSystemTimeZoneById(Value);

        public DateTimeOffset ToLocal(DateTimeOffset utcTimestamp) =>
            TimeZoneInfo.ConvertTime(utcTimestamp, ToTimeZoneInfo());

        public override string ToString() => Value;

        // Matches IANA zone IDs like "America/Chicago", "America/Argentina/Buenos_Aires", "Etc/GMT+5"
        [GeneratedRegex(@"^(?:Etc/[A-Za-z0-9+\-]+|[A-Za-z_]+/[A-Za-z0-9_+\-]+(?:/[A-Za-z0-9_+\-]+)?)$", RegexOptions.CultureInvariant)]
        private static partial Regex IanaZonePattern();
    }
}