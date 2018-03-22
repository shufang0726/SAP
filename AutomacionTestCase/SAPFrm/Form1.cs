using SAPFrm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAPFrm.DAL;
using System.Threading;
using System.Reflection;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net;
using System.Xml;
using System.Collections;
using System.Diagnostics;

namespace SAPFrm
{
    public partial class Form1 : Form
    {
        DBHelperJob DBJB = new DBHelperJob();
        public List<string> machines = new List<string>();
        public List<string> machines_result = new List<string>();
        public List<job> jobs = new List<job>();
        public List<string> jobs_result = new List<string>();
        public string pool = null;
        HLKModel hm = new HLKModel();
        public class job
        {
            public int jid { get; set; }
            public string jname { get; set; }
            public int? jfeatureid { get; set; }
            public string fpath { get; set; }
            public string fname { get; set; }

        }

        public class result
        {
            public string machineName { get; set; }
            public int? jobid { get; set; }
            public int Result { get; set; }

        }
        public enum type
        {
            Client,
            Web,
            Android,
            IOS
        }
        public Form1()
        {
            InitializeComponent();
            //this.RunCase.Enabled = false;
            progressBar.Visible = false;
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            machine_additems(type.Client.ToString());
            job_additems(type.Client.ToString());
            pool = type.Client.ToString();
        }
        private void Th_()
        {
            HLKController hc = new HLKController();
            string[] scheduleid = hc.RunJob(jobs_result, machines_result, pool).Split('~');
            if (scheduleid.Length == 1 && scheduleid[0].Contains('*'))
            {
                SetControlPropertyValue(Lab_Output, "text", scheduleid[0] + "error");
                SetControlPropertyValue(Lab_Output, "ForeColor", Color.Red);
                return;
            }
            SetControlPropertyValue(progressBar, "value", 10);
            //SetControlPropertyValue(Lab_Output, "text", "Complete:10%        Please wait to get run results...");
            //Lab_Output.Text = "Complete:"+ progressBar.Value + @"%        Completed Run Jobs...";
            int i = 10;
            while (true)
            {
                List<result> result = DBJB.GetResultFromDB(scheduleid);
                i++;
                if (i == 91)
                    i--;
                SetControlPropertyValue(progressBar, "value", i);

                //SetControlPropertyValue(Lab_Output, "text", "Complete:" + i + @"%        Please wait to get run results...");
                if (result.Count == scheduleid.Length - 1)
                {
                    
                    string filename = hm.ExcelModify(result, jobs_result, machines_result, pool);
                    SetControlPropertyValue(progressBar, "value", 100);
                    SetControlPropertyValue(Lab_Output, "text", "Complete:100% Excel Exported Successfully! ");
                    SetControlPropertyValue(linkLabel, "text", filename);
                    break;
                }
                Thread.Sleep(1000);
            }

            SetControlPropertyValue(comCheckBoxList1, "enabled", true);
            SetControlPropertyValue(comCheckBoxList2, "enabled", true);
            SetControlPropertyValue(ClientCheck, "enabled", true);
            SetControlPropertyValue(WebCheck, "enabled", true);
            SetControlPropertyValue(AndroidCheck, "enabled", true);
            SetControlPropertyValue(IOSCheck, "enabled", true);
            SetControlPropertyValue(RunCase, "enabled", true);

        }
        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }

        private void RunCase_Click(object sender, EventArgs e)
        {
           // hm.CopyDllFile(pool, machines_result);
            progressBar.Value = 0;
            linkLabel.Text = "";
            if (jobs_result.Count == 0 || machines_result.Count == 0)
                return;

            comCheckBoxList1.Enabled = false;
            comCheckBoxList2.Enabled = false;
            ClientCheck.Enabled = false;
            WebCheck.Enabled = false;
            AndroidCheck.Enabled = false;
            IOSCheck.Enabled = false;
            RunCase.Enabled = false;
            Lab_Output.Text = "       Start Run Jobs...";

            Thread read = new Thread(new ThreadStart(Th_));
            read.Start();
        }
        private void comCheckBoxList1_ItemClick(object sender, ItemCheckEventArgs e)
        {
            string text = comCheckBoxList1.GetItemText(comCheckBoxList1.Items[e.Index]);
            if (machines_result.Contains(text))
                machines_result.Remove(text);
            else
                machines_result.Add(text);
        }

