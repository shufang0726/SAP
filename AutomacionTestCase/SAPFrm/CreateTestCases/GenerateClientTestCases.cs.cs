using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SAPFrm.CreateTestCases
{
    class GenerateClientTestCases
    {
        private static string path = Environment.CurrentDirectory;
        private static string SaveGeneratedTCFilePath = path.Substring(0, path.Length - 16) + "Automation_Client";
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
            strWriter.WriteLine("using System.Xml;");
            strWriter.WriteLine("using System.Collections.Generic;");
            strWriter.WriteLine("using Automation_Client;");
            strWriter.WriteLine("using UIAF;");
            strWriter.WriteLine("namespace Automation_Client");
            strWriter.WriteLine("{");
            string[] strGroup = fullName.Split('.');
            string className = strGroup[0].Substring(strGroup[0].LastIndexOf('\\') + 1, strGroup[0].Length - strGroup[0].LastIndexOf('\\') - 1);
            strWriter.WriteLine("    public class " + className);
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
                    string strBrowserName = null;
                    #region
                    for (int j = 0; j < xmlNodeSteps.Count; j++)
                    {
                        if (j == 0)
                        {
                            strBrowserName = xmlNodeSteps[0].Attributes["value"].Value;
                            strWriter.WriteLine("                ClientUtility.StartProcess(@" + "\"" + strBrowserName + "\"" + ");");
                        }
                        else
                        {
                            string strStepAction = null;
                            if (xmlNodeSteps[j].Attributes["action"] != null)
                            {
                                strStepAction = xmlNodeSteps[j].Attributes["action"].Value;
                            }
                            string strWaitForTime = null;
                            if (xmlNodeSteps[j].Attributes["waitForTime"] != null)
                            {
                                strWaitForTime = xmlNodeSteps[j].Attributes["waitForTime"].Value;
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
                            string strtype = null;
                            if (xmlNodeSteps[j].Attributes["type"] != null)
                            {
                                strtype = xmlNodeSteps[j].Attributes["type"].Value;
                            }
                            string strwheel = null;
                            if (xmlNodeSteps[j].Attributes["wheel"] != null)
                            {
                                strwheel = xmlNodeSteps[j].Attributes["wheel"].Value;
                            }

                            switch (strStepAction)
                            {
                                case "SendKeysToElement":
                                    {
                                        string strSendValue = xmlNodeSteps[j].Attributes["sendValue"].Value;
                                        strWriter.WriteLine("                Global.Type(" + "\"" + strSendValue + "\"" + ",true);");
                                        strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                        if (strIsScreenShot == "true")
                                        {
                                            strWriter.WriteLine("                UIAFUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                        }
                                        strWriter.WriteLine("");
                                        break;
                                    }
                                case "Click":
                                    {
                                        string strValue = xmlNodeSteps[j].Attributes["value"].Value;
                                        strWriter.WriteLine("                new AccObject(@" + "\"" + strValue + "\"" + ").Click();");
                                        if (strWaitForTime == null)
                                            strWriter.WriteLine("                Thread.Sleep(5000);");
                                        else
                                            strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                        if (strIsScreenShot == "true")
                                        {
                                            strWriter.WriteLine("                UIAFUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                        }
                                        strWriter.WriteLine("");
                                        break;
                                    }
                                case "RightClick":
                                    {
                                        string strValue = xmlNodeSteps[j].Attributes["value"].Value;
                                        strWriter.WriteLine("                new AccObject(@" + "\"" + strValue + "\"" + ").RightClick();");
                                        if (strWaitForTime == null)
                                            strWriter.WriteLine("                Thread.Sleep(5000);");
                                        else
                                            strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                        if (strIsScreenShot == "true")
                                        {
                                            strWriter.WriteLine("                UIAFUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                        }
                                        strWriter.WriteLine("");
                                        break;
                                    }
                                case "DoubleClick":
                                    {
                                        string strValue = xmlNodeSteps[j].Attributes["value"].Value;
                                        strWriter.WriteLine("                new AccObject(@" + "\"" + strValue + "\"" + ").DoubleClick();");
                                        if (strWaitForTime == null)
                                            strWriter.WriteLine("                Thread.Sleep(5000);");
                                        else
                                            strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                        if (strIsScreenShot == "true")
                                        {
                                            strWriter.WriteLine("                UIAFUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                        }
                                        strWriter.WriteLine("");
                                        break;
                                    }
                                case "Global":
                                    {
                                        string strValue = xmlNodeSteps[j].Attributes["value"].Value;
                                        string strTypeValue = xmlNodeSteps[j].Attributes["type"].Value;
                                        switch (strTypeValue)
                                        {
                                            case "type":
                                                strWriter.WriteLine("                Global.Type(" + "\"" + strValue + "\"" + ",200);");
                                                break;
                                            case "click":
                                                strWriter.WriteLine("                Global.Click(" + strValue + ");");
                                                break;
                                            case "moveMouse":
                                                strWriter.WriteLine("                Global.MoveMouse(" + strValue + ");");
                                                break;
                                            case "moveWheel":
                                                if (strwheel.ToUpper().Equals("UP"))
                                                {
                                                    strWriter.WriteLine("                Global.MoveWheel(WheelDirection.Up," + strValue + ");");
                                                }
                                                if (strwheel.ToUpper().Equals("DOWN"))
                                                {
                                                    strWriter.WriteLine("                Global.MoveWheel(WheelDirection.Down," + strValue + ");");
                                                }
                                                
                                                break;
                                            default:
                                                break;
                                        }
                                        if (strWaitForTime == null)
                                            strWriter.WriteLine("                Thread.Sleep(5000);");
                                        else
                                            strWriter.WriteLine("                Thread.Sleep(" + strWaitForTime + ");");
                                        if (strIsScreenShot == "true")
                                        {
                                            strWriter.WriteLine("                UIAFUtility.CaptureWindow(\"" + Guid.NewGuid().ToString() + "_Step" + (j + 1) + "\");");
                                        }
                                        strWriter.WriteLine("");
                                        break;
                                    }
                                default:
                                    break;
                            }

                        }
                    }
                    #endregion
                    //Set driver
                    strWriter.WriteLine("            }");
                    strWriter.WriteLine("            catch");
                    strWriter.WriteLine("            {");
                    strWriter.WriteLine("                throw;");
                    strWriter.WriteLine("            }");
                    strWriter.WriteLine("            finally");
                    strWriter.WriteLine("            {");
                    strWriter.WriteLine("                 ClientUtility.EndProcess(@" + "\"" + strBrowserName + "\"" + ");");
                    strWriter.WriteLine("            }");
                    strWriter.WriteLine("        }");
                }
                strWriter.WriteLine("    }");
                strWriter.WriteLine("}");

                //Clear up
                strWriter.WriteLine("");
            }
            strWriter.Close();
            strWriter.Dispose();
        }
    }
}

