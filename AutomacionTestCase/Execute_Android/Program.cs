using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execute_Android
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentPath = Environment.CurrentDirectory;
            string SolutionPath = currentPath.Substring(0, currentPath.Length - 16) + "Automation_Android\\Automation_Android.csproj";
            using (Process myPro = new Process())
            {
                //指定启动进程是调用的应用程序和命令行参数
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                psi.UseShellExecute = false;
                psi.RedirectStandardInput = true;
                myPro.StartInfo = psi;
                myPro.Start();

                myPro.StandardInput.WriteLine(@"cd C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE");
                myPro.StandardInput.WriteLine(@"devenv /build debug " + SolutionPath + "");
                System.Threading.Thread.Sleep(9000);


            }
        }
    }
}
