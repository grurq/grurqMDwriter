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
    public partial class replace : Form
    {
        public bool Closed { get; set; }

        public replace()
        {
            InitializeComponent();
        }
        public Button ButtonOnSubForm1

        {
            get { return this.button1; }
        }
        public Button ButtonOnSubForm2

        {
            get { return this.button2; }
        }
        public Button ButtonOnSubForm3

        {
            get { return this.button3; }
        }


        private void replace_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.searchword;
            textBox2.Text = Properties.Settings.Default.replaceword;
            if (!Properties.Settings.Default.bigandsmall) { checkBox1.Checked = true; } else { checkBox1.Checked = false; }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.bigandsmall = false;
            }
            else
            {
                Properties.Settings.Default.bigandsmall = true;
            }
            Properties.Settings.Default.Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.searchword = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.replaceword = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void replace_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Closed)
            {
                e.Cancel = true;
                Form form = (Form)sender;
                form.Visible = false;
            }

        }
    }
}
