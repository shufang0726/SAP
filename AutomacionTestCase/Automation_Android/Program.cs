using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAF;

namespace Automation_Android
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.UseUIA = true;
            Config.NonLocalizedResourcesEnabled = true;
            Config.TestcaseTimeout = 1000 * 60 * 60 * 10;
            Config.MaximumTimeout = 1000 * 60 * 60 * 10;
            Config.SearchTimeout = 30000;
            AppVariables.Set(Config.Parameters.RpfInfoWindow, false);
            TestcaseHandler.Main(args);
        }
    }
}
