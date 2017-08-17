using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace WandAutomation
{
    public class WandLogin : BasePage
    {
       
        
        public static void Login()
        {
            Driver.FindElement(By.Id("usernamefield")).SendKeys(ReadFromExcel.ReadCredentialsFromExcel()[0]);
            Driver.FindElement(By.Id("passwordfield")).SendKeys(ReadFromExcel.ReadCredentialsFromExcel()[1]);
            Driver.FindElement(By.Name("loginButton")).Click();
            
        }
    }
}
