namespace ScrapEra.Gui
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.tabDebug = new MetroFramework.Controls.MetroTabPage();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroButton4 = new MetroFramework.Controls.MetroButton();
            this.btnReport = new MetroFramework.Controls.MetroButton();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.btnClear = new MetroFramework.Controls.MetroButton();
            this.lstLogs = new System.Windows.Forms.ListBox();
            this.tabDashboard = new MetroFramework.Controls.MetroTabPage();
            this.tabHome = new MetroFramework.Controls.MetroTabPage();
            this.tabLocalScraping = new MetroFramework.Controls.MetroTabPage();
            this.tabAbout = new MetroFramework.Controls.MetroTabPage();
            this.txtAbout = new MetroFramework.Controls.MetroTextBox();
            this.metroTabControl1.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.tabDashboard);
            this.metroTabControl1.Controls.Add(this.tabDebug);
            this.metroTabControl1.Controls.Add(this.tabAbout);
            this.metroTabControl1.Controls.Add(this.tabHome);
            this.metroTabControl1.Controls.Add(this.tabLocalScraping);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(20, 60);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 4;
            this.metroTabControl1.Size = new System.Drawing.Size(720, 405);
            this.metroTabControl1.TabIndex = 0;
            this.metroTabControl1.UseSelectable = true;
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.metroPanel1);
            this.tabDebug.HorizontalScrollbarBarColor = true;
            this.tabDebug.HorizontalScrollbarHighlightOnWheel = false;
            this.tabDebug.HorizontalScrollbarSize = 10;
            this.tabDebug.Location = new System.Drawing.Point(4, 38);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Size = new System.Drawing.Size(712, 363);
            this.tabDebug.TabIndex = 2;
            this.tabDebug.Text = "Lab/Diagnosis";
            this.tabDebug.VerticalScrollbarBarColor = true;
            this.tabDebug.VerticalScrollbarHighlightOnWheel = false;
            this.tabDebug.VerticalScrollbarSize = 10;
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.metroButton4);
            this.metroPanel1.Controls.Add(this.btnReport);
            this.metroPanel1.Controls.Add(this.btnSave);
            this.metroPanel1.Controls.Add(this.btnClear);
            this.metroPanel1.Controls.Add(this.lstLogs);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(712, 363);
            this.metroPanel1.TabIndex = 2;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // metroButton4
            // 
            this.metroButton4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.metroButton4.Location = new System.Drawing.Point(527, 315);
            this.metroButton4.Name = "metroButton4";
            this.metroButton4.Size = new System.Drawing.Size(165, 36);
            this.metroButton4.TabIndex = 6;
            this.metroButton4.Text = "Todo";
            this.metroButton4.UseSelectable = true;
            // 
            // btnReport
            // 
            this.btnReport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReport.Location = new System.Drawing.Point(356, 315);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(165, 36);
            this.btnReport.TabIndex = 5;
            this.btnReport.Text = "Report";
            this.btnReport.UseSelectable = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(185, 315);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(165, 36);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseSelectable = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClear.Location = new System.Drawing.Point(14, 315);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(165, 36);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseSelectable = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lstLogs
            // 
            this.lstLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstLogs.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogs.FormattingEnabled = true;
            this.lstLogs.ItemHeight = 18;
            this.lstLogs.Items.AddRange(new object[] {
            "ds",
            "d",
            "d",
            "d",
            "s",
            "ds",
            "dd",
            "sd",
            "sd",
            "d",
            "s",
            "ds"});
            this.lstLogs.Location = new System.Drawing.Point(14, 21);
            this.lstLogs.Name = "lstLogs";
            this.lstLogs.Size = new System.Drawing.Size(683, 288);
            this.lstLogs.TabIndex = 2;
            // 
            // tabDashboard
            // 
            this.tabDashboard.HorizontalScrollbarBarColor = true;
            this.tabDashboard.HorizontalScrollbarHighlightOnWheel = false;
            this.tabDashboard.HorizontalScrollbarSize = 10;
            this.tabDashboard.Location = new System.Drawing.Point(4, 38);
            this.tabDashboard.Name = "tabDashboard";
            this.tabDashboard.Size = new System.Drawing.Size(712, 363);
            this.tabDashboard.TabIndex = 0;
            this.tabDashboard.Text = "Dashboard";
            this.tabDashboard.VerticalScrollbarBarColor = true;
            this.tabDashboard.VerticalScrollbarHighlightOnWheel = false;
            this.tabDashboard.VerticalScrollbarSize = 10;
            // 
            // tabHome
            // 
            this.tabHome.HorizontalScrollbarBarColor = true;
            this.tabHome.HorizontalScrollbarHighlightOnWheel = false;
            this.tabHome.HorizontalScrollbarSize = 10;
            this.tabHome.Location = new System.Drawing.Point(4, 38);
            this.tabHome.Name = "tabHome";
            this.tabHome.Size = new System.Drawing.Size(712, 363);
            this.tabHome.TabIndex = 1;
            this.tabHome.Text = "ScrapE home";
            this.tabHome.VerticalScrollbarBarColor = true;
            this.tabHome.VerticalScrollbarHighlightOnWheel = false;
            this.tabHome.VerticalScrollbarSize = 10;
            // 
            // tabLocalScraping
            // 
            this.tabLocalScraping.HorizontalScrollbarBarColor = true;
            this.tabLocalScraping.HorizontalScrollbarHighlightOnWheel = false;
            this.tabLocalScraping.HorizontalScrollbarSize = 10;
            this.tabLocalScraping.Location = new System.Drawing.Point(4, 38);
            this.tabLocalScraping.Name = "tabLocalScraping";
            this.tabLocalScraping.Size = new System.Drawing.Size(712, 363);
            this.tabLocalScraping.TabIndex = 4;
            this.tabLocalScraping.Text = "Local Scraping Beta";
            this.tabLocalScraping.VerticalScrollbarBarColor = true;
            this.tabLocalScraping.VerticalScrollbarHighlightOnWheel = false;
            this.tabLocalScraping.VerticalScrollbarSize = 10;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.txtAbout);
            this.tabAbout.HorizontalScrollbarBarColor = true;
            this.tabAbout.HorizontalScrollbarHighlightOnWheel = false;
            this.tabAbout.HorizontalScrollbarSize = 10;
            this.tabAbout.Location = new System.Drawing.Point(4, 38);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Size = new System.Drawing.Size(712, 363);
            this.tabAbout.TabIndex = 3;
            this.tabAbout.Text = "About";
            this.tabAbout.VerticalScrollbarBarColor = true;
            this.tabAbout.VerticalScrollbarHighlightOnWheel = false;
            this.tabAbout.VerticalScrollbarSize = 10;
            // 
            // txtAbout
            // 
            // 
            // 
            // 
            this.txtAbout.CustomButton.Image = null;
            this.txtAbout.CustomButton.Location = new System.Drawing.Point(364, 2);
            this.txtAbout.CustomButton.Name = "";
            this.txtAbout.CustomButton.Size = new System.Drawing.Size(343, 343);
            this.txtAbout.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtAbout.CustomButton.TabIndex = 1;
            this.txtAbout.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtAbout.CustomButton.UseSelectable = true;
            this.txtAbout.CustomButton.Visible = false;
            this.txtAbout.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtAbout.FontWeight = MetroFramework.MetroTextBoxWeight.Bold;
            this.txtAbout.Lines = new string[] {
        "metroTextBox1"};
            this.txtAbout.Location = new System.Drawing.Point(3, 12);
            this.txtAbout.MaxLength = 32767;
            this.txtAbout.Multiline = true;
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.PasswordChar = '\0';
            this.txtAbout.ReadOnly = true;
            this.txtAbout.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtAbout.SelectedText = "";
            this.txtAbout.SelectionLength = 0;
            this.txtAbout.SelectionStart = 0;
            this.txtAbout.Size = new System.Drawing.Size(710, 348);
            this.txtAbout.TabIndex = 2;
            this.txtAbout.Text = "metroTextBox1";
            this.txtAbout.UseSelectable = true;
            this.txtAbout.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtAbout.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // MainForm
            // 
            this.ApplyImageInvert = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = ((System.Drawing.Image)(resources.GetObject("$this.BackImage")));
            this.BackImagePadding = new System.Windows.Forms.Padding(230, 10, 0, 0);
            this.BackMaxSize = 50;
            this.ClientSize = new System.Drawing.Size(760, 485);
            this.Controls.Add(this.metroTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "ScrapEra";
            this.TransparencyKey = System.Drawing.Color.SkyBlue;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.metroTabControl1.ResumeLayout(false);
            this.tabDebug.ResumeLayout(false);
            this.metroPanel1.ResumeLayout(false);
            this.tabAbout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private MetroFramework.Controls.MetroTabPage tabDashboard;
        private MetroFramework.Controls.MetroTabPage tabHome;
        private MetroFramework.Controls.MetroTabPage tabDebug;
        private MetroFramework.Controls.MetroTabPage tabAbout;
        private MetroFramework.Controls.MetroTabPage tabLocalScraping;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private System.Windows.Forms.ListBox lstLogs;
        private MetroFramework.Controls.MetroButton metroButton4;
        private MetroFramework.Controls.MetroButton btnReport;
        private MetroFramework.Controls.MetroButton btnSave;
        private MetroFramework.Controls.MetroButton btnClear;
        private MetroFramework.Controls.MetroTextBox txtAbout;




    }
}

