using System;
using MetroFramework.Forms;
using ScrapEra.Gui.Properties;

namespace ScrapEra.Gui
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += " " + Settings.Default.Version;
        }
    }
}