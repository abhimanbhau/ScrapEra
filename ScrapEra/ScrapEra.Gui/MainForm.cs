using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
            var tr = new CleanReaderWeb();
            bool success;
            var stuff = tr.Transcode(txtCleanReaderUrl.Text, out success);
            webCleanReader.DocumentText = stuff;
        }

        private void btnCleanReaderHelp_Click(object sender, EventArgs e)
        {
        }
    }
}