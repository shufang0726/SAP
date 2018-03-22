using Automation_UIAF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation_Client
{
   public static class ClientUtility
    {
        public static void StartProcess(string processName)
        {
            using (Process newProcess = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(processName);
                newProcess.StartInfo = startInfo;
                newProcess.Start();
                Thread.Sleep(25000);
                Native.MaxWindow(newProcess.MainWindowHandle);
            }     
        }

        public static void EndProcess(string processName)
        {
            Thread.Sleep(2000);
            string name = Path.GetFileNameWithoutExtension(processName);
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name.ToLower() && !process.HasExited)
                {
                    process.Kill();
                    Thread.Sleep(3000);
                }
            }
        }
    }
}