        private void comCheckBoxList2_ItemClick(object sender, ItemCheckEventArgs e)
        {
            string text = comCheckBoxList2.GetItemText(comCheckBoxList2.Items[e.Index]);
            if (jobs_result.Contains(text))
                jobs_result.Remove(text);
            else
                jobs_result.Add(text);
        }
        private void ClientCheck_CheckedChanged(object sender, EventArgs e)
        {
            pool = type.Client.ToString();
            comCheckBoxList1.RemoveAllItems();
            comCheckBoxList2.RemoveAllItems();
            machine_additems(pool);
            job_additems(pool);
        }

        private void WebCheck_CheckedChanged(object sender, EventArgs e)
        {
            pool = type.Web.ToString();
            comCheckBoxList1.RemoveAllItems();
            comCheckBoxList2.RemoveAllItems();
            machine_additems(pool);
            job_additems(pool);
        }

        private void AndroidCheck_CheckedChanged(object sender, EventArgs e)
        {
            pool = type.Android.ToString();
            comCheckBoxList1.RemoveAllItems();
            comCheckBoxList2.RemoveAllItems();
            machine_additems(pool);
            job_additems(pool);
        }

        private void IOSCheck_CheckedChanged(object sender, EventArgs e)
        {
            pool = type.IOS.ToString();
            comCheckBoxList1.RemoveAllItems();
            comCheckBoxList2.RemoveAllItems();
            machine_additems(pool);
            job_additems(pool);
        }
        public void machine_additems(string pool)
        {
            machines_result = new List<string>();
            try
            {
                HLKController hc = new HLKController();
                machines = hc.ConnectHLK(pool);
                foreach (string machinename in machines)
                comCheckBoxList1.AddItems(machinename);
            }
            catch { }
        }

        public void job_additems(string pool)
        {
            jobs_result = new List<string>();
            try
            {
                jobs = DBJB.GetJobID(pool);
                foreach (job job in jobs)
                    comCheckBoxList2.AddItems(job.jid + "-" + job.jname);
            }
            catch { }
        }

        private void linkLabel_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("ExpLore", linkLabel.Text);
            System.Diagnostics.Process.Start(linkLabel.Text);

        }

        private void downloadXML_Click(object sender, EventArgs e)
        {
            HLKController hk = new HLKController();
            string getxmlpath = hk.GetXMLPath(pool);
            string[] XMLpath = getxmlpath.Split(',');
            this.linkLabel1.Visible = false;
            this.labDownload.Visible = true;
            //string originalPath = @"\\hlkserver\ShareFlie\Template_case.xml";
            //string path = @"D:\Template_case.xml";
            if (File.Exists(XMLpath[1]))
            {
                File.Delete(XMLpath[1]);
                File.Copy(XMLpath[0], XMLpath[1]);
            }
            else
            {
                File.Copy(XMLpath[0], XMLpath[1]);
            }
            this.labDownload.Text = "success! path:" + XMLpath[1];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoadXML_Click(object sender, EventArgs e)
        {   
            hm.CreateCSFile(pool, machines_result);
            this.labDownload.Visible = false;
            this.linkLabel1.Visible = true;
            this.RunCase.Enabled = true;
            string path = null;
            if (pool.Equals("Web"))
            {
                 path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Selenium\\bin\\Debug";
            }
            if (pool.Equals("Client"))
            {
                path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Client\\bin\\Debug";
            }
            if (pool.Equals("Android"))
            {
                path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 16) + "Automation_Android\\bin\\Debug";
            }
            linkLabel1.Text = path;
        }

        #region  move frm and close frm

        private Point mouseOff;
        private bool leftFlag;
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);
                leftFlag = true;
            }
        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);
                Location = mouseSet;
            }
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;
            }
        }
        private void btnExitSys_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }
    }
}