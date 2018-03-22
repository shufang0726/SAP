using System;
using System.Threading;
using System.Xml;
using System.Collections.Generic;
using Automation_Client;
using UIAF;
namespace Automation_Client
{
    public class AutoTestCase
    {

        [Description("qq_install")]
        [RunFlags((Object)new string[] {"FULL"}, new string[] {" GA" })]
        public static void qq_install()
        {
            try
            {
                ClientUtility.StartProcess(@"C:\Users\v-shgao\Desktop\QQIntl2.11-1.91.1370.0.exe");
                Global.Type("{Enter}",200);
                Thread.Sleep(5000);
                UIAFUtility.CaptureWindow("7b88b4f2-fa46-4d90-a150-7026835a96b5_Step2");

                new AccObject(@";ClassName = '#32770';Role = 'dialog';Role = 'check box' ; ClassName = 'Button' ").Click();
                Thread.Sleep(5000);

                Global.Type("{Enter}",200);
                Thread.Sleep(5000);
                UIAFUtility.CaptureWindow("d76ce0a3-4b92-4e33-890c-2a9524a334b9_Step4");

                Global.Type("{Enter}",200);
                Thread.Sleep(5000);
                UIAFUtility.CaptureWindow("d6b7c5ac-4a43-41f1-9042-d5f33c65ec20_Step5");

                Global.Type("{Enter}",200);
                Thread.Sleep(5000);
                UIAFUtility.CaptureWindow("ac6311b8-4960-4b95-aba1-0194170a0c86_Step6");

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

