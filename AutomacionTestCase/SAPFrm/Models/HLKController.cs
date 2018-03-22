using Microsoft.DistributedAutomation;
using Microsoft.DistributedAutomation.Asset;
using Microsoft.DistributedAutomation.ComponentHierarchy;
using Microsoft.DistributedAutomation.Jobs;
using SAPFrm.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;
using System.Configuration;

namespace SAPFrm.Models
{

    public class HLKController
    {
        //private const string ServerName = "HCKSERVER.fareast.corp.microsoft.com";
        //private const string DataBaseName = "WTTIdentity";
        //private const string DataStoreName = "HCKSERVER";
        //private const string ServerName = "HLKSERVER.fareast.corp.microsoft.com";
        //private const string DataBaseName = "WTTIdentity";
        //private const string DataStoreName = "HLKSERVER";
        private readonly string ServerName = ConfigurationManager.AppSettings["ServerName"];
        private readonly string DataBaseName = ConfigurationManager.AppSettings["DataBaseName"];
        private readonly string DataStoreName = ConfigurationManager.AppSettings["DataStoreName"];
        HLKModel objHlk = new HLKModel();
        DBHelperJob DBJB = new DBHelperJob();
        HLKModel hl = new HLKModel();
        string[] Results = null;
        private DataStore dataStore = null;
        private SqlIdentityConnectInfo sqlIdentityConnectInfo = null;

        public HLKController()
        {
            sqlIdentityConnectInfo = new SqlIdentityConnectInfo(ServerName, DataBaseName);
            dataStore = Enterprise.Connect(DataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo);
        }
        /// <summary>
        /// connect hlk 
        /// </summary>
        /// <returns></returns>
        public List<string> ConnectHLK(string pool)
        {
            if (pool == "Client") pool = @"$\Auto_Client";
            else if (pool == "Web") pool = @"$\Auto_Web";
            else if (pool == "Android") pool = @"$\Auto_Android";
            else if (pool == "IOS") pool = @"$\Auto_IOS";
            else return null;
            //objHlk.ShowXmlValue();
            List<string> machineName = new List<string>();
            using (DataStore dataStore = Enterprise.Connect(DataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, pool);

                ResourceCollection machineCollection = (ResourceCollection)dataStore.Query(query);
                if (machineCollection.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (Resource machine in machineCollection)
                    {
                        machineName.Add(machine.Name);
                    }
                }
            }
            return machineName;
        }

        /// <summary>
        /// delete run job result
        /// </summary>
        public void DeleteRunJobResult()
        {

        }

        /// <summary>
        /// move machine pool
        /// </summary>
        public void MoveMachine()
        {
            using (DataStore dataStore = Enterprise.Connect(DataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, @"$\Default Pool");

                ResourceCollection machineCollection = (ResourceCollection)dataStore.Query(query);
                if (machineCollection.Count == 0)
                    return;
                Resource machine = machineCollection[0];

                query = new Query(typeof(ResourcePool));
                query.AddExpression("FullPath", QueryOperator.Equals, @"$\Automation_VM");
                ResourcePoolCollection pools = (ResourcePoolCollection)dataStore.Query(query);
                if (pools.Count == 0)
                    return;
                ResourcePool pool = pools[0];

                machine.ResourcePoolId = pool.Id;
                machine.DataStoreOperation = DataStoreOperation.Update;
                machine.CommitToDataStore();
            }

        }

        /// <summary>
        /// reset vm status
        /// </summary>
        public void ResetMachineStatus()
        {
            using (DataStore dataStore = Enterprise.Connect(DataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, @"$\Automation_VM");
                ResourceCollection machineCollection = (ResourceCollection)dataStore.Query(query);

                if (machineCollection.Count != 0)
                {
                    foreach (Resource r in machineCollection)
                    {
                        if (r.ResourceStatusId == ResourceStatus.Manual && DateTime.Now.Subtract(r.LastHBTime).CompareTo(new TimeSpan(0, 30, 0)) < 0)
                        {
                            r.ResourceStatusId = ResourceStatus.Reset;
                        }
                        r.DataStoreOperation = DataStoreOperation.Update;
                        try
                        {
                            r.CommitToDataStore();
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// run job
        /// </summary>
        public string RunJob(List<string> jobs_result, List<string> machines_result, string Pool)
        {
            //objHlk.ShowXmlValue();
            if (Pool == "Client") Pool = @"$\Auto_Client";
            else if (Pool == "Web") Pool = @"$\Auto_Web";
            else if (Pool == "Android") Pool = @"$\Auto_Android";
            else if (Pool == "IOS") Pool = @"$\Auto_IOS";
            else return "Pool error";
            string result = null;
            using (DataStore dataStore = Enterprise.Connect(DataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, Pool);
                ResourceCollection machines = (ResourceCollection)dataStore.Query(query);//all machines

                for (int i = 0; i < machines.Count; i++)
                {
                    Resource machine = machines[i];
                    if (machines_result.Contains(machine.Name))
                    {
                        for (int j = 0; j < jobs_result.Count; j++)
                        {
                            query = new Query(typeof(ResourcePool));
                            var db = machine.Name;
                            query.AddExpression("Id", QueryOperator.Equals, machine.ResourcePoolId);

                            ResourcePoolCollection pools = (ResourcePoolCollection)dataStore.Query(query);
                            ResourcePool pool = pools[0];

                            //int id = int.Parse(objHlk.JobId);//job id
                            int id = int.Parse(jobs_result[j].Split('-')[0]);

                            query = new Query(typeof(Job));

                            query.AddExpression("Id", QueryOperator.Equals, id);

                            JobCollection jobCollection = (JobCollection)dataStore.Query(query);
                            Job job = jobCollection[0];
                            ArrayList myParamsList = new ArrayList();
                            ScheduleHelper sh = new ScheduleHelper();
                            ScheduleOptions so = new ScheduleOptions();
                            sh.ScheduleOptions = so;
                            sh.ResourceCollection.Add(machine);
                            sh.ResourcePool = pool;
                            sh.JobCollection.Add(job);
                            sh.JobParameterCollection.Add(job.Id, myParamsList);
                            sh.SourceDataStore = dataStore;
                            sh.DestinationDataStore = dataStore;
                            try
                            {
                                ScheduleOutput output = sh.ScheduleJobs();
                                int shc = output.ScheduleId;
                                result += shc.ToString() + "~";
                            }
                            catch
                            {
                                return "machine:" + machine.Name + "job" + id + "*";
                            }
                        }
                    }
                    else continue;
                }
                return result;
            }
        }

        public void GetReadyMachinesStatus(string pool)
        {
            List<string> machines = this.ConnectHLK(pool);
            if (machines != null && machines.Count > 0)
            {
                // All ready machines are in one data store

                foreach (string machine in machines)
                {
                    DataStore dataStore = Enterprise.Connect(DataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo);
                    Query query = new Query(typeof(Resource));
                    query.AddExpression("FullPath", QueryOperator.Equals, @"$\Automation_VM");
                    ResourceCollection resources = (ResourceCollection)dataStore.Query(query);
                    if (resources != null && resources.Count > 0)
                    {
                        Resource resource = resources[0];

                        if (resource.LatestResourceConfiguration != null)
                        {
                            foreach (ResourceConfigurationValue v in resource.LatestResourceConfiguration.ResourceConfigurationValueCollection)
                            {
                                Console.WriteLine(v.ResourceConfigurationVal);
                            }
                            Console.ReadLine();
                            foreach (ResourceConfigurationValue v in resource.LatestResourceConfiguration.ResourceConfigurationValueCollection)
                            {
                                switch (v.DimensionId)
                                {
                                    case 32:
                                        string HLKLabString = v.ResourceConfigurationVal;
                                        break;
                                    case 24:
                                        string HLKLanguage = v.ResourceConfigurationVal;
                                        break;
                                    case 42:
                                        string HLKOSEdition = v.ResourceConfigurationVal;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                    }
                }
            }
        }

        public string GetMachineLanguage(string machineName, string pool)
        {
            string lanuage = string.Empty;
            SqlIdentityConnectInfo sqlIdentityConnectInfo = new SqlIdentityConnectInfo("SAPSERVER.fareast.corp.microsoft.com", "WTTIdentity");
            using (DataStore dataStore = Enterprise.Connect("SAPSERVER", JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, @"$\Auto_" + pool);
                ResourceCollection resources = (ResourceCollection)dataStore.Query(query);
                if (resources != null && resources.Count > 0)
                {
                    for (int i = 0; i < resources.Count; i++)
                    {
                        Resource resource = resources[i];
                        for (int j = 0; j < machineName.Length; j++)
                        {
                            if (resource.Name == machineName)
                            {
                                if (resource.LatestResourceConfiguration != null)
                                {
                                    foreach (ResourceConfigurationValue v in resource.LatestResourceConfiguration.ResourceConfigurationValueCollection)
                                    {
                                        if (v.DimensionId == 24)
                                        {
                                            lanuage = v.ResourceConfigurationVal;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return lanuage;
        }

        public Job ReviewJobId()
        {
            Query query = new Query(typeof(Job));
            int jobId = 4049;
            query.AddExpression("Id", QueryOperator.Equals, jobId);
            JobCollection jobs = (JobCollection)dataStore.Query(query);
            if (jobs.Count > 0)
            {
                return jobs[0];
            }
            return null;
        }

        public void CreateJobId()
        {
            DataStore dataStore = Enterprise.Connect(DataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo);
            Query query = new Query(typeof(Job));
            query.AddExpression("Id", QueryOperator.Equals, "4061");
            Job exampleJob = ((JobCollection)dataStore.Query(query))[0];
            Job job = exampleJob.Clone(dataStore);
            string ranNums = Convert.ToString((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000);
            job.Name = "AutoJob_" + ranNums.Substring(7, 5);
            job.Description = "";
            job.JobExecutionTypeId = JobExecutionType.Library;
            string fullPath = @"$\AutomationJobs";
            job.FeatureId = GetFeatureId(fullPath);
            job.DescriptionFormatId = TextFormat.Text;
            ParameterCollection parameterCollection = job.CommonContext.ParameterCollection;
            string userName = Environment.UserName;
            job.CreatedByAlias = userName;
            job.AssignedToAlias = userName;
            job.OwnerAlias = userName;
            job.LastUpdatedByAlias = userName;
            job.ReviewedBy = userName;
            job.Priority = 1;
            job.DataStoreOperation = DataStoreOperation.Insert;
            job.CommitToDataStore();
        }

        private int GetFeatureId(string fullPath)
        {
            Query query = new Query(typeof(Feature));
            query.AddExpression("FullPath", QueryOperator.Equals, fullPath);
            return ((FeatureCollection)dataStore.Query(query))[0].Id;
        }


        //public string GetResult(string machineName)
        //{
        //    string results = null;
        //    string machineID = null;
        //    XmlDocument doc = new XmlDocument();
        //    machineID = DBJB.GetMachineId(machineName);
        //    string folder;
        //    string folderPath =@"\\"+ DataStoreName+ @"\HLKLogs\Gatherers\" + machineID + "";
        //    DirectoryInfo di = new DirectoryInfo(folderPath);
        //    FileInfo[] arrFi = di.GetFiles("*.*");

        //    Array.Sort(arrFi, delegate (FileInfo x, FileInfo y)
        //    {
        //        return y.LastWriteTime.CompareTo(x.LastWriteTime);

        //    });
        //    folder = arrFi[4].Name;
        //    string[] strArry = folder.Split('-');
        //    //Console.Write(strArry[1]);
        //    string dd = strArry[1].Substring(strArry[1].Length - 2, 2);


        //    //get Results from xml
        //    int NowYear = DateTime.Now.Year;
        //    int NowMouth = DateTime.Now.Month;

        //    string NowTime = NowMouth + "-" +dd+ "-" + NowYear;
        //    string filePath = @"\\"+DataStoreName + @"\HLKLogs\" +NowTime+"\\";

        //    DirectoryInfo di2 = new DirectoryInfo(filePath);
        //    DirectoryInfo[] arrFi2 = di2.GetDirectories();

        //    Array.Sort(arrFi2, delegate (DirectoryInfo x, DirectoryInfo y)
        //    {
        //        return y.LastWriteTime.CompareTo(x.LastWriteTime);

        //    });
        //    filePath += arrFi2[0].Name + Path.DirectorySeparatorChar + machineID + Path.DirectorySeparatorChar + "WTTJQResults.xml";
        //    if (filePath != null)
        //    {
        //        doc.Load(filePath);
        //        XmlNode root = doc.SelectSingleNode("//WttCommMessage");
        //        XmlNodeList RunResults = root.SelectNodes("RunResult");
        //        foreach (XmlNode RunResult in RunResults)
        //        {
        //            string status = RunResult.Attributes["Status"].Value;
        //            if (status == "NotComplete")
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                results = status;
        //            }
        //        }

        //    }
        //    return results;

        //}

        public string GetXMLPath(string pool)
        {
            string originalPath = string.Empty;
            string path = string.Empty;
            string listpath = null;      
            if (pool.Equals("Web"))
            {
                originalPath = @"\\hlkserver\ShareFlie\Web_case.xml";
                path = @"C:\Web_case.xml";
            }
            if (pool.Equals("Client"))
            {
                originalPath = @"\\hlkserver\ShareFlie\Client_case.xml";
                path = @"C:\Client_case.xml";
            }
            if (pool.Equals("Android"))
            {
                originalPath = @"\\hlkserver\ShareFlie\Android_case.xml";
                path = @"C:\Android_case.xml";
            }
            listpath= originalPath +","+ path;
            return listpath;
        }
    }
}

