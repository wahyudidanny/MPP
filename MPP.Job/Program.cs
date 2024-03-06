
using OpenQA.Selenium;
using MPP.Service.Models;
using MPP.Service.Setup;
using MPP.Service.Interface;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

public class Program
{

    private static AppSettings? appSettings;
    private static SettingWAChrome? settingWAChrome;

    static async Task Main(string[] args)
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

        var yearGenerate = DateTime.Now.Year.ToString();
        var monthGenerate = DateTime.Now.Month.ToString();

        var msBusinessUnit = _businessUnitService?.GetDataBusinessUnit().ToList() ?? new List<T_MsBusinessUnit>();
        var distinctRegionCode = _businessUnitService?.GetDataRegionDistinct(msBusinessUnit).ToList();

        if (msBusinessUnit.Count() > 0)
        {

            if (appSettings.GetValue("flagGeneratePDF") == "1")
            {

                foreach (var data in distinctRegionCode)
                {
                    string groupFilePath = getFilePath(data.RegionCode.ToString());
                    if (string.IsNullOrEmpty(groupFilePath)) continue;

                    string[] files = Directory.GetFiles(groupFilePath);

                    foreach (string file in files)
                    {
                        File.Delete(file);
                        Console.WriteLine($"Deleted file: {file}");
                    }

                }

                foreach (var data in msBusinessUnit)
                {
                    try
                    {
                        await GeneratePDF(data.Company, data.Location, yearGenerate, monthGenerate, data.RegionCode, httpClientFactory);

                    }
                    catch (Exception err)
                    {

                        Console.WriteLine(err.ToString());

                    }
                }

            }

        }
        else
        {

            Console.WriteLine("Data business unit is empty, please check your script");

        }


        if (appSettings.GetValue("flagSendWA") == "1")
        {
            await openChromeWhatsap(distinctRegionCode);

        }

    }



    static async Task GeneratePDF(string? company, string? location, string tahun, string bulan, string? kodeRegion, IHttpClientFactory? httpClientFactory)
    {

        try
        {
            string apiUrl = $"{settingWAChrome.apiBaseUrl}/api/MPP/GenerateApprovalMPP/?company={company}&location={location}&kodeRegion={kodeRegion}&tahun={tahun}&bulan={bulan}";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response from API:" + responseContent);

                }

            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Response from API:" + e.ToString());
        }

    }

    private static async Task openChromeWhatsap(List<RegionCodeDistinct> distinctRegionModels)
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
                SendToWA(driver, distinctRegionModels);
                driver.Quit();

            }
            catch (Exception ex)
            {
                driver.Quit();
                Console.WriteLine(ex.ToString());

            }
        }

    }



    static void SendToWA(IWebDriver driver, List<RegionCodeDistinct> distinctRegionModels)
    {


        foreach (var val in distinctRegionModels)
        {
            string groupWAGName = getGroupSensus(val.RegionCode.ToString());
            
            if (string.IsNullOrEmpty(groupWAGName)) continue;

            string[] files = Directory.GetFiles(getFilePath(val.RegionCode.ToString()));

            if (files.Count() > 0)
            {
                IWebElement searchBox = driver.FindElement(By.XPath(settingWAChrome.waSearchBox));

                searchBox.SendKeys(groupWAGName);

                Thread.Sleep(5000);
                string searchResultEl = string.Format(settingWAChrome.waSelectSearchResult, groupWAGName);
                driver.FindElement(By.XPath(searchResultEl)).Click();
                driver.FindElement(By.XPath(settingWAChrome.waTextBoxMessage)).SendKeys("In a few minutes you will recieved auto notification from *WhatsApp Automation System*"); //--> Element text message value
                driver.FindElement(By.XPath(settingWAChrome.waTextBoxMessage)).SendKeys(Keys.Enter);
                Thread.Sleep(3000);

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

                driver.FindElement(By.XPath(settingWAChrome.waTextBoxMessage)).SendKeys("Thankyou *WhatsApp Automation System*"); //--> Element text message value
                driver.FindElement(By.XPath(settingWAChrome.waTextBoxMessage)).SendKeys(Keys.Enter);

                Thread.Sleep(5000);

            }

        }

    }

    public static string getFilePath(string kodeRegion)
    {
        string pathFile = string.Empty;

        if (kodeRegion.ToLower() == "pku")
            pathFile = appSettings.GetValue("filePathRiau");
        else if (kodeRegion.ToLower() == "ptk")
            pathFile = appSettings.GetValue("filePathKalbar");
        else if (kodeRegion.ToLower() == "bpn")
            pathFile = appSettings.GetValue("filePathKaltimFR");

        return pathFile;
    }


    public static string getGroupSensus(string kodeRegion)
    {
        string groupSensus = string.Empty;

        if (kodeRegion.ToLower() == "pku") // pku
            groupSensus = appSettings.GetValue("waReceiptGroupRiauFR");
        else if (kodeRegion.ToLower() == "ptk") //kalbar
            groupSensus = appSettings.GetValue("waReceiptGroupKalbarFR");
        else if (kodeRegion.ToLower() == "bpn") // kaltim fr
            groupSensus = appSettings.GetValue("waReceiptGroupKaltimFR");

        return groupSensus;
    }


}