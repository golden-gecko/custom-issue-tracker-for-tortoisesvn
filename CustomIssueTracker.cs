using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Interop.BugTraqProvider;
using System.Net;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace CustomIssueTracker
{
	/**
	 * @author Wojciech Holisz <wojciech.holisz@gmail.com>
	 */
    [ComVisible(true), Guid("5870B3F1-8393-4c83-ACED-1D5E803A4F2B"), ClassInterface(ClassInterfaceType.None)]
    public class CustomIssueTracker : IBugTraqProvider2
    {
        public string CheckCommit(IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList, string commitMessage)
        {
			// Pobierz parametry.
			Parameters p = new Parameters(parameters);


            // Check file list.
			List<string> warnOnCommit = p.getList("//warnings/files/name");
			string warningMessage = "";

            foreach (string path in pathList)
            {
                string root = commonRoot.Replace("\\", "/");
				string name = path.Replace(root, "");

				foreach (string file in warnOnCommit)
				{
					if (name.Contains(file))
					{
						warningMessage += "\n" + name;
					}
				}
            }

            if (warningMessage.Length > 0)
            {
				DialogResult result = MessageBox.Show("Czy na pewno chcesz zmodyfikować te pliki?\n" + warningMessage,
					"TortoiseSVN", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (result == DialogResult.No)
				{
					return "Usuń pliki z rewizji.";
				}
            }


			// Check commit message.
			int minMessageLength = int.Parse(p.get("//warnings/message/length"));
            
			if (commitMessage.Length < minMessageLength)
            {
                return string.Format("Uzupełnij komentarz. Minimalna długość: {0}.", minMessageLength);
            }

            return "";
        }

        public string GetCommitMessage(IntPtr hParentWnd, string parameters, string commonRoot, string[] pathList, string originalMessage)
        {
            string[] revPropNames = new string[0];
            string[] revPropValues = new string[0];
            string dummystring = "";

            return GetCommitMessage2(hParentWnd, parameters, "", commonRoot, pathList, originalMessage, "", out dummystring, out revPropNames, out revPropValues);
        }

        public string GetCommitMessage2(IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList, string originalMessage, string bugID, out string bugIDOut, out string[] revPropNames, out string[] revPropValues)
        {
            try
            {
                List<TicketItem> tickets = new List<TicketItem>();

                // Pobierz parametry.
                Parameters p = new Parameters(parameters);

                // Przygotuj żądanie, którze pobierze listę zadań.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(p.get("//tracker/url"));

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                string data = string.Format("login={0}&pass={1}", p.get("//tracker/user"), p.get("//tracker/password"));
                byte[] bytes = Encoding.ASCII.GetBytes(data);

                request.ContentLength = bytes.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                // Pobierz odpowiedź (listę zadań) w formacie XML.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    string s = reader.ReadToEnd();

                    // Dodaj zadania do listy.
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(s);

                    XmlNodeList tasks = doc.SelectNodes("//zadanie");

                    for (int i = 0; i < tasks.Count; ++i)
                    {
                        int number = int.Parse(tasks[i].SelectSingleNode("numer").InnerText);
                        string client = tasks[i].SelectSingleNode("klient").InnerText;
                        string project = tasks[i].SelectSingleNode("projekt").InnerText;
                        string title = tasks[i].SelectSingleNode("tytul").InnerText;
                        string url = tasks[i].SelectSingleNode("link").InnerText;
                        string status = tasks[i].SelectSingleNode("aktywne").InnerText;

                        TicketItem ticketItem = new TicketItem(number, status, client, project, title, url);

                        if (status.Equals("w trakcie") == true)
                        {
                            tickets.Insert(0, ticketItem);
                        }
                        else
                        {
                            tickets.Add(ticketItem);
                        }
                    }
                }

                bugIDOut = "";
                revPropValues = null;
                revPropNames = null;

                MyIssuesForm form = new MyIssuesForm(tickets);

                if (form.ShowDialog() != DialogResult.OK)
                {
                    return originalMessage;
                }

                StringBuilder result = new StringBuilder(originalMessage);

                if (originalMessage.Length != 0 && !originalMessage.EndsWith("\n"))
                {
                    result.AppendLine();
                }

                foreach (TicketItem ticket in form.TicketsFixed)
                {
                    result.AppendLine(string.Format("{0}  -  {1}", ticket.Title, ticket.Url));
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                throw;
            }
        }

        public string GetLinkText(IntPtr hParentWnd, string parameters)
        {
            return "Choose Task";
        }

        public bool HasOptions()
        {
            return true;
        }

        public string OnCommitFinished(IntPtr hParentWnd, string commonRoot, string[] pathList, string logMessage, int revision)
        {
            return "";
        }

        public string ShowOptionsDialog(IntPtr hParentWnd, string parameters)
        {
            OptionsForm form = new OptionsForm(parameters);

            // Jeżeli anulowano, to zwróć poprzednie parametry.
            if (form.ShowDialog() == DialogResult.Cancel)
            {
                return parameters;
            }

            // Jeżeli OK, to zwróć nowe parametry.
			return form.textBox4.Text;
        }

        public bool ValidateParameters(IntPtr hParentWnd, string parameters)
        {
            return true;
        }
    }
}
