using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomIssueTracker
{
    public partial class OptionsForm : Form
    {
        public OptionsForm(string parameters)
        {
            InitializeComponent();

			textBox4.Text = parameters;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

		private void OptionsForm_Load(object sender, EventArgs e)
		{

		}
    }
}
