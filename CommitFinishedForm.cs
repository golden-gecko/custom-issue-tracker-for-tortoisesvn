using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomIssueTracker
{
	public partial class CommitFinishedForm : Form
	{
		public CommitFinishedForm( List<TicketItem> selectedTickets )
		{
			InitializeComponent( );
		}

        private void timer1_Tick(object sender, EventArgs e)
        {
            button4_Click(button4, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            Close();
        }

		private void CommitFinishedForm_Load(object sender, EventArgs e)
		{

		}
	}
}
