using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace WandAutomation
{
    public class EnterTimesheets : BasePage
    {
        public static readonly IList<int> Leaves = LeavesHolidays();

        public static void SelectFromDropDown(string elementId, string excelKey)
        {
            WebDriverWait waitForSubmitButton = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            waitForSubmitButton.Until(
                ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id(elementId)));
            IWebElement selectType = Driver.FindElement(By.Id(elementId));
            SelectElement select = new SelectElement(selectType);
            select.SelectByValue(ReadFromExcel.ReturnValueFromExcel(excelKey));
        }

        public static void ClickSubmit()
        {
            Driver.FindElement(By.XPath("//img[@src = '/media/images/but_submit.gif']")).Click();
        }

        public static List<int> LeavesHolidays()
        {
            var days = ReadFromExcel.ReturnValueFromExcel("Leaves/Holidays").Split(',');
            if (days.Contains("")) return new List<int>(0);
            var lists = new List<int>();
            foreach (var day in days)
            {
                var i = (int) (DayOfWeek) Enum.Parse(typeof(DayOfWeek), day);
                lists.Add(i);
            }
            return lists;
        }

    public static void EnterRegularHours()
        {
            for (var i = 1; i < 6; i++)
            {
                if (Leaves.Contains(i)) continue;
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var i1 = i-1;
                IWebElement regularHours =
                    wait.Until(d => d.FindElement(
                        By.Id("billingDetailItems" + i1 + ".customFields0.regularHours")));
                regularHours.Clear();
                regularHours.SendKeys(ReadFromExcel.ReturnValueFromExcel("RegularHours"));
            }
        }


        public static void SelectChargeCode()
        {
            for (var i = 1; i < 6; i++)
            {
                if (Leaves.Contains(i)) continue;
                var temp = i - 1;
                SelectFromDropDown("cf_0_" + temp + "_0_0", "ChargeCode");
            }
        }

        public static void ClickDefaultLink()
        {
            WebDriverWait waitForSubmitButton = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            waitForSubmitButton.Until(
                ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("input[value = 'Submit'")));
            if (ReadFromExcel.ReturnValueFromExcel("IsDefault").Contains("Yes"))
            {
                IList<IWebElement> selectElements = Driver.FindElements(By.LinkText("Default"));
                for (var i = 1; i < selectElements.Count - 1; i++)
                {
                    if (Leaves.Contains(i)) continue;
                    selectElements[i-1].Click();
                    Thread.Sleep(3000);
                }
            }
        }

        public static void NoLunchBreak()
        {
            for (var i = 1; i < 6; i++)
            {
                var temp = i - 1;
                if (Leaves.Contains(i)) continue;
                var ischeckedElement = Driver.FindElement(By.Name("billingDetailItems[" + temp + "].noBreakTaken"));
                if (!ischeckedElement.Selected)
                {
                    ischeckedElement.Click();
                }
            }
        }

        public static void ClickOnFinalSubmit()
        {
            Driver.FindElement(By.CssSelector("input[value = 'Submit'")).Click();
            
        }

        public static void ValidateEnteredHours()
        {
            WebDriverWait waitForFinalMessage = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            waitForFinalMessage.Until(
                ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("body11bold")));
            var totalHoursSubmitted = Driver.FindElement(By.ClassName("body11bold")).Text;
            var resultString = Regex.Match(totalHoursSubmitted, @"\d+").Value;
            var expectedTotalHours = ReadFromExcel.ReturnValueFromExcel("TotalHours");
            if (!resultString.Contains(expectedTotalHours))
            {
                throw new Exception("Your expected hours "+expectedTotalHours+" for this week is not matching submitted total hours"+totalHoursSubmitted+". Kindly re-check and submit again");
            }
        }
    }
}
