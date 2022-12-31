using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDwritercsharp
{
    public partial class settings : Form
    {
        
        public settings()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) Properties.Settings.Default.underbar = false;
            Properties.Settings.Default.Save();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) Properties.Settings.Default.underbar = true;
            
            Properties.Settings.Default.Save();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) Properties.Settings.Default.bold = false;
            
            Properties.Settings.Default.Save();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked) Properties.Settings.Default.bold = true;
            Properties.Settings.Default.Save();
        }
        private void settings_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.underbar) { this.radioButton2.Checked = true; } else { this.radioButton1.Checked = true; }
            if (Properties.Settings.Default.bold) { this.radioButton4.Checked = true; } else { this.radioButton3.Checked = true; }

        }

    }
}
