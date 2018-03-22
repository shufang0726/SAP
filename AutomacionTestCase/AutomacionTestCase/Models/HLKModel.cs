using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutomacionTestCase.Models
{
    public class HLKModel
    {
        public string Pool { get; set; }
        public string IdentityServer { get; set; }
        public string IdentityDatabase { get; set; }
        public string DataStore { get; set; }
        public string JobId { get; set; }
        public static string MachinID { get; set; }
       


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
        public IList<String> GetTaskResult(string path)
        {
            List<string> Status = new List<string>();
            if (path != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode root = doc.SelectSingleNode("//WttCommMessage");
                XmlNodeList taskResults = root.SelectNodes("TaskResult");
                foreach (XmlNode taskResult in taskResults)
                {
                    string status = taskResult.Attributes["Status"].Value;
                    if (status == "NotComplete")
                    {
                        continue;
                    }
                    else
                    {
                        Status.Add(status);
                        Console.Write(Status[0]+"<br/>");
                    }
                }

            }

            return Status;
        }
        //public String GetJobID(string path)
        //{

        //    if (path != null)
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(path);
        //        XmlNode root = doc.SelectSingleNode("//WttCommMessage");
        //        XmlNodeList jobResults = root.SelectNodes("JobResult");
        //        JobId = jobResults[0].Attributes["JobGUID"].Value;
        //    }
        //    return JobId;
        //}

        
    }
}
