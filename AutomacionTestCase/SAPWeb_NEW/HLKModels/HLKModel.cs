
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SAPWeb_NEW.DBConnect;
using SAPWeb_NEW.Models;
using System.Web.Http;
using System.Web.Mvc;

namespace SAPWeb.HLKModels
{
    public enum type
    {
        Client,
        Web,
        Android,
        IOS
    }


    public class HLKModel 

    {
        public string Pool { get; set; }
        public string IdentityServer { get; set; }
        public string IdentityDatabase { get; set; }
        public string DataStore { get; set; }
        public string JobId { get; set; }
     

        public List<result> GetResultFromDB(string[] Id)
        {
            using (HLKJobsEntities db = new HLKJobsEntities())
            {
                List<result> Result = new List<result>();

                for (int i = 0; i < Id.Length - 1; i++)
                {
                    int id = int.Parse(Id[i]);
                    var guId = (
                        from s in db.Schedules
                        join r in db.Runs on s.Id equals r.ScheduleId
                        join re in db.Results on r.Id equals re.RunId
                        join rc in db.ResourceConfigurations on re.ResourceConfigurationId equals rc.Id
                        where s.Id == id
                        select new
                        {
                            res = re.Fail,
                            jobid = re.JobId,
                            maName = rc.Name
                        });
                    var result1 = guId.ToList();
                    foreach (var item in result1)
                    {
                        result result = new result();
                        result.jobid = item.jobid;
                        result.machineName = item.maName;
                        result.Result = item.res;
                        Result.Add(result);
                    }

                }
                return Result;
            }
        }

        public string GetMachineId(string machineName)
        {
            using (HLKJobsEntities db = new HLKJobsEntities())
            {
                var guId = (
                    from j in db.Resources
                    where j.Name == machineName
                    select new
                    {
                        Guid = j.Guid
                    });
                var result1 = guId.ToList();

                return result1[0].Guid.ToString();
            }
        }

        public void ShowXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load("hlkservermanageConfig.xml");
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
    }
}