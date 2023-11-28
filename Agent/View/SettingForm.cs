using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Agent.Controller;

namespace Agent.View
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        public void SetInitialValue(Config.Data data)
        {
            numInterval.Value = Math.Max(Math.Min(data.historyInterval, numInterval.Maximum), numInterval.Minimum);
            txtPath.Text = data.historyPath;
        }

        public Config.Data GetValue()
        {
            Config.Data data = new Config.Data();
            data.historyInterval = (int)numInterval.Value;
            data.historyPath = txtPath.Text;
            return data;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = browse.SelectedPath;
            }
        }
    }
}
