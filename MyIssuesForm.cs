using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CustomIssueTracker
{
	/**
	 * @author Wojciech Holisz <wojciech.holisz@gmail.com>
	 */
    partial class MyIssuesForm : Form
    {
        private readonly IEnumerable<TicketItem> _tickets;
        private readonly List<TicketItem> _ticketsAffected = new List<TicketItem>();

        public MyIssuesForm(IEnumerable<TicketItem> tickets)
        {
            InitializeComponent();
            _tickets = tickets;
        }

        public IEnumerable<TicketItem> TicketsFixed
        {
            get { return _ticketsAffected; }
        }

        private void MyIssuesForm_Load(object sender, EventArgs e)
        {
            Hashtable icons = new Hashtable()
            {
                { "nie rozpoczete", 0 },
                { "w trakcie", 1 },
                { "zawieszone", 2 },
                { "wykonane", 3 },
                { "konsultacja", 4 }
            };

            foreach(TicketItem ticketItem in _tickets)
            {
                ListViewItem lvi = new ListViewItem("");

                lvi.SubItems.Add(ticketItem.Number.ToString());
                lvi.SubItems.Add(ticketItem.Status);
                lvi.SubItems.Add(ticketItem.Client);
                lvi.SubItems.Add(ticketItem.Project);
                lvi.SubItems.Add(ticketItem.Title);

                lvi.ImageIndex = (int)icons[ticketItem.Status];
                lvi.Tag = ticketItem;
                
                if (ticketItem.Status.Equals("w trakcie") == true)
                {
                    lvi.Checked = true;
                }

                listView1.Items.Add(lvi);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                TicketItem ticketItem = lvi.Tag as TicketItem;

                if (ticketItem != null && lvi.Checked)
                {
                    _ticketsAffected.Add(ticketItem);
                }
            }
        }

		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
			listView1.Sort();
		}
    }
}
