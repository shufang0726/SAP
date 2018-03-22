using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPFrm.DBConnect;
using SAPWeb_NEW.DBConnect;
using static SAPFrm.Form1;

namespace SAPFrm.DAL
{
    public class DBHelperJob
    {
        public List<job> GetJobID(string pool)
        {
            string Pool = "Auto_"+pool;
            using (HLKJobsEntities db = new HLKJobsEntities())
            {
                List<job> Result = new List<job>();
                var result = (
                    from j in db.Jobs
                    join f in db.Features on j.FeatureId equals f.Id
                    where f.Name ==Pool
                    select new
                    {
                        jid = j.Id,
                        jname = j.Name,
                        jfeatureid = j.FeatureId,
                        fpath = f.Path,
                        fname=f.Name
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

        public string GetMachineId(string machineName)
        {
            using (HLKJobsEntities db = new HLKJobsEntities())
            {
                var guId = (
                    from j in db.Resources
                    where j.Name == machineName
                    select new
                    {
                       Guid=j.Guid
                    });
                var result1 = guId.ToList();

                return result1[0].Guid.ToString();
            }
        }
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
    }
}
