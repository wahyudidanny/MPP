
using OpenQA.Selenium;
using MPP.Service.Models;
using MPP.Service.Setup;
using MPP.Service.Interface;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Microsoft.Extensions.Http;

using RestSharp;
public class Program
{

    private static AppSettings? appSettings;
    private static SettingWAChrome? settingWAChrome;

    private static void Main(string[] args)
    {

        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        appSettings = new AppSettings(configuration);
        settingWAChrome = new SettingWAChrome(configuration);

        var services = new ServiceCollection();
        services.RegisterContext(configuration);
        services.RegisterService(configuration);
        services.AddHttpClient();


        var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var _businessUnitService = serviceProvider.GetService<IDataService>();

        var dataKebun = _businessUnitService?.GetDataBusinessUnit().ToList();

        if (appSettings.GetValue("flagGeneratePDF") == "1")
        {

            foreach (var data in dataKebun)
            {
                try
                {
                    GeneratePDF(data.Company, data.Location, "2024", "1", httpClientFactory);

                }
                catch (Exception err)
                {


                }
            }

        }

        if (appSettings.GetValue("flagSendWA") == "1")
        {
            openChromeWhatsap();



        }





    }

    public static async Task<RestResponse> GetAsync(string url, string? jwtToken = "")
    {
        var client = new RestClient();
        //client.Timeout = -1;
        var request = new RestRequest(url, Method.Get);
        request.AddHeader("Content-Type", "application/json");
        if (!string.IsNullOrEmpty(jwtToken))
            request.AddHeader("Authorization", $"Bearer {jwtToken}");

        return await client.ExecuteAsync(request);

    }

    private async static void GeneratePDF(string? company, string? location, string tahun, string bulan, IHttpClientFactory httpClientFactory)
    {

        string apiUrl = $"{settingWAChrome.apiBaseUrl}/api/MPP/GenerateApprovalMPP/?company={company}&location={location}&tahun={tahun}&bulan={bulan}";

        using (var httpClient = httpClientFactory.CreateClient())
        {
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

            }
        }





    }


    private static async Task openChromeWhatsap()
    {


        string ChromeUserData = settingWAChrome.chromeUserData;

        var options = new ChromeOptions();
        options.AddArgument(ChromeUserData);


        using (IWebDriver driver = new ChromeDriver(settingWAChrome.chromeDriverPath, options))
        {
            try
            {
                driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(60);
                Thread.Sleep(3000);
                SendToWA(driver);
                driver.Quit();

            }
            catch (Exception ex)
            {
                driver.Quit();
                Console.WriteLine(ex.ToString());

            }
        }

    }



    static void SendToWA(IWebDriver driver)
    {

        IWebElement searchBox = driver.FindElement(By.XPath(settingWAChrome.waSearchBox));
        searchBox.SendKeys(settingWAChrome.waRecipientGroup);

        Thread.Sleep(3000);

        driver.FindElement(By.XPath(string.Format(settingWAChrome.waSelectSearchResult ?? "", "VRA"))).Click();

        string[] files = Directory.GetFiles(appSettings.GetValue("filePathRiau"));

        foreach (string file in files)
        {
            IWebElement attachmentButton = driver.FindElement(By.CssSelector(settingWAChrome.waAttachmentButton));
            attachmentButton.Click();
            Thread.Sleep(5000);
            IWebElement fileInput = driver.FindElement(By.CssSelector(settingWAChrome.waInputFileDefault));
            fileInput.SendKeys(file);
            driver.FindElement(By.XPath(settingWAChrome.waTextBoxMessageImage)).SendKeys(Keys.Enter);
            Thread.Sleep(5000);
        }


    }

}