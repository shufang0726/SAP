﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CreateTestCases
{
    public class GenerateAndroidTestCases
    {
        private static string path = Environment.CurrentDirectory;
        private static string SaveGeneratedTCFilePath = path.Substring(0, path.Length - 16) + "Automation_Android";
        private StreamWriter strWriter;
        XmlNodeList xmlNodes;
        public void GenerateCSDocFrame(XmlDocument xmlDoc)
        {
            string fullName = string.Empty;
            xmlNodes = xmlDoc.SelectNodes("Cases");
            string strClassName = xmlNodes[0].Attributes["name"].Value;
            FileStream fileStream;
            if (!File.Exists(SaveGeneratedTCFilePath))
            {
                fullName = SaveGeneratedTCFilePath + $"\\{strClassName}" + ".cs";
                var newfile = File.Create(fullName);
                newfile.Close();
            }
            else
            {
                string filename = SaveGeneratedTCFilePath.Substring(SaveGeneratedTCFilePath.LastIndexOf('\\') + 1, SaveGeneratedTCFilePath.Length - SaveGeneratedTCFilePath.LastIndexOf('\\') - 1);
                string[] names = filename.Split('.');

                for (int i = 1; i <= 100; i++)
                {
                    fullName = SaveGeneratedTCFilePath.Substring(0, SaveGeneratedTCFilePath.LastIndexOf('\\')) + "\\" + names[0] + i + ".cs";
                    if (!File.Exists(fullName))
                    {
                        var newfile = File.Create(fullName);
                        newfile.Close();
                        break;
                    }
                }
            }

            fileStream = File.Open(fullName, FileMode.Append, FileAccess.Write);

            strWriter = new StreamWriter(fileStream);
            strWriter.WriteLine("using System;");
            strWriter.WriteLine("using System.Threading;");
            strWriter.WriteLine("using OpenQA.Selenium;");
            strWriter.WriteLine("using System.Xml;");
            strWriter.WriteLine("using System.Collections.Generic;");
            strWriter.WriteLine("using Automation_Selenium.Utility;");
            strWriter.WriteLine("using Automation_Android;");
            strWriter.WriteLine("using Automation_Android.Utility;");

            strWriter.WriteLine("using UIAF;");
            strWriter.WriteLine("namespace Automation_Android");
            strWriter.WriteLine("{");
            string[] strGroup = fullName.Split('.');
            string className = strGroup[0].Substring(strGroup[0].LastIndexOf('\\') + 1, strGroup[0].Length - strGroup[0].LastIndexOf('\\') - 1);
            strWriter.WriteLine("    public  class " + className);
            strWriter.WriteLine("    {");
            strWriter.WriteLine();
        }

        public void GenerateTC(XmlDocument xmlDoc)
        {
            XmlNodeList xmlNodes;
            if (xmlDoc.SelectNodes("Cases/Case") != null)
            {
                xmlNodes = xmlDoc.SelectNodes("Cases/Case");
                for (int i = 0; i < xmlNodes.Count; i++)
                {
                    string TestCaseName = xmlNodes[i].Attributes["name"].Value;
                    string RunFlag = xmlNodes[i].Attributes["flag"].Value;
                    strWriter.WriteLine("        [Description(" + '"' + TestCaseName + '"' + ")]");
                    strWriter.WriteLine("        [RunFlags((Object)new string[] {" + '"' + RunFlag + '"' + "}, new string[] {" + '"' + " GA" + '"' + " })]");
                    strWriter.WriteLine("        public static void " + TestCaseName + "()");
                    strWriter.WriteLine("        {");
                    strWriter.WriteLine("            try");
                    strWriter.WriteLine("            {");

                    //Get driver
                    XmlNodeList xmlNodeSteps = xmlNodes[i].SelectNodes("step");
                    string strVersion = null;
                    string strdeviceName = null;
                    string strUri = null;
                    string strBrowser = null;


                    for (int j = 0; j < xmlNodeSteps.Count; j++)
                    {

                        string strStepAction = null;
                        if (xmlNodeSteps[j].Attributes["action"] != null)
                        {
                            strStepAction = xmlNodeSteps[j].Attributes["action"].Value;
                        }
                        string strCssPath = null;
                        if (xmlNodeSteps[j].Attributes["cssPath"] != null)
                        {
                            strCssPath = xmlNodeSteps[j].Attributes["cssPath"].Value;
                        }
                        int strWaitForTime = 1500;
                        if (xmlNodeSteps[j].Attributes["waitForTime"] != null)
                        {
                            strWaitForTime = Convert.ToInt32(xmlNodeSteps[j].Attributes["waitForTime"].Value);
                        }
                        string strIsScreenShot = null;
                        if (xmlNodeSteps[j].Attributes["isScreenShot"] != null)
                        {
                            strIsScreenShot = xmlNodeSteps[j].Attributes["isScreenShot"].Value;
                        }
                        string strLoopTimes = null;
                        if (xmlNodeSteps[j].Attributes["LoopTimes"] != null)
                        {
                            strLoopTimes = xmlNodeSteps[j].Attributes["LoopTimes"].Value;
                        }

                        switch (strStepAction)
                        {
                            case "Navigate":
                                {
                                    string strURLPath = xmlNodeSteps[1].Attributes["value"].Value;
                                    strWriter.WriteLine("                AndroidUtility.CurrentWebDriver.Navigate().GoToUrl(\"" + strURLPath + "\");");
                                    strWriter.WriteLine("                AndroidUtility.CurrentWebDriver.Manage().Window.Maximize();");
                                    strWriter.WriteLine("                Thread.Sleep(1500);");
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "openFile":
                                {
                                    string appPackage = xmlNodeSteps[j].Attributes["package"].Value;
                                    string strActivity = xmlNodeSteps[j].Attributes["activity"].Value;
                                    strWriter.WriteLine("                 AndroidUtility.openInstalApp(\"" + strVersion + "\",\"" + strdeviceName + "\",\"" + strUri + "\",\"" + appPackage + "\",\"" + strActivity + "\");");
                                    strWriter.WriteLine("                Thread.Sleep(15000);");
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;

                                }
                            case "APPClick":
                                {
                                    string strbutton = xmlNodeSteps[j].Attributes["value"].Value;
                                    strWriter.WriteLine("                AndroidUtility.Click(AndroidUtility.GetElementByName(\"" + strbutton + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strLoopTimes != null)
                                    {
                                        strWriter.WriteLine("                AndroidUtility.Click(AndroidUtility.GetElementByName(\"" + strCssPath + "\")," + Convert.ToInt32(strLoopTimes) + ");");
                                        strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    }
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;

                                }
                            case "SendKeysToElement":
                                {
                                    string strSendValue = xmlNodeSteps[j].Attributes["sendValue"].Value;
                                    strWriter.WriteLine("                AndroidUtility.SetElementValue(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"),\"" + strSendValue + "\");");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "ClickElement":
                                {
                                    strWriter.WriteLine("                AndroidUtility.Click(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strLoopTimes != null)
                                    {
                                        strWriter.WriteLine("                AndroidUtility.Click(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\")," + Convert.ToInt32(strLoopTimes) + ");");
                                        strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    }
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "ClickJSElement":
                                {
                                    strWriter.WriteLine("                AndroidUtility.ClickElementByJs(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strLoopTimes != null)
                                    {
                                        strWriter.WriteLine("                AndroidUtility.ClickElementByJs(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\")," + Convert.ToInt32(strLoopTimes) + ");");
                                        strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    }
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "MouseHover":
                                {
                                    strWriter.WriteLine("                AndroidUtility.HoverWebElementByJs(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "MouseFocus":
                                {
                                    strWriter.WriteLine("                AndroidUtility.FocusElement(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "RightClick":
                                {
                                    strWriter.WriteLine("                AndroidUtility.RightClick(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strLoopTimes != null)
                                    {
                                        strWriter.WriteLine("                AndroidUtility.RightClick(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\")," + Convert.ToInt32(strLoopTimes) + ");");
                                        strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    }
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "DoubleClick":
                                {
                                    strWriter.WriteLine("                AndroidUtility.DoubleClick(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strLoopTimes != null)
                                    {
                                        strWriter.WriteLine("                AndroidUtility.DoubleClick(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\")," + Convert.ToInt32(strLoopTimes) + ");");
                                        strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    }
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "MouseScroll":
                                {
                                    strWriter.WriteLine("                AndroidUtility.ScrollIntoViewByJs(AndroidUtility.GetElementByXPath(\"" + strCssPath + "\"));");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            case "WaitForAppear":
                                {
                                    strWriter.WriteLine("                AndroidUtility.WaitForElementLoaded(\"" + strCssPath + "\");");
                                    strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                    if (strIsScreenShot == "true")
                                    {
                                        strWriter.WriteLine("                AndroidUtility.CaptureWindow(\"" + Guid.NewGuid().ToString("N") + "_Step" + (j + 1) + "\");");
                                    }
                                    strWriter.WriteLine("");
                                    break;
                                }
                            default:
                                break;
                        }

                    }
                }
                //Set driver
                strWriter.WriteLine("            }");
                strWriter.WriteLine("            catch");
                strWriter.WriteLine("            {");
                strWriter.WriteLine("                throw;");
                strWriter.WriteLine("            }");
                strWriter.WriteLine("            finally");
                strWriter.WriteLine("            {");
                strWriter.WriteLine("                AndroidUtility.Cleanup();");
                strWriter.WriteLine("            }");
                strWriter.WriteLine("        }");
            }
            strWriter.WriteLine("    }");
            strWriter.WriteLine("}");

            //Clear up
            strWriter.WriteLine("");

            strWriter.Close();
            strWriter.Dispose();
        }

    }
}