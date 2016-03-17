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
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.txtCleanReaderUrl = new System.Windows.Forms.TextBox();
            this.btnPreview = new MetroFramework.Controls.MetroButton();
            this.btnCleanReaderHelp = new MetroFramework.Controls.MetroButton();
            this.btnCleanReaderConfigure = new MetroFramework.Controls.MetroButton();
            this.btnCleanReaderToTxt = new MetroFramework.Controls.MetroButton();
            this.btnCleanReaderToHtml = new MetroFramework.Controls.MetroButton();
            this.btnCleanReaderToPdf = new MetroFramework.Controls.MetroButton();
            this.webCleanReader = new System.Windows.Forms.WebBrowser();
            this.btnLoadCleanReader = new MetroFramework.Controls.MetroButton();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabDebug = new MetroFramework.Controls.MetroTabPage();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroButton4 = new MetroFramework.Controls.MetroButton();
            this.btnReport = new MetroFramework.Controls.MetroButton();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.btnClear = new MetroFramework.Controls.MetroButton();
            this.lstLogs = new System.Windows.Forms.ListBox();
            this.tabAbout = new MetroFramework.Controls.MetroTabPage();
            this.txtAbout = new MetroFramework.Controls.MetroTextBox();
            this.tabHome = new MetroFramework.Controls.MetroTabPage();
            this.tabDashboard = new MetroFramework.Controls.MetroTabPage();
            this.tabLocalScraping = new MetroFramework.Controls.MetroTabPage();
            this.metroTabControl1.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.metroTabPage2.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.metroTabPage1);
            this.metroTabControl1.Controls.Add(this.metroTabPage2);
            this.metroTabControl1.Controls.Add(this.tabDebug);
            this.metroTabControl1.Controls.Add(this.tabAbout);
            this.metroTabControl1.Controls.Add(this.tabHome);
            this.metroTabControl1.Controls.Add(this.tabDashboard);
            this.metroTabControl1.Controls.Add(this.tabLocalScraping);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(40, 115);
            this.metroTabControl1.Margin = new System.Windows.Forms.Padding(6);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(1440, 846);
            this.metroTabControl1.TabIndex = 2;
            this.metroTabControl1.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.Controls.Add(this.txtCleanReaderUrl);
            this.metroTabPage1.Controls.Add(this.btnPreview);
            this.metroTabPage1.Controls.Add(this.btnCleanReaderHelp);
            this.metroTabPage1.Controls.Add(this.btnCleanReaderConfigure);
            this.metroTabPage1.Controls.Add(this.btnCleanReaderToTxt);
            this.metroTabPage1.Controls.Add(this.btnCleanReaderToHtml);
            this.metroTabPage1.Controls.Add(this.btnCleanReaderToPdf);
            this.metroTabPage1.Controls.Add(this.webCleanReader);
            this.metroTabPage1.Controls.Add(this.btnLoadCleanReader);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 19;
            this.metroTabPage1.Location = new System.Drawing.Point(8, 41);
            this.metroTabPage1.Margin = new System.Windows.Forms.Padding(6);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(1424, 797);
            this.metroTabPage1.TabIndex = 5;
            this.metroTabPage1.Text = "Clean Reader";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 20;
            // 
            // txtCleanReaderUrl
            // 
            this.txtCleanReaderUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCleanReaderUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtCleanReaderUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtCleanReaderUrl.Location = new System.Drawing.Point(3, 15);
            this.txtCleanReaderUrl.Name = "txtCleanReaderUrl";
            this.txtCleanReaderUrl.Size = new System.Drawing.Size(1198, 31);
            this.txtCleanReaderUrl.TabIndex = 11;
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.Location = new System.Drawing.Point(1216, 566);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(6);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(202, 106);
            this.btnPreview.TabIndex = 10;
            this.btnPreview.Text = "Print Preview";
            this.btnPreview.UseSelectable = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnCleanReaderHelp
            // 
            this.btnCleanReaderHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanReaderHelp.Location = new System.Drawing.Point(1216, 684);
            this.btnCleanReaderHelp.Margin = new System.Windows.Forms.Padding(6);
            this.btnCleanReaderHelp.Name = "btnCleanReaderHelp";
            this.btnCleanReaderHelp.Size = new System.Drawing.Size(202, 106);
            this.btnCleanReaderHelp.TabIndex = 9;
            this.btnCleanReaderHelp.Text = "Help";
            this.btnCleanReaderHelp.UseSelectable = true;
            this.btnCleanReaderHelp.Click += new System.EventHandler(this.btnCleanReaderHelp_Click);
            // 
            // btnCleanReaderConfigure
            // 
            this.btnCleanReaderConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanReaderConfigure.Location = new System.Drawing.Point(1216, 448);
            this.btnCleanReaderConfigure.Margin = new System.Windows.Forms.Padding(6);
            this.btnCleanReaderConfigure.Name = "btnCleanReaderConfigure";
            this.btnCleanReaderConfigure.Size = new System.Drawing.Size(202, 106);
            this.btnCleanReaderConfigure.TabIndex = 8;
            this.btnCleanReaderConfigure.Text = "Print Page";
            this.btnCleanReaderConfigure.UseSelectable = true;
            this.btnCleanReaderConfigure.Click += new System.EventHandler(this.btnCleanReaderConfigure_Click);
            // 
            // btnCleanReaderToTxt
            // 
            this.btnCleanReaderToTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanReaderToTxt.Location = new System.Drawing.Point(1216, 331);
            this.btnCleanReaderToTxt.Margin = new System.Windows.Forms.Padding(6);
            this.btnCleanReaderToTxt.Name = "btnCleanReaderToTxt";
            this.btnCleanReaderToTxt.Size = new System.Drawing.Size(202, 106);
            this.btnCleanReaderToTxt.TabIndex = 7;
            this.btnCleanReaderToTxt.Text = "ToTXT";
            this.btnCleanReaderToTxt.UseSelectable = true;
            this.btnCleanReaderToTxt.Click += new System.EventHandler(this.btnCleanReaderToTxt_Click);
            // 
            // btnCleanReaderToHtml
            // 
            this.btnCleanReaderToHtml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanReaderToHtml.Location = new System.Drawing.Point(1216, 213);
            this.btnCleanReaderToHtml.Margin = new System.Windows.Forms.Padding(6);
            this.btnCleanReaderToHtml.Name = "btnCleanReaderToHtml";
            this.btnCleanReaderToHtml.Size = new System.Drawing.Size(202, 106);
            this.btnCleanReaderToHtml.TabIndex = 6;
            this.btnCleanReaderToHtml.Text = "ToHTML";
            this.btnCleanReaderToHtml.UseSelectable = true;
            this.btnCleanReaderToHtml.Click += new System.EventHandler(this.btnCleanReaderToHtml_Click);
            // 
            // btnCleanReaderToPdf
            // 
            this.btnCleanReaderToPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanReaderToPdf.Location = new System.Drawing.Point(1216, 96);
            this.btnCleanReaderToPdf.Margin = new System.Windows.Forms.Padding(6);
            this.btnCleanReaderToPdf.Name = "btnCleanReaderToPdf";
            this.btnCleanReaderToPdf.Size = new System.Drawing.Size(202, 106);
            this.btnCleanReaderToPdf.TabIndex = 5;
            this.btnCleanReaderToPdf.Text = "ToPDF";
            this.btnCleanReaderToPdf.UseSelectable = true;
            this.btnCleanReaderToPdf.Click += new System.EventHandler(this.btnCleanReaderToPdf_Click);
            // 
            // webCleanReader
            // 
            this.webCleanReader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webCleanReader.Location = new System.Drawing.Point(6, 96);
            this.webCleanReader.Margin = new System.Windows.Forms.Padding(6);
            this.webCleanReader.MinimumSize = new System.Drawing.Size(40, 38);
            this.webCleanReader.Name = "webCleanReader";
            this.webCleanReader.Size = new System.Drawing.Size(1198, 663);
            this.webCleanReader.TabIndex = 4;
            // 
            // btnLoadCleanReader
            // 
            this.btnLoadCleanReader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadCleanReader.Location = new System.Drawing.Point(1216, 6);
            this.btnLoadCleanReader.Margin = new System.Windows.Forms.Padding(6);
            this.btnLoadCleanReader.Name = "btnLoadCleanReader";
            this.btnLoadCleanReader.Size = new System.Drawing.Size(202, 58);
            this.btnLoadCleanReader.TabIndex = 3;
            this.btnLoadCleanReader.Text = "Load";
            this.btnLoadCleanReader.UseSelectable = true;
            this.btnLoadCleanReader.Click += new System.EventHandler(this.btnLoadCleanReader_Click);
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.Controls.Add(this.tableLayoutPanel1);
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 19;
            this.metroTabPage2.Location = new System.Drawing.Point(8, 41);
            this.metroTabPage2.Margin = new System.Windows.Forms.Padding(6);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(1424, 797);
            this.metroTabPage2.TabIndex = 6;
            this.metroTabPage2.Text = "Settings";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 20;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1424, 797);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.metroPanel1);
            this.tabDebug.HorizontalScrollbarBarColor = true;
            this.tabDebug.HorizontalScrollbarHighlightOnWheel = false;
            this.tabDebug.HorizontalScrollbarSize = 19;
            this.tabDebug.Location = new System.Drawing.Point(8, 41);
            this.tabDebug.Margin = new System.Windows.Forms.Padding(6);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Size = new System.Drawing.Size(1424, 797);
            this.tabDebug.TabIndex = 2;
            this.tabDebug.Text = "Lab/Diagnosis";
            this.tabDebug.VerticalScrollbarBarColor = true;
            this.tabDebug.VerticalScrollbarHighlightOnWheel = false;
            this.tabDebug.VerticalScrollbarSize = 20;
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
            this.metroPanel1.HorizontalScrollbarSize = 19;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(1424, 797);
            this.metroPanel1.TabIndex = 2;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 20;
            // 
            // metroButton4
            // 
            this.metroButton4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.metroButton4.Location = new System.Drawing.Point(1054, 705);
            this.metroButton4.Margin = new System.Windows.Forms.Padding(6);
            this.metroButton4.Name = "metroButton4";
            this.metroButton4.Size = new System.Drawing.Size(330, 69);
            this.metroButton4.TabIndex = 6;
            this.metroButton4.Text = "Todo";
            this.metroButton4.UseSelectable = true;
            // 
            // btnReport
            // 
            this.btnReport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReport.Location = new System.Drawing.Point(712, 705);
            this.btnReport.Margin = new System.Windows.Forms.Padding(6);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(330, 69);
            this.btnReport.TabIndex = 5;
            this.btnReport.Text = "Report";
            this.btnReport.UseSelectable = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(370, 705);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(330, 69);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseSelectable = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClear.Location = new System.Drawing.Point(28, 705);
            this.btnClear.Margin = new System.Windows.Forms.Padding(6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(330, 69);
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
            this.lstLogs.ItemHeight = 36;
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
            this.lstLogs.Location = new System.Drawing.Point(28, 40);
            this.lstLogs.Margin = new System.Windows.Forms.Padding(6);
            this.lstLogs.Name = "lstLogs";
            this.lstLogs.Size = new System.Drawing.Size(1366, 540);
            this.lstLogs.TabIndex = 2;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.txtAbout);
            this.tabAbout.HorizontalScrollbarBarColor = true;
            this.tabAbout.HorizontalScrollbarHighlightOnWheel = false;
            this.tabAbout.HorizontalScrollbarSize = 19;
            this.tabAbout.Location = new System.Drawing.Point(8, 41);
            this.tabAbout.Margin = new System.Windows.Forms.Padding(6);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Size = new System.Drawing.Size(1424, 797);
            this.tabAbout.TabIndex = 3;
            this.tabAbout.Text = "About";
            this.tabAbout.VerticalScrollbarBarColor = true;
            this.tabAbout.VerticalScrollbarHighlightOnWheel = false;
            this.tabAbout.VerticalScrollbarSize = 20;
            // 
            // txtAbout
            // 
            this.txtAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtAbout.CustomButton.Image = null;
            this.txtAbout.CustomButton.Location = new System.Drawing.Point(752, 1);
            this.txtAbout.CustomButton.Margin = new System.Windows.Forms.Padding(12);
            this.txtAbout.CustomButton.Name = "";
            this.txtAbout.CustomButton.Size = new System.Drawing.Size(667, 667);
            this.txtAbout.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtAbout.CustomButton.TabIndex = 1;
            this.txtAbout.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtAbout.CustomButton.UseSelectable = true;
            this.txtAbout.CustomButton.Visible = false;
            this.txtAbout.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtAbout.FontWeight = MetroFramework.MetroTextBoxWeight.Bold;
            this.txtAbout.Lines = new string[] {
        "metroTextBox1"};
            this.txtAbout.Location = new System.Drawing.Point(6, 23);
            this.txtAbout.Margin = new System.Windows.Forms.Padding(6);
            this.txtAbout.MaxLength = 32767;
            this.txtAbout.Multiline = true;
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.PasswordChar = '\0';
            this.txtAbout.ReadOnly = true;
            this.txtAbout.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtAbout.SelectedText = "";
            this.txtAbout.SelectionLength = 0;
            this.txtAbout.SelectionStart = 0;
            this.txtAbout.Size = new System.Drawing.Size(1420, 669);
            this.txtAbout.TabIndex = 2;
            this.txtAbout.Text = "metroTextBox1";
            this.txtAbout.UseSelectable = true;
            this.txtAbout.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtAbout.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // tabHome
            // 
            this.tabHome.HorizontalScrollbarBarColor = true;
            this.tabHome.HorizontalScrollbarHighlightOnWheel = false;
            this.tabHome.HorizontalScrollbarSize = 19;
            this.tabHome.Location = new System.Drawing.Point(8, 41);
            this.tabHome.Margin = new System.Windows.Forms.Padding(6);
            this.tabHome.Name = "tabHome";
            this.tabHome.Size = new System.Drawing.Size(1424, 797);
            this.tabHome.TabIndex = 1;
            this.tabHome.Text = "ScrapE home";
            this.tabHome.VerticalScrollbarBarColor = true;
            this.tabHome.VerticalScrollbarHighlightOnWheel = false;
            this.tabHome.VerticalScrollbarSize = 20;
            // 
            // tabDashboard
            // 
            this.tabDashboard.HorizontalScrollbarBarColor = true;
            this.tabDashboard.HorizontalScrollbarHighlightOnWheel = false;
            this.tabDashboard.HorizontalScrollbarSize = 19;
            this.tabDashboard.Location = new System.Drawing.Point(8, 41);
            this.tabDashboard.Margin = new System.Windows.Forms.Padding(6);
            this.tabDashboard.Name = "tabDashboard";
            this.tabDashboard.Size = new System.Drawing.Size(1424, 797);
            this.tabDashboard.TabIndex = 0;
            this.tabDashboard.Text = "Dashboard";
            this.tabDashboard.VerticalScrollbarBarColor = true;
            this.tabDashboard.VerticalScrollbarHighlightOnWheel = false;
            this.tabDashboard.VerticalScrollbarSize = 20;
            // 
            // tabLocalScraping
            // 
            this.tabLocalScraping.HorizontalScrollbarBarColor = true;
            this.tabLocalScraping.HorizontalScrollbarHighlightOnWheel = false;
            this.tabLocalScraping.HorizontalScrollbarSize = 19;
            this.tabLocalScraping.Location = new System.Drawing.Point(8, 41);
            this.tabLocalScraping.Margin = new System.Windows.Forms.Padding(6);
            this.tabLocalScraping.Name = "tabLocalScraping";
            this.tabLocalScraping.Size = new System.Drawing.Size(1424, 797);
            this.tabLocalScraping.TabIndex = 4;
            this.tabLocalScraping.Text = "Local Scraping Beta";
            this.tabLocalScraping.VerticalScrollbarBarColor = true;
            this.tabLocalScraping.VerticalScrollbarHighlightOnWheel = false;
            this.tabLocalScraping.VerticalScrollbarSize = 20;
            // 
            // MainForm
            // 
            this.ApplyImageInvert = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = ((System.Drawing.Image)(resources.GetObject("$this.BackImage")));
            this.BackImagePadding = new System.Windows.Forms.Padding(430, 10, 0, 0);
            this.BackMaxSize = 50;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1520, 999);
            this.Controls.Add(this.metroTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(40, 115, 40, 38);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Style = MetroFramework.MetroColorStyle.Orange;
            this.Text = "ScrapEra : Web Scraping Suite";
            this.TransparencyKey = System.Drawing.Color.SkyBlue;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.metroTabControl1.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.metroTabPage1.PerformLayout();
            this.metroTabPage2.ResumeLayout(false);
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
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroButton btnLoadCleanReader;
        private System.Windows.Forms.WebBrowser webCleanReader;
        private MetroFramework.Controls.MetroButton btnCleanReaderConfigure;
        private MetroFramework.Controls.MetroButton btnCleanReaderToTxt;
        private MetroFramework.Controls.MetroButton btnCleanReaderToHtml;
        private MetroFramework.Controls.MetroButton btnCleanReaderToPdf;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetroFramework.Controls.MetroButton btnCleanReaderHelp;
        private MetroFramework.Controls.MetroButton btnPreview;
        private System.Windows.Forms.TextBox txtCleanReaderUrl;
    }
}