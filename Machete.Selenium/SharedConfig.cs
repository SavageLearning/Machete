using System;
using Microsoft.Extensions.Configuration;

namespace Machete.Test
{
    public static class SharedConfig
    {
        private static IConfiguration Configuration { get; set; }
        public static string ChromeDriverPath { get; }
        public static string BaseSeleniumTestUrl { get; }
        public static string SeleniumUser { get; }
        public static string SeleniumUserPassword { get; }
        
        /// <summary>
        /// Reads the appsettings.json file and create public properties
        /// necessary for Selenium tests against a specific baseUrl
        /// </summary>
        static SharedConfig()
        {
            var builder = new ConfigurationBuilder()     
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            ChromeDriverPath = Configuration["chromeDriverPath"];
            BaseSeleniumTestUrl = Configuration["baseSeleniumTestUrl"];
            SeleniumUser = Configuration["seleniumUser"];
            SeleniumUserPassword = Configuration["SeleniumUserPassword"];

            var sharedConfigOkToRun = BaseSeleniumTestUrl.Contains("test")
                                          || BaseSeleniumTestUrl.Contains("localhost")
                                          || BaseSeleniumTestUrl.Contains("blueprint")
                                          || BaseSeleniumTestUrl.Contains("default");
            if (!sharedConfigOkToRun) throw new Exception("Unable to run Selenium tests outside test environments");
        }
    }
}
