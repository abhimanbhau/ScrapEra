using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using OpenQA.Selenium.IE;
using ScrapEra.CleanReader;
using ScrapEra.Gui.Properties;
using ScrapEra.ScrapLogger;
using ScrapEra.Selenium;
using ScrapEra.Utils;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace ScrapEra.Gui
{
    public partial class MainForm : MetroForm
    {
        private readonly BindingSource _bs = new BindingSource();
        private readonly AutoCompleteStringCollection _urlArray;
        private Thread _cleanReaderCentralThread;
        private string _currentUrl;

        public MainForm()
        {
            InitializeComponent();
            _bs.DataSource = Logger.LogList;
            lstLogs.DataSource = _bs;
            PeriodicTaskFactory.Start(() => { _bs.ResetBindings(false); }, 2000, maxIterations: 10);
            //var list =  Properties.Settings.Default.UrlHistory.Cast<string>().ToList();
            _urlArray = new AutoCompleteStringCollection();
            _urlArray.AddRange(Settings.Default.UrlHistory.Cast<string>().ToArray());
            //urlArray.Cast<string>().ToList().ForEach(Console.WriteLine);
            txtCleanReaderUrl.AutoCompleteCustomSource = _urlArray;
            rbtnIE.Checked = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += " " + Settings.Default.Version;
            // About Page setup
            txtAbout.Text = Resources.AboutScrapEra;
            Refresh();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Logger.LogList.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllLines(dialog.FileName, lstLogs.Items.OfType<string>());
            }
        }

        private void btnLoadCleanReader_Click(object sender, EventArgs e)
        {
            Logger.LogI(GetType().FullName + "." + MethodBase.GetCurrentMethod().Name +
                        " Starting CleanReader for URL - " + txtCleanReaderUrl.Text);
            if (txtCleanReaderUrl.Text.Trim().Length == 0)
            {
                return;
            }
            if (!(txtCleanReaderUrl.Text.Trim().Contains("http://")) &&
                !txtCleanReaderUrl.Text.Trim().Contains("https://"))
            {
                txtCleanReaderUrl.Text = "http://" + txtCleanReaderUrl.Text;
            }
            if (_cleanReaderCentralThread != null)
            {
                MetroMessageBox.Show(this,
                    "Please Wait..\n\nAlready processing previous request",
                    "Already in progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            _currentUrl = txtCleanReaderUrl.Text;
            _cleanReaderCentralThread = new Thread(CleanReaderWorker) {IsBackground = true};
            _cleanReaderCentralThread.Start();
            _urlArray.Add(txtCleanReaderUrl.Text);
        }

        public void CleanReaderWorker()
        {
            try
            {
                var tr = new CleanReaderWeb();
                var stuff = tr.Transcode(_currentUrl);
                webCleanReader.DocumentText = stuff;
                CleanThread();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this,
                    ex.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Logger.LogE(ex.Source + " -> " + ex.Message +
                            Environment.NewLine + ex.StackTrace);
            }
        }

        private void CleanThread()
        {
            try
            {
                _cleanReaderCentralThread = null;
            }
            catch (Exception ex)
            {
                Logger.LogE(ex.Source + " -> " + ex.Message +
                            Environment.NewLine + ex.StackTrace);
            }
        }

        private void btnCleanReaderHelp_Click(object sender, EventArgs e)
        {
            MetroMessageBox.Show(this,
                "This tool help you clean clutter from URL pages\nIt uses intelligent algorithm to achieve the same\n\n"
                + "This tool is part of ScrapEra suite",
                "ScrapEra CleanReader Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnCleanReaderToPdf_Click(object sender, EventArgs e)
        {
        }

        private void btnCleanReaderToHtml_Click(object sender, EventArgs e)
        {
            var sf = new SaveFileDialog
            {
                Title = "Select File to save HTML content as",
                DefaultExt = "html",
                AddExtension = true,
                AutoUpgradeEnabled = true,
                Filter = "HTML files | *.html"
            };
            if (sf.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sf.FileName, webCleanReader.DocumentText);
            }
        }

        private void btnCleanReaderToTxt_Click(object sender, EventArgs e)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(webCleanReader.DocumentText);
            var stuff = doc.DocumentNode.SelectNodes("//p").Select(para => para.InnerText);
            var sb = new StringBuilder();
            foreach (var str in stuff)
            {
                sb.Append(str);
            }
            var sf = new SaveFileDialog
            {
                Title = "Select File to save HTML content as",
                DefaultExt = "txt",
                AddExtension = true,
                AutoUpgradeEnabled = true,
                Filter = "Text files | *.txt"
            };
            if (sf.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sf.FileName, sb.ToString());
            }
        }

        private void btnCleanReaderConfigure_Click(object sender, EventArgs e)
        {
            webCleanReader.ShowPrintDialog();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            webCleanReader.ShowPrintPreviewDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.UrlHistory.AddRange(_urlArray.Cast<string>().ToArray().Distinct().ToArray());
            Settings.Default.Save();
        }

        private void btnRunScript_Click(object sender, EventArgs e)
        {
            if (txtSourceFolder.Text.Trim().Length == 0)
            {
                Logger.LogE(GetType().FullName + "." + MethodBase.GetCurrentMethod().Name
                            + " Select a valid path");
                MetroMessageBox.Show(this, "Please select a valid folder path", "Error", MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
                return;
            }
            if (txtSeleniumCode.Text.Trim().Length == 0)
            {
                MetroMessageBox.Show(this, "Code editor is empty.\nWrite code or load existing code from a file",
                    "Error", MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
            InternetExplorerDriver driver = null;
            ScriptWorker.RunScript(ref driver, txtSeleniumCode.Lines, txtSourceFolder.Text);
            Process.Start("Explorer.exe", txtSourceFolder.Text);
        }

        private void btnBrowseFolderPath_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourceFolder.Text = dialog.SelectedPath;
                    if (!Directory.Exists(txtSourceFolder.Text))
                    {
                        Directory.CreateDirectory(txtSourceFolder.Text);
                    }
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FileDialog dialog = new OpenFileDialog())
            {
                try
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        txtSeleniumCode.Text = "";
                        txtSeleniumCode.Lines = File.ReadAllLines(dialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogE(GetType().FullName + "." + MethodBase.GetCurrentMethod().Name + " " +
                                ex.Message);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FileDialog dialog = new SaveFileDialog())
            {
                try
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllLines(dialog.FileName, txtSeleniumCode.Lines);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogE(GetType().FullName + "." + MethodBase.GetCurrentMethod().Name + " " +
                                ex.Message);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}