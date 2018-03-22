using System;
using System.Threading;
using OpenQA.Selenium;
using System.Xml;
using System.Collections.Generic;
using Automation_Selenium.Utility;
using UIAF;
namespace Automation_Selenium
{
    public class AutoTestCase
    {

        [Description("DownloadQQ")]
        [RunFlags((Object)new string[] {"full"}, new string[] {" GA" })]
        public static void DownloadQQ()
        {
            try
            {
                WebDriverUtility.InitializeWebDriver(BrowserType.Edge);
                WebDriverUtility.CurrentWebDriver.Navigate().GoToUrl("https://baidu.com/");
                WebDriverUtility.CurrentWebDriver.Manage().Window.Maximize();
                Thread.Sleep(15000);

                WebDriverUtility.SetElementValue(WebDriverUtility.GetElementByXPath("//input[@id='kw']"),"QQ");
                Thread.Sleep(5000);
                WebDriverUtility.CaptureWindow("c1170c34-34e7-4c0f-94ab-90cd2d0bb3b4_Step3");

                WebDriverUtility.Click(WebDriverUtility.GetElementByXPath("//input[@id='su']"));
                Thread.Sleep(5000);
                WebDriverUtility.CaptureWindow("4fbf87b2-e13a-4669-a076-b92ae1eb8b71_Step4");

                WebDriverUtility.Click(WebDriverUtility.GetElementByXPath("//div[@id='2']/div[2]/div[1]/div[1]/div[2]/p[6]/a"));

                Thread.Sleep(5000);
                WebDriverUtility.CaptureWindow("d7520273-a47d-4709-bccc-0af94d14f890_Step5");

            }
            catch
            {
                throw;
            }
            finally
            {
                WebDriverUtility.Cleanup();
            }
        }
    }
}

