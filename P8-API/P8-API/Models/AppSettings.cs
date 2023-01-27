using System.Diagnostics.CodeAnalysis;

namespace P8_API.Models
{
    [ExcludeFromCodeCoverage]
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public string GoogleAPIKey { get; set; }
    }
}
