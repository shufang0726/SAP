using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAPWeb_NEW.Models
{
    public class XMLFileModel
    {
        public string pool { get; set; }
        public string filePath { get; set; }

        public List<string> checkedMachinceList { get; set; }

        public List<string> jobs_result { get; set; }

        public List<string> machines_result { get; set; }
    }
}