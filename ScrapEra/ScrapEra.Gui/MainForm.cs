using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using ScrapEra.CleanReader;
using ScrapEra.Gui.Properties;
using ScrapEra.ScrapLogger;
using ScrapEra.Utils;

namespace ScrapEra.Gui
{
    public partial class MainForm : MetroForm
    {
        private readonly BindingSource _bs = new BindingSource();
        private Thread _cleanReaderCentralThread;

        public MainForm()
        {
            InitializeComponent();
            _bs.DataSource = Logger.LogList;
            lstLogs.DataSource = _bs;
            PeriodicTaskFactory.Start(() => { _bs.ResetBindings(false); }, 2000, maxIterations: 10);
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
            if (_cleanReaderCentralThread != null)
            {
                MetroMessageBox.Show(this,
                    "Please Wait..\n\nAlready processing previous request",
                    "Already in progress",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            _cleanReaderCentralThread = new Thread(CleanReaderWorker) {IsBackground = true};
            _cleanReaderCentralThread.Start();
        }

        public void CleanReaderWorker()
        {
            try
            {
                var tr = new CleanReaderWeb();
                var stuff = tr.Transcode(txtCleanReaderUrl.Text);
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
    }
}