using SAPFrm.CreateTestCases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static SAPFrm.Form1;
using Excel = Microsoft.Office.Interop.Excel;


namespace SAPFrm.Models
{
    public class HLKModel
    {
        public string Pool { get; set; }
        public string IdentityServer { get; set; }
        public string IdentityDatabase { get; set; }
        public string DataStore { get; set; }
        public string JobId { get; set; }


        public void ShowXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load("HLKConfig.xml");
                XmlNode root = xmlDoc.SelectSingleNode("//MachineConfig");
                if (root != null)
                {
                    this.Pool = root.SelectSingleNode("Pool").InnerText;
                    this.IdentityServer = root.SelectSingleNode("IdentityServer").InnerText;
                    this.IdentityDatabase = root.SelectSingleNode("IdentityDatabase").InnerText;
                    this.DataStore = root.SelectSingleNode("DataStore").InnerText;
                    this.JobId = root.SelectSingleNode("JobId").InnerText;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //public void ExcelModify(List<string> jobs_result, List<string> machines_result, List<string> work_result)
        //{
        //    string NowYear = DateTime.Now.Year.ToString();
        //    string NowMouth = DateTime.Now.Month.ToString();
        //    string NowDay = DateTime.Now.Day.ToString();
        //    string NowHour = DateTime.Now.Hour.ToString();
        //    string NowMinutes = DateTime.Now.Minute.ToString();
        //    int num = 0;
        //    string NowTime = NowMouth + NowDay + NowYear + "-" + NowHour + NowMinutes;
        //    string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Results";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string fileName = path + Path.DirectorySeparatorChar + NowTime + ".xlsx";
        //    //creat file
        //    Excel.Application xApp = new Excel.Application();
        //    Excel.Workbook excelDoc = xApp.Workbooks.Add(Type.Missing);
        //    Excel.Worksheet xSheet = excelDoc.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    xApp.DisplayAlerts = false;
        //    Excel.Range MH = xSheet.get_Range("A1", Missing.Value);
        //    MH.Value = "MachineName";
        //    MH.Interior.ColorIndex = 0;
        //    Excel.Range LH = xSheet.get_Range("B1", Missing.Value);
        //    LH.Value = "Language";
        //    LH.Interior.ColorIndex = 0;
        //    //add jobdata to the excel
        //    for (int j = 3; j < jobs_result.Count+3; j++)
        //    {
        //        Excel.Range jobId = (Excel.Range)xSheet.Cells[1, j];
        //        jobId.Value = jobs_result[j-3];
        //        jobId.Interior.ColorIndex = 0;
        //        for (int i = 2; i < machines_result.Count+2; i++)
        //        {
        //            Excel.Range machineName = (Excel.Range)xSheet.Cells[i, 1];
        //            machineName.Value = machines_result[i-2];
        //            machineName.Interior.ColorIndex = 0;

        //            Excel.Range workResult = (Excel.Range)xSheet.Cells[i, j];
        //            if (num < machines_result.Count)
        //            {
        //                num++;
        //            }
        //            workResult.Value = work_result[num];
        //            if (workResult.Value == "Failed")
        //            {
        //                workResult.Interior.ColorIndex = 3;
        //            }
        //            if (workResult.Value == "Passed")
        //            {
        //                workResult.Interior.ColorIndex = 4;
        //            }
        //        }
        //    }

        //    xSheet.SaveAs(fileName);
        //    xSheet = null;
        //    xApp.Quit();
        //    xApp = null;

        //}

        public string ExcelModify(List<result> ResultArray, List<string> jobs_result, List<string> machines_result, string pool)
        {
            HLKController ObjHLK = new HLKController();

            string NowYear = DateTime.Now.Year.ToString();
            string NowMouth = DateTime.Now.Month.ToString();
            string NowDay = DateTime.Now.Day.ToString();
            string NowHour = DateTime.Now.Hour.ToString();
            string NowMinutes = DateTime.Now.Minute.ToString();

            string NowTime = NowMouth + NowDay + NowYear + "-" + NowHour + NowMinutes;
            string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Results";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = path + Path.DirectorySeparatorChar + NowTime + ".xlsx";
            //creat file
            Excel.Application xApp = new Excel.Application();
            Excel.Workbook excelDoc = xApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet xSheet = excelDoc.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xApp.DisplayAlerts = false;
            Excel.Range MH = xSheet.get_Range("A1", Missing.Value);
            MH.Value = "MachineName";
            MH.Interior.ColorIndex = 0;
            Excel.Range LH = xSheet.get_Range("B1", Missing.Value);
            LH.Value = "Language";
            LH.Interior.ColorIndex = 0;
            //add jobdata to the excel
            System.Threading.Thread.Sleep(20000);
            for (int i = 0; i < machines_result.Count; i++)//machine count
            {
                for (int j = i * jobs_result.Count, n = 2; j < (i + 1) * jobs_result.Count; j++, n++)//result array index
                {
                    if (j == i * jobs_result.Count && i == 0)
                    {
                        for (int m = 0; m < jobs_result.Count; m++)
                        {
                            Excel.Range jobName = (Excel.Range)xSheet.Cells[1, m + 3];
                            jobName.Value = ResultArray[m].jobid;
                            jobName.Interior.ColorIndex = 0;
                        }
                    }
                    if (j == i * jobs_result.Count)
                    {
                        Excel.Range MachineName = (Excel.Range)xSheet.Cells[i + 2, 1];
                        string machineName = ResultArray[j].machineName;
                        MachineName.Value = ResultArray[j].machineName;
                        MachineName.Interior.ColorIndex = 0;
                        string getLanguage = ObjHLK.GetMachineLanguage(machineName, pool);
                        Excel.Range language = (Excel.Range)xSheet.Cells[i + 2, 2];
                        language.Value = getLanguage;
                        MachineName.Interior.ColorIndex = 0;
                    }
                    if (i != 0)
                    {
                        Excel.Range result1 = (Excel.Range)xSheet.Cells[i + 2, j + 3 - jobs_result.Count];
                        result1.Value = ResultArray[j].Result == 0 ? "Passed" : "Failed";
                        if (result1.Value == "Passed")
                            result1.Interior.ColorIndex = 4;
                        else
                            result1.Interior.ColorIndex = 3;
                    }
                    else
                    {
                        Excel.Range result1 = (Excel.Range)xSheet.Cells[i + 2, j + 3];
                        result1.Value = ResultArray[j].Result == 0 ? "Passed" : "Failed";
                        if (result1.Value == "Passed")
                            result1.Interior.ColorIndex = 4;
                        else
                            result1.Interior.ColorIndex = 3;
                    }
                }
            }
            xSheet.SaveAs(fileName);
            xSheet = null;
            xApp.Quit();
            xApp = null;
            return fileName;
        }

        public void CreateCSFile(string pool,List<string> machineList)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(fileDialog.FileName);
                string[] str = new string[] { ".xml" };
                if (!((IList)str).Contains(extension))
                {
                    MessageBox.Show("Only upload XML files！");
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileDialog.FileName);
                    switch (pool)
                    {
                        case "Web":
                            #region  create Selenium cs file
                            
                            GenerateWebTestCases generateTestCases = new GenerateWebTestCases();
                            generateTestCases.GenerateCSDocFrame(xmlDoc);
                            generateTestCases.GenerateTC(xmlDoc);
                            this.GenWebCsproj(pool);
                            //Start Build Solution
                            string ExecuteWebPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Execute\\bin\\Debug\\Execute.exe";
                            Process.Start(ExecuteWebPath);
                             //path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Selenium\\bin\\Debug";
                            #endregion
                            break;
                        case "Client":
                            #region create UIAF cs file
                           GenerateClientTestCases gct = new GenerateClientTestCases();
                            gct.GenerateCSDocFrame(xmlDoc);
                            gct.GenerateTC(xmlDoc);
                            this.GenWebCsproj(pool); 
                            //Start Build Solution
                            string ExecuteClientPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Execute_Client\\bin\\Debug\\Execute_Client.exe";
                            Process.Start(ExecuteClientPath);
                             //path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Client\\bin\\Debug";
                            #endregion
                            break;
                        case "Android":
                            #region create UIAF cs file
                            GenerateAndroidTestCases gctAndroid = new GenerateAndroidTestCases();
                            gctAndroid.GenerateCSDocFrame(xmlDoc);
                            gctAndroid.GenerateTC(xmlDoc);
                            this.GenWebCsproj(pool);
                            //Start Build Solution
                            string ExecuteAndroidPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Execute_Android\\bin\\Debug\\Execute_Android.exe";
                            Process.Start(ExecuteAndroidPath);
                            //string ExecuteAndroidPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Android//Automation_Android.csproj";
                            //Execute.Program.ExecuteFile(ExecuteAndroidPath);
                            //path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Client\\bin\\Debug";
                            #endregion
                            break;
                         
                        case "IOS":
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void GenWebCsproj(string type)
        {
            string projPath = Environment.CurrentDirectory;
            string CsprojFile = null;
            string CSFile = null;
            if (type.Equals("Web"))
            {
                 CsprojFile = projPath.Substring(0, projPath.Length - 16) + "Automation_Selenium\\Automation_Selenium.csproj";
                 CSFile = projPath.Substring(0, projPath.Length - 16) + "Automation_Selenium";
            }
            else if (type.Equals("Client"))
            {
                CsprojFile = projPath.Substring(0, projPath.Length - 16) + "Automation_Client\\Automation_Client.csproj";
                CSFile = projPath.Substring(0, projPath.Length - 16) + "Automation_Client";
            }
            else if (type.Equals("Android"))
            {
                CsprojFile = projPath.Substring(0, projPath.Length - 16) + "Automation_Android\\Automation_Android.csproj";
                CSFile = projPath.Substring(0, projPath.Length - 16) + "Automation_Android";
            }
            var files = Directory.GetFiles(CSFile, "*.cs");
            List<String> currFiles = new List<String>();

            foreach (var file in files)
            {
                String path = file.ToString();
                StringBuilder sb = new StringBuilder();
                string csPath = path.Substring(path.LastIndexOf(@"\"));
                sb.Append(csPath.Substring(1, csPath.Length - 1));
                currFiles.Add(sb.ToString());
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(CsprojFile);

            XmlNodeList xnl = doc.ChildNodes[0].ChildNodes;
            if (doc.ChildNodes[0].Name.ToLower() != "project")
            {
                xnl = doc.ChildNodes[1].ChildNodes;
            }
            foreach (XmlNode xn in xnl)
            {

                if (xn.ChildNodes.Count > 0 && xn.ChildNodes[0].Name.ToLower() == "compile")
                {
                    foreach (XmlNode cxn in xn.ChildNodes)
                    {
                        if (cxn.Name.ToLower() == "compile")
                        {
                            XmlElement xe = (XmlElement)cxn;
                            String includeFile = xe.GetAttribute("Include");

                            //if (includeFile.StartsWith(@"ProtocolModel\"))
                            //{
                            Console.WriteLine(includeFile);
                            foreach (String item in currFiles)
                            {
                                if (item.Equals(includeFile))
                                {
                                    currFiles.Remove(item);
                                    break;
                                }
                            }
                            //}
                        }
                    }

                    foreach (String item in currFiles)
                    {
                        XmlElement xelKey = doc.CreateElement("Compile", doc.DocumentElement.NamespaceURI);
                        XmlAttribute xelType = doc.CreateAttribute("Include");
                        xelType.InnerText = item;
                        xelKey.SetAttributeNode(xelType);
                        xn.AppendChild(xelKey);
                    }
                }
            }
            doc.Save(CsprojFile);
        }

        public void CopyDllFile(string pool,List<string> machineList)
        {
            
            string srcPath=null;
            if (pool.Equals("Web"))
            {
                srcPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Selenium\\bin\\Debug";
            }
            if (pool.Equals("Client"))
            {
                srcPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Client\\bin\\Debug";
            }
            if (pool.Equals("Android"))
            {
                srcPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Android\\bin\\Debug";
            }

            string[] fileList = Directory.GetFiles(srcPath);
            foreach (var item in machineList)
            {
                //string aimPath = $"\\\\{item}\\uiaf";
                string aimPath = $"\\\\Skype_shgao_01\\uiaf";
                foreach (string file in fileList)
                {                 
                    string currentdir = aimPath + "\\" + file.Substring(file.LastIndexOf("\\") + 1);

                    if (File.Exists(currentdir))
                    {
                        File.Delete(currentdir);
                        File.Copy(file, currentdir);
                    }
                    else
                    {
                        File.Copy(file, currentdir);
                    }

                }
            }
        }
    }
}

