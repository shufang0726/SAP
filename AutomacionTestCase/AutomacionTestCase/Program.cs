using AutomacionTestCase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AutomacionTestCase
{
    class Program
    {
        static void Main(string[] args)
        {

            //HLKController hlkCollect = new HLKController();
            //hlkCollect.ConnectHLK();
            //hlkCollect.MoveMachine();
            //hlkCollect.ResetMachineStatus();
            //hlkCollect.GetReadyMachinesStatus();
            // hlkCollect.RunJob();

            //string path = @"\\HLKSERVER\HLKLogs";
            HLKModel hlkModel = new HLKModel();
            //hlkModel.GetTaskResult(path + System.IO.Path.DirectorySeparatorChar + @"9-18-2017\8C96608A-DF9C-E711-A93F-02155DC5AF06\7C1C6470-93D5-4642-A2AF-C5BA4EC811C7\WTTJQResults.xml");
            // hlkModel.GetJobID(path + System.IO.Path.DirectorySeparatorChar + @"9-18-2017\8C96608A-DF9C-E711-A93F-02155DC5AF06\7C1C6470-93D5-4642-A2AF-C5BA4EC811C7\WTTJQResults.xml");

            string jobID, machineName, machinID, Guid, RunId;
            string NowYear = DateTime.Now.ToString("yyyy");
            string NowMouth = DateTime.Now.ToString("MM");
            string NowDay = DateTime.Now.ToString("dd");
            string NowTime = NowMouth + "-" + NowDay + "-" + NowYear;
            //Console.Write(NowTime);

            string filePath = @"\\hlkserver\HLKLogs\9-19-2017";

            //Sort Directory by LastWriteTime
            DirectoryInfo di = new DirectoryInfo(filePath);
            DirectoryInfo[] arrFi = di.GetDirectories();

            Array.Sort(arrFi, delegate (DirectoryInfo x, DirectoryInfo y)
            {
                return y.LastWriteTime.CompareTo(x.LastWriteTime);

            });
            RunId = arrFi[0].Name;

            Guid = GetDataFromDB.GetResultID(RunId);


            machineName = GetDataFromDB.GetMachineName(Guid);
            jobID = GetDataFromDB.GetJobID(Guid);
            machinID = GetDataFromDB.GetMachineID(machineName);
            Console.WriteLine("JobId: {0}",jobID);
            Console.WriteLine("machineID: {0}", machinID);
            Console.WriteLine("machineName: {0}",machineName);
            hlkModel.GetTaskResult(filePath + Path.DirectorySeparatorChar + RunId + Path.DirectorySeparatorChar + machinID + Path.DirectorySeparatorChar + "WTTJQResults.xml");

            Console.ReadKey();

        }
    }
}
