using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomacionTestCase.Models
{
  public static class GetDataFromDB
    {
        static SqlConnection conn = new SqlConnection();
        static string machineName { get; set; }
        static string machineID { get; set; }
        static string jobID { get; set; }
        static string rID { get; set; }

        static string connectionString = @"Data Source=10.248.233.35;Initial Catalog=HLKJobs;Integrated Security=true";
        //static string selectGuid = "Select Guid from Result";

        #region GetJobID From Results
        public static string GetJobID(string GUID)
        {
            string selectJob = "Select JobId from Result where RunId='" + GUID + "'";
            conn.ConnectionString = connectionString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = selectJob;
            SqlDataReader jobReader = cmd.ExecuteReader();
            while (jobReader.Read())
            {
                jobID = jobReader[0].ToString();
            }
            jobReader.Close();
            cmd.Dispose();
            conn.Close();
            //Console.Write("JobID:{0}", jobID);
            return jobID;
        }
        #endregion

        #region GetResultId
        /// <summary>
        /// Get RunID from directory and then according to runId, Get ResultId as Guid
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public static string GetResultID(string RunID)
        {
            string selectRunId = "select Id from Run where Guid='" + RunID + "'";
            conn.ConnectionString = connectionString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = selectRunId;
            SqlDataReader RunReader = cmd.ExecuteReader();
            while (RunReader.Read())
            {
                RunID = RunReader[0].ToString();
            }
            RunReader.Close();
            cmd.Dispose();
            conn.Close();
            return RunID;
        }
        #endregion

        #region GetMachineName
        public static String GetMachineName(string GuID)
        {
            string selectConId = "select ResourceConfigurationId from Result where RunId='" + GuID + "'";
            conn.ConnectionString = connectionString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = selectConId;
            SqlDataReader ConReader = cmd.ExecuteReader();
            while (ConReader.Read())
            {
                rID = ConReader[0].ToString();
            }
            ConReader.Close();
            string selectMachine = "Select Name from ResourceConfiguration where id='" + rID + "'";
            cmd.CommandText = selectMachine;
            SqlDataReader machineReader = cmd.ExecuteReader();
            while (machineReader.Read())
            {
                machineName = machineReader[0].ToString();
                //Console.WriteLine(" ResouseID:" + rID + " MachineName" + machineName + "");
            }
            machineReader.Close();

            cmd.Dispose();
            conn.Close();
            return machineName;

        }
        #endregion

        #region GetMachineID
        public static string GetMachineID(string machineName)
        {
            string selectMachineID = "Select Guid from Resource where Name='" + machineName + "'";
            conn.ConnectionString = connectionString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = selectMachineID;
            SqlDataReader MachineIDReader = cmd.ExecuteReader();
            while (MachineIDReader.Read())
            {
                machineID = MachineIDReader[0].ToString();
            }
            MachineIDReader.Close();
            cmd.Dispose();
            conn.Close();
            //Console.Write("JobID:{0}", jobID);
            return machineID;
        }
        #endregion

    }
}
