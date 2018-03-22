using System;
using System.Threading;
using System.Xml;
using System.Collections.Generic;
using Automation_Client;
using UIAF;
using System.Text.RegularExpressions;

namespace Automation_Client
{
    public class AutomationTestCase
    {

        [Description("Client")]
        [RunFlags((Object)new string[] {"FULL"}, new string[] {" GA" })]
        public static void Client()
        {
            try
            {
                ClientUtility.StartProcess(@"C:\Users\v-shgao\Desktop\QQIntl2.11-1.91.1370.0.exe");
                Global.Type("{Enter}");
                Thread.Sleep(5000);
                new AccObject(null, @";ClassName = '#32770';Role = 'dialog';Role = 'check box' ; ClassName = 'Button'", new string[0], "").Click();
                UIAFUtility.CaptureWindow("d7f4c8cf-bac2-4a15-aa5d-2b60ea350393_Step3");
                Thread.Sleep(5000);
                Global.Type("{Enter}");
                Thread.Sleep(5000);
                Global.Type("{Enter}");
                Thread.Sleep(5000);
                Global.Type("{Enter}");                
                Thread.Sleep(5000);             

            }
            catch
            {
                throw;
            }
            finally
            {
                 ClientUtility.EndProcess(@"C:\Users\v-shgao\Desktop\QQIntl2.11-1.91.1370.0.exe");
            }
        }
    }
}

