﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using ScrapEra.CleanReader;
using ScrapEra.Gui.Properties;
using ScrapEra.ScrapLogger;
using ScrapEra.Utils;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace ScrapEra.Gui
{
    public partial class MainForm : MetroForm
    {
        private readonly BindingSource _bs = new BindingSource();
        private Thread _cleanReaderCentralThread;
        private AutoCompleteStringCollection urlArray;
        private string currentUrl;

        public MainForm()
        {
            InitializeComponent();
            _bs.DataSource = Logger.LogList;
            lstLogs.DataSource = _bs;
            PeriodicTaskFactory.Start(() => { _bs.ResetBindings(false); }, 2000, maxIterations: 10);
            //var list =  Properties.Settings.Default.UrlHistory.Cast<string>().ToList();
            urlArray = new AutoCompleteStringCollection ();
            urlArray.AddRange(Properties.Settings.Default.UrlHistory.Cast<String>().ToArray());
            //urlArray.Cast<string>().ToList().ForEach(Console.WriteLine);
            txtCleanReaderUrl.AutoCompleteCustomSource = urlArray;
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
            if (txtCleanReaderUrl.Text.Trim().Length == 0) { return; }
            if (!(txtCleanReaderUrl.Text.Trim().Contains("http://")) && !txtCleanReaderUrl.Text.Trim().Contains("https://"))
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
            currentUrl = txtCleanReaderUrl.Text;
            _cleanReaderCentralThread = new Thread(CleanReaderWorker) {IsBackground = true};
            _cleanReaderCentralThread.Start();
            urlArray.Add(txtCleanReaderUrl.Text);
        }

        public void CleanReaderWorker()
        {
            try
            {
                var tr = new CleanReaderWeb();
                var stuff = tr.Transcode(currentUrl);
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
            }
        }

        private void CleanThread()
        {
            try
            {
                _cleanReaderCentralThread = null;
            }
            catch (Exception)
            {
                // ignored
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
            var sf = new SaveFileDialog();
            sf.Title = "Select File to save HTML content as";
            sf.DefaultExt = "html";
            sf.AddExtension = true;
            sf.AutoUpgradeEnabled = true;
            sf.Filter = "HTML files | *.html";
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
            var sf = new SaveFileDialog();
            sf.Title = "Select File to save HTML content as";
            sf.DefaultExt = "txt";
            sf.AddExtension = true;
            sf.AutoUpgradeEnabled = true;
            sf.Filter = "Text files | *.txt";
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
            Properties.Settings.Default.UrlHistory.AddRange(urlArray.Cast<string>().ToArray());
            Properties.Settings.Default.Save();
        }
    }
}