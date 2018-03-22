namespace SAPFrm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.RunCase = new System.Windows.Forms.Button();
            this.ClientCheck = new System.Windows.Forms.RadioButton();
            this.IOSCheck = new System.Windows.Forms.RadioButton();
            this.AndroidCheck = new System.Windows.Forms.RadioButton();
            this.WebCheck = new System.Windows.Forms.RadioButton();
            this.Lab_Output = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comCheckBoxList2 = new ComboBoxCustomer.ComCheckBoxList();
            this.comCheckBoxList1 = new ComboBoxCustomer.ComCheckBoxList();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.downloadXML = new System.Windows.Forms.Button();
            this.labDownload = new System.Windows.Forms.Label();
            this.btnLoadXML = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExitSys = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RunCase
            // 
            this.RunCase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RunCase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunCase.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RunCase.Location = new System.Drawing.Point(400, 63);
            this.RunCase.Name = "RunCase";
            this.RunCase.Size = new System.Drawing.Size(109, 27);
            this.RunCase.TabIndex = 0;
            this.RunCase.Text = "Run Test Case";
            this.RunCase.UseVisualStyleBackColor = true;
            this.RunCase.Click += new System.EventHandler(this.RunCase_Click);
            // 
            // ClientCheck
            // 
            this.ClientCheck.AutoSize = true;
            this.ClientCheck.Checked = true;
            this.ClientCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientCheck.Location = new System.Drawing.Point(129, 126);
            this.ClientCheck.Name = "ClientCheck";
            this.ClientCheck.Size = new System.Drawing.Size(89, 20);
            this.ClientCheck.TabIndex = 1;
            this.ClientCheck.TabStop = true;
            this.ClientCheck.Text = "Client Test";
            this.ClientCheck.UseVisualStyleBackColor = true;
            this.ClientCheck.CheckedChanged += new System.EventHandler(this.ClientCheck_CheckedChanged);
            // 
            // IOSCheck
            // 
            this.IOSCheck.AutoSize = true;
            this.IOSCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IOSCheck.Location = new System.Drawing.Point(431, 126);
            this.IOSCheck.Name = "IOSCheck";
            this.IOSCheck.Size = new System.Drawing.Size(78, 20);
            this.IOSCheck.TabIndex = 2;
            this.IOSCheck.Text = "IOS Test";
            this.IOSCheck.UseVisualStyleBackColor = true;
            this.IOSCheck.CheckedChanged += new System.EventHandler(this.IOSCheck_CheckedChanged);
            // 
            // AndroidCheck
            // 
            this.AndroidCheck.AutoSize = true;
            this.AndroidCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AndroidCheck.Location = new System.Drawing.Point(320, 126);
            this.AndroidCheck.Name = "AndroidCheck";
            this.AndroidCheck.Size = new System.Drawing.Size(103, 20);
            this.AndroidCheck.TabIndex = 3;
            this.AndroidCheck.Text = "Android Test";
            this.AndroidCheck.UseVisualStyleBackColor = true;
            this.AndroidCheck.CheckedChanged += new System.EventHandler(this.AndroidCheck_CheckedChanged);
            // 
            // WebCheck
            // 
            this.WebCheck.AutoSize = true;
            this.WebCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebCheck.Location = new System.Drawing.Point(227, 126);
            this.WebCheck.Name = "WebCheck";
            this.WebCheck.Size = new System.Drawing.Size(88, 20);
            this.WebCheck.TabIndex = 4;
            this.WebCheck.Text = "Web Test ";
            this.WebCheck.UseVisualStyleBackColor = true;
            this.WebCheck.CheckedChanged += new System.EventHandler(this.WebCheck_CheckedChanged);
            // 
            // Lab_Output
            // 
            this.Lab_Output.AutoSize = true;
            this.Lab_Output.Location = new System.Drawing.Point(160, 310);
            this.Lab_Output.Name = "Lab_Output";
            this.Lab_Output.Size = new System.Drawing.Size(0, 13);
            this.Lab_Output.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(116, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Machines :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(141, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Jobs :";
            // 
            // comCheckBoxList2
            // 
            this.comCheckBoxList2.DataSource = null;
            this.comCheckBoxList2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comCheckBoxList2.Location = new System.Drawing.Point(197, 226);
            this.comCheckBoxList2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comCheckBoxList2.Name = "comCheckBoxList2";
            this.comCheckBoxList2.Size = new System.Drawing.Size(217, 21);
            this.comCheckBoxList2.TabIndex = 11;
            this.comCheckBoxList2.ItemClick += new ComboBoxCustomer.ComCheckBoxList.CheckBoxListItemClick(this.comCheckBoxList2_ItemClick);
            // 
            // comCheckBoxList1
            // 
            this.comCheckBoxList1.DataSource = null;
            this.comCheckBoxList1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comCheckBoxList1.Location = new System.Drawing.Point(197, 178);
            this.comCheckBoxList1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comCheckBoxList1.Name = "comCheckBoxList1";
            this.comCheckBoxList1.Size = new System.Drawing.Size(217, 21);
            this.comCheckBoxList1.TabIndex = 10;
            this.comCheckBoxList1.ItemClick += new ComboBoxCustomer.ComCheckBoxList.CheckBoxListItemClick(this.comCheckBoxList1_ItemClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(58, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Test type:";
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(33, 282);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(0, 13);
            this.linkLabel.TabIndex = 15;
            this.linkLabel.Click += new System.EventHandler(this.linkLabel_Click);
            // 
            // downloadXML
            // 
            this.downloadXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.downloadXML.Location = new System.Drawing.Point(47, 63);
            this.downloadXML.Name = "downloadXML";
            this.downloadXML.Size = new System.Drawing.Size(171, 27);
            this.downloadXML.TabIndex = 16;
            this.downloadXML.Text = "Download XML template";
            this.downloadXML.UseVisualStyleBackColor = true;
            this.downloadXML.Click += new System.EventHandler(this.downloadXML_Click);
            // 
            // labDownload
            // 
            this.labDownload.AutoSize = true;
            this.labDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDownload.ForeColor = System.Drawing.Color.Red;
            this.labDownload.Location = new System.Drawing.Point(179, 103);
            this.labDownload.Name = "labDownload";
            this.labDownload.Size = new System.Drawing.Size(0, 16);
            this.labDownload.TabIndex = 17;
            // 
            // btnLoadXML
            // 
            this.btnLoadXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnLoadXML.Location = new System.Drawing.Point(242, 63);
            this.btnLoadXML.Name = "btnLoadXML";
            this.btnLoadXML.Size = new System.Drawing.Size(134, 27);
            this.btnLoadXML.TabIndex = 18;
            this.btnLoadXML.Text = "Load XML";
            this.btnLoadXML.UseVisualStyleBackColor = true;
            this.btnLoadXML.Click += new System.EventHandler(this.btnLoadXML_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(135)))), ((int)(((byte)(248)))));
            this.pictureBox1.Location = new System.Drawing.Point(1, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(576, 34);
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseUp);
            // 
            // btnExitSys
            // 
            this.btnExitSys.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(193)))), ((int)(((byte)(253)))));
            this.btnExitSys.CausesValidation = false;
            this.btnExitSys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitSys.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(135)))), ((int)(((byte)(248)))));
            this.btnExitSys.Image = ((System.Drawing.Image)(resources.GetObject("btnExitSys.Image")));
            this.btnExitSys.Location = new System.Drawing.Point(529, 2);
            this.btnExitSys.Name = "btnExitSys";
            this.btnExitSys.Size = new System.Drawing.Size(48, 34);
            this.btnExitSys.TabIndex = 20;
            this.btnExitSys.UseMnemonic = false;
            this.btnExitSys.UseVisualStyleBackColor = false;
            this.btnExitSys.Click += new System.EventHandler(this.btnExitSys_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(47, 97);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(0, 13);
            this.linkLabel1.TabIndex = 21;
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(41, 268);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(486, 25);
            this.progressBar.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(576, 340);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.btnExitSys);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnLoadXML);
            this.Controls.Add(this.labDownload);
            this.Controls.Add(this.downloadXML);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comCheckBoxList2);
            this.Controls.Add(this.comCheckBoxList1);
            this.Controls.Add(this.Lab_Output);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.WebCheck);
            this.Controls.Add(this.AndroidCheck);
            this.Controls.Add(this.IOSCheck);
            this.Controls.Add(this.ClientCheck);
            this.Controls.Add(this.RunCase);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SAP";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RunCase;
        private System.Windows.Forms.RadioButton ClientCheck;
        private System.Windows.Forms.RadioButton IOSCheck;
        private System.Windows.Forms.RadioButton AndroidCheck;
        private System.Windows.Forms.RadioButton WebCheck;
        private System.Windows.Forms.Label Lab_Output;
        private ComboBoxCustomer.ComCheckBoxList comCheckBoxList1;
        private ComboBoxCustomer.ComCheckBoxList comCheckBoxList2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Button downloadXML;
        private System.Windows.Forms.Label labDownload;
        private System.Windows.Forms.Button btnLoadXML;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnExitSys;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

