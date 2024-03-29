﻿using System;
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
    public partial class search : Form
    {

        public bool Closed { get; set; }
        public search()
        {
            InitializeComponent();


        }
        public Button ButtonOnSubForm

        {
            get { return this.button1; }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.searchword = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void search_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.searchword;
            if (Properties.Settings.Default.searchup) { radioButton1.Checked = true; radioButton2.Checked = false; }
            else { radioButton1.Checked = false; radioButton2.Checked = true; }
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Properties.Settings.Default.searchup = true;
                Properties.Settings.Default.Save();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                Properties.Settings.Default.searchup = false;
                Properties.Settings.Default.Save();
               
            }
            else
            {
                Properties.Settings.Default.searchup = true;
                Properties.Settings.Default.Save();
               
            }
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                Properties.Settings.Default.bigandsmall = false;
                Properties.Settings.Default.Save();
                MessageBox.Show("On");
            }
            else
            {
                Properties.Settings.Default.bigandsmall = true;
                Properties.Settings.Default.Save();
                MessageBox.Show("Off");
            }
        }

        private void search_FormClosing(object sender, FormClosingEventArgs e)
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
