using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAF;

namespace Automation_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestcaseHandler.Main(args);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
