using MPP.Job.Entities;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

public class Program
{

    private static AppSettings appSettings;
    private static SettingWAChrome settingWAChrome;
    static async Task Main(string[] args)
    {

    
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        appSettings = new AppSettings(configuration);
        settingWAChrome = new SettingWAChrome(configuration);

        await openChromeWhatshap();


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