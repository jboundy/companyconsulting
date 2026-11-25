namespace Identity.Domain.Models
{
    public class UserProfile
    {
        public required int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? DisplayName { get; set; }
        public Locale Locale { get; set; } = Locale.Default;
        public TimeZoneStruct TimeZone { get; set; } = TimeZoneStruct.Default;
        public required UserPreferences Preferences { get; set; }
    }
}