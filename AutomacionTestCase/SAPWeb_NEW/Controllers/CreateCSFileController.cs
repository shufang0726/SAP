using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using CreateTestCases;

using System.Text;
using SAPWeb_NEW.Models;
using System.Web;
using System.IO;
using System.Configuration;

namespace CreateCSFileController.Controller
{
    [RoutePrefix("HLKFile")]
    public class CreateCSFileController : ApiController
    {
        private readonly string filePath = ConfigurationManager.AppSettings["filePath"];

        [HttpPost]
        [Route("CreateCSFile")]
        public IHttpActionResult CreateCSFile([FromBody]XMLFileModel model)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(model.filePath);
            switch (model.pool)
            {
                case "Web":
                    #region  create Selenium cs file

                    GenerateWebTestCases generateTestCases = new GenerateWebTestCases();
                    generateTestCases.GenerateCSDocFrame(xmlDoc);
                    generateTestCases.GenerateTC(xmlDoc);
                    this.GenWebCsproj(model.pool);
                    //Start Build Solution
                    string ExecuteWebPath = filePath + "\\Automation_Seleniumt.csproj";
                    Execute.Program.ExecuteFile(ExecuteWebPath);
                    //path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Selenium\\bin\\Debug";
                    #endregion
                    break;
                case "Client":
                    #region create UIAF cs file
                    GenerateClientTestCases gct = new GenerateClientTestCases();
                    gct.GenerateCSDocFrame(xmlDoc);
                    gct.GenerateTC(xmlDoc);
                    this.GenWebCsproj(model.pool);
                    //Start Build Solution
                    string ExecuteClientPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Client//Automation_Client.csproj";
                    Execute.Program.ExecuteFile(ExecuteClientPath);
                    // Process.Start(ExecuteClientPath);
                    //path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Client\\bin\\Debug";
                    #endregion
                    break;
                case "Android":
                    #region create UIAF cs file
                    GenerateAndroidTestCases gctAndroid = new GenerateAndroidTestCases();
                    gctAndroid.GenerateCSDocFrame(xmlDoc);
                    gctAndroid.GenerateTC(xmlDoc);
                    this.GenWebCsproj(model.pool);
                    //Start Build Solution
                    string ExecuteAndroidPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Android//Automation_Android.csproj";
                    Execute.Program.ExecuteFile(ExecuteAndroidPath);
                    //path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Client\\bin\\Debug";
                    #endregion
                    break;
                case "IOS":
                    break;
                default:
                    break;
            }

            return Ok("Upload XML success!");
        }

        public void GenWebCsproj(string type)
        {
            string projPath = Environment.CurrentDirectory;
            string CsprojFile = null;
            string CSFile = null;
            if (type.Equals("Web"))
            {
                CsprojFile = filePath + "\\Automation_Selenium.csproj";
                CSFile = filePath;
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
    }

}
