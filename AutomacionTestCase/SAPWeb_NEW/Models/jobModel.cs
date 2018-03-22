using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAPWeb_NEW.Models
{
    public class job
    {
        public int jid { get; set; }
        public string jname { get; set; }
        public int? jfeatureid { get; set; }
        public string fpath { get; set; }
        public string fname { get; set; }
    }
}