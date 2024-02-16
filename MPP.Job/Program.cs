
using OpenQA.Selenium;
using MPP.Job.Entities;
using MPP.Service.Setup;
using MPP.Service.Interface;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        var serviceProvider = services.BuildServiceProvider();
        var _businessUnitService = serviceProvider.GetService<IDataService>();

        var dataKebun = _businessUnitService?.GetDataBusinessUnit().ToList();



              Console.WriteLine(dataKebun.Count());

        if (appSettings.GetValue("flagGeneratePDF") == "1")
        {

                  Console.WriteLine("1");
            foreach (var data in dataKebun)
            {
            //     try
            //     {


            //         GenerateImage(data.Company, data.Location, periodeDesc, data.RegionCode, data.KodeGroup);
            //     }
            //     catch (Exception err)
            //     {
            //         Log.Error($"{data.Company} {data.Location} {data.RegionCode} {data.KodeGroup}", "Error");
            //         Log.Error(err, "Error");
                  Console.WriteLine( data.Company + " _ "  + data.Location + "_" + data.RegionCode);
            //     }
            }

        }



        Console.WriteLine("hello world");


    }


    private static async Task openChromeWhatshap()
    {

        try
        {

            string ChromeUserData = settingWAChrome.chromeUserData;

            var options = new ChromeOptions();
            options.AddArgument(ChromeUserData);


            using (IWebDriver driver = new ChromeDriver(settingWAChrome.chromeDriverPath, options))
            {

                driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(60);
                Thread.Sleep(10000);
                SendToWA(driver);

                driver.Quit();
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());


        }
    }



    static void SendToWA(IWebDriver driver)
    {

        IWebElement searchBox = driver.FindElement(By.XPath(settingWAChrome.waSearchBox));
        searchBox.SendKeys(settingWAChrome.waRecipientGroup);
        //driver.FindElement(By.XPath(settingWAChrome.waTextBoxMessage)).SendKeys("In a few minutes you will recieved auto notification from *WhatsApp Automation System*"); //--> Element text message value
        Console.WriteLine("1");
        Thread.Sleep(3000); // Wait for the search results to load
        Console.WriteLine("2");
        driver.FindElement(By.XPath(string.Format(settingWAChrome.waSelectSearchResult ?? "", "VRA"))).Click();


        IWebElement attachmentButton = driver.FindElement(By.CssSelector(settingWAChrome.waAttachmentButton));
        attachmentButton.Click();
        Thread.Sleep(2000); // Wait for the search results to load

        string imageFullPath = Path.GetFullPath("C:\\AllProject\\Change Request\\2024\\MPP\\MPP.File\\dummytest.pdf");
        IWebElement fileInput = driver.FindElement(By.CssSelector(settingWAChrome.waInputFileDefault));
        fileInput.SendKeys(imageFullPath);
        Thread.Sleep(2000); // Wait for the search results to load
        Console.WriteLine("3");

        driver.FindElement(By.XPath(settingWAChrome.waTextBoxMessageImage)).SendKeys(Keys.Enter);

        Thread.Sleep(10000);

        Console.WriteLine("4");

    }

}