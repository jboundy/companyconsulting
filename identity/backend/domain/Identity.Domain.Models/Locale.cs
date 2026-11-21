using System.Globalization;
using System.Text.RegularExpressions;

namespace Identity.Domain.Models
{
    public readonly partial record struct Locale
    {
        public static readonly Locale Default = new("en-US");

        public string Value { get; }

        private Locale(string value) => Value = value;

        public static bool TryCreate(string? input, out Locale locale, out string? error)
        {
            locale = default;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Locale is required.";
                return false;
            }

            string trimmed = input.Trim().Replace('_', '-');

            if (!Bcp47Pattern().IsMatch(trimmed))
            {
                error = $"'{input}' is not a valid locale format (expected e.g. 'en-US' or 'fr-CA').";
                return false;
            }

            try
            {
                CultureInfo culture = CultureInfo.GetCultureInfo(trimmed);
                if (string.IsNullOrEmpty(culture.Name))
                {
                    error = $"'{input}' is not a recognized culture.";
                    return false;
                }

                locale = new Locale(culture.Name);
                return true;
            }
            catch (CultureNotFoundException)
            {
                error = $"'{input}' is not a supported culture.";
                return false;
            }
        }

        public CultureInfo ToCultureInfo() => CultureInfo.GetCultureInfo(Value);
        public override string ToString() => Value;

        // Matches tags like "en", "en-US", "pt-BR", "zh-Hant-TW", "es-419"
        [GeneratedRegex(@"^[a-zA-Z]{2,3}(?:-[A-Za-z]{4})?(?:-(?:[A-Za-z]{2}|\d{3}))?$", RegexOptions.CultureInvariant)]
        private static partial Regex Bcp47Pattern();
    }
}