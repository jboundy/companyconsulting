namespace Identity.Domain.Models
{
    public class UserProfile
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? DisplayName { get; set; }
        public int MyProperty { get; set; }
        public Locale Locale { get; set; } = Locale.Default;
        public TimeZone TimeZone { get; set; } = TimeZone.Default;
        public required UserPreferences Preferences { get; set; }
    }
}