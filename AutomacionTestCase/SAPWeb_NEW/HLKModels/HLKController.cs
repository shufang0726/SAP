using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.DistributedAutomation;
using Microsoft.DistributedAutomation.Jobs;
using Microsoft.DistributedAutomation.Asset;
using Microsoft.DistributedAutomation.ComponentHierarchy;
using System.Web.Http;
using System.Collections;
using SAPWeb_NEW.Models;
using System.Configuration;
using SAPWeb.HLKModels;
using System.IO;

namespace SAPWeb
{
    [RoutePrefix("HLK")]
    public class HLKController : ApiController
    {
        private readonly string serverName= ConfigurationManager.AppSettings["serverName"];
        private readonly string dataBaseName = ConfigurationManager.AppSettings["dataBaseName"];
        private readonly string dataStoreName = ConfigurationManager.AppSettings["dataStoreName"];
        HLKModel objHlk = new HLKModel();
        HLKModel hl = new HLKModel();
        private DataStore dataStore = null;
        private SqlIdentityConnectInfo sqlIdentityConnectInfo = null;

        private readonly string filePath = ConfigurationManager.AppSettings["filePath"];

        public HLKController()
        {
            sqlIdentityConnectInfo = new SqlIdentityConnectInfo(serverName, dataBaseName);
            dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo);
        }
        /// <summary>
        /// connect hlk 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ConnectHLK/{pool}")]
        public List<Machine> ConnectHLK(string pool)
        {
            if (pool == "Client") pool = @"$\Auto_Client";
            else if (pool == "Web") pool = @"$\Auto_Web";
            else if (pool == "Android") pool = @"$\Auto_Android";
            else if (pool == "IOS") pool = @"$\Auto_IOS";
            else return null;
            List<Machine> objMachine = new List<Machine>();
            using (DataStore dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
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
                        objMachine.Add(new Machine {
                            MachineName=machine.Name,
                            Status=machine.ResourceStatusId.ToString(),
                            Language= GetMachineLanguage(machine.Name, pool)
                        });
                    }
                }
            }
            return objMachine;
        }

        [HttpGet]
        [Route("GetJobID/{pool}")]
        public List<job> GetJobID(string pool)
        {
            string Pool = "Auto_" + pool;
            using (SAPWeb_NEW.DBConnect.HLKJobsEntities db = new SAPWeb_NEW.DBConnect.HLKJobsEntities())
            {
                List<job> Result = new List<job>();
                var result = (
                    from j in db.Jobs
                    join f in db.Features on j.FeatureId equals f.Id
                    where f.Name == Pool
                    select new
                    {
                        jid = j.Id,
                        jname = j.Name,
                        jfeatureid = j.FeatureId,
                        fpath = f.Path,
                        fname = f.Name
                    });
                var result1 = result.ToList();
                foreach (var item in result1)
                {
                    job job1 = new job();
                    job1.jid = item.jid;
                    job1.jname = item.jname;
                    job1.jfeatureid = item.jfeatureid;
                    job1.fpath = item.fpath;
                    job1.fname = item.fname;
                    Result.Add(job1);
                }
                return Result;
            }
        }

        [HttpPost]
        [Route("CopyDllFile")]
        public IHttpActionResult CopyDllFile([FromBody]XMLFileModel model)
        {
            string pool = model.pool;
            string srcPath = null;
            if (pool.Equals("Web"))
            {
                srcPath = filePath + "\\bin\\Debug";
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
            foreach (var item in model.checkedMachinceList)
            {
                string aimPath = $"\\\\{item}\\uiaf";
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
            return Ok("Copy dll file success!");
        }


        [HttpPost]
        [Route("RunJob")]
        public IHttpActionResult RunJob([FromBody]XMLFileModel model)
        {
            string Pool = model.pool;
            //objHlk.ShowXmlValue();
            if (Pool == "Client") Pool = @"$\Auto_Client";
            else if (Pool == "Web") Pool = @"$\Auto_Web";
            else if (Pool == "Android") Pool = @"$\Auto_Android";
            else if (Pool == "IOS") Pool = @"$\Auto_IOS";
            else return Ok("Pool Error");
            string result = null;
            using (DataStore dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, Pool);
                ResourceCollection machines = (ResourceCollection)dataStore.Query(query);//all machines

                for (int i = 0; i < machines.Count; i++)
                {
                    Resource machine = machines[i];
                    if (model.machines_result.Contains(machine.Name))
                    {
                        for (int j = 0; j < model.jobs_result.Count; j++)
                        {
                            query = new Query(typeof(ResourcePool));
                            var db = machine.Name;
                            query.AddExpression("Id", QueryOperator.Equals, machine.ResourcePoolId);

                            ResourcePoolCollection pools = (ResourcePoolCollection)dataStore.Query(query);
                            ResourcePool pool = pools[0];

                            //int id = int.Parse(objHlk.JobId);//job id
                            int id = int.Parse(model.jobs_result[j].Split('-')[0]);

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
                                return Ok("machine:" + machine.Name + "job" + id + "*");
                            }
                        }
                    }
                    else continue;
                }
                return Ok(result);
            }
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
            using (DataStore dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
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
            using (DataStore dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
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

        public string GetMachineLanguage(string machineName, string pool)
        {
            string lanuage = string.Empty;
            using (DataStore dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals,pool);
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

        public Microsoft.DistributedAutomation.Jobs.Job ReviewJobId()
        {
            Query query = new Query(typeof(Microsoft.DistributedAutomation.Jobs.Job));
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
            DataStore dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo);
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


    }
}