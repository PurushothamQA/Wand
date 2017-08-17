using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace WandAutomation
{
    public class BasePage
    {
        public static readonly IWebDriver Driver = new FirefoxDriver();

        public static void Main(string[] args)
        {
            const string url = "https://prowand.pro-unlimited.com/login.html";
            Driver.Navigate().GoToUrl(url);
          
            WandLogin.Login();
            Driver.Manage().Window.Maximize();
            EnterTimesheets.SelectFromDropDown("selectedBillingType", "BillingType");
            Thread.Sleep(500);
            EnterTimesheets.SelectFromDropDown( "dateRangeString", "DateRange");
            EnterTimesheets.ClickSubmit();
            EnterTimesheets.ClickDefaultLink();
            EnterTimesheets.EnterRegularHours();
            EnterTimesheets.SelectChargeCode();
            EnterTimesheets.NoLunchBreak();
            EnterTimesheets.ClickOnFinalSubmit();

        }
    }
}
