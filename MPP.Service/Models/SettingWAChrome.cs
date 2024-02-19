using Microsoft.Extensions.Configuration;

namespace MPP.Service.Models
{
    public class SettingWAChrome
    {
        private readonly IConfiguration _configuration;
        public SettingWAChrome(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? apiBaseUrl { get { return GetValue("apiBaseUrl"); } }
        public string? chromeDriverPath { get { return GetValue("chromeDriverPath"); } }
        public string? chromeUserData { get { return GetValue("chromeUserData"); } }
        public string? waSearchBox { get { return GetValue("waSearchBox"); } }
        public string? waTextBoxMessage { get { return GetValue("waTextBoxMessage"); } }
        public string? waRecipientGroup { get { return GetValue("waRecipientGroup"); } }
        public string? waSelectSearchResult { get { return GetValue("waSelectSearchResult"); } }
        public string? waAttachmentButton { get { return GetValue("waAttachmentButton"); } }
        public string? waInputFileDefault { get { return GetValue("waInputFileDefault"); } }
        public string? waTextBoxMessageImage { get { return GetValue("waTextBoxMessageImage"); } }

        public string? GetValue(string key)
        {
            var result = _configuration.GetSection("SettingWAChrome")[key];
            return result;
        }

    }
}