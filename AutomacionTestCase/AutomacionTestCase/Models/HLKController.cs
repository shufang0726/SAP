using Microsoft.DistributedAutomation;
using Microsoft.DistributedAutomation.Asset;
using Microsoft.DistributedAutomation.ComponentHierarchy;
using Microsoft.DistributedAutomation.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutomacionTestCase.Models
{

    public class HLKController
    {
        private const string ServerName = "HCKSERVER.fareast.corp.microsoft.com";
        private const string DataBaseName = "WTTIdentity";
        private const string DataStoreName = "HCKSERVER";
        HLKModel objHlk = new HLKModel();
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
        public List<string> ConnectHLK()
        {
            objHlk.ShowXmlValue();
            List<string> machineName = new List<string>();
            using (DataStore dataStore = Enterprise.Connect(objHlk.DataStore, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, objHlk.Pool);

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
            using (DataStore dataStore = Enterprise.Connect(objHlk.DataStore, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, @"$\Default Pool");

                ResourceCollection machineCollection = (ResourceCollection)dataStore.Query(query);
                if (machineCollection.Count == 0)
                    return;
                Resource machine = machineCollection[0];

                query = new Query(typeof(ResourcePool));
                query.AddExpression("FullPath", QueryOperator.Equals, objHlk.Pool);
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
            using (DataStore dataStore = Enterprise.Connect(objHlk.DataStore, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, objHlk.Pool);
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
        public void RunJob()
        {
            //objHlk.ShowXmlValue();
            using (DataStore dataStore = Enterprise.Connect(objHlk.DataStore, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo))
            {
                Query query = new Query(typeof(Resource));
                query.AddExpression("FullPath", QueryOperator.Equals, objHlk.Pool);
                ResourceCollection machines = (ResourceCollection)dataStore.Query(query);
                for (int i = 0; i < machines.Count; i++)
                {
                    Resource machine = machines[i];
                    query = new Query(typeof(ResourcePool));
                    var db = machine.Name;
                    query.AddExpression("Id", QueryOperator.Equals, machine.ResourcePoolId);
                    ResourcePoolCollection pools = (ResourcePoolCollection)dataStore.Query(query);
                    ResourcePool pool = pools[0];
                    int id = int.Parse(objHlk.JobId);
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
                    ScheduleOutput output = sh.ScheduleJobs();
                }
            }
        }

        public void GetReadyMachinesStatus()
        {
            List<string> machines = this.ConnectHLK();
            if (machines != null && machines.Count > 0)
            {
                // All ready machines are in one data store

                foreach (string machine in machines)
                {
                    DataStore dataStore = Enterprise.Connect(objHlk.DataStore, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo);
                    Query query = new Query(typeof(Resource));
                    query.AddExpression("FullPath", QueryOperator.Equals, objHlk.Pool);
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
            DataStore dataStore = Enterprise.Connect(objHlk.DataStore, JobsRuntimeDataStore.ServiceName, sqlIdentityConnectInfo);
            Query query = new Query(typeof(Job));
            query.AddExpression("Id", QueryOperator.Equals, "4061");
            Job exampleJob = ((JobCollection)dataStore.Query(query))[0];
            Job job = exampleJob.Clone(dataStore);
            string ranNums =Convert.ToString((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000);
            job.Name = "AutoJob_" + ranNums.Substring(7,5);
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

