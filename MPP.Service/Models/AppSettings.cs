using Microsoft.Extensions.Configuration;

namespace MPP.Service.Models
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;
        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? apiBaseUrl { get { return GetValue("apiBaseUrl"); } }
        public string? filePath { get { return GetValue("filePath"); } }
        public string? generatePDF { get { return GetValue("generatePDF"); } }
        public string? sendPDFProd { get { return GetValue("sendPDFProd"); } }

        public string? GetValue(string key)
        {
            var result = _configuration.GetSection("AppSettings")[key];
            return result;
        }

    }
}