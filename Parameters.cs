using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Xml;

namespace CustomIssueTracker
{
    class Parameters
    {
        public Parameters(string filename)
        {
			doc.Load(filename);

			/*
			XmlNodeList tasks = doc.SelectNodes("//zadanie");

            char[] trimChars = new char[] { '"' };
            char[] splitChars = new char[] { ' ' };

            foreach (string param in parameters.Split(splitChars, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] pair = param.Split('=');

                this.parameters.Add(
                    pair[0].Replace("--", ""),
                    pair[1].Trim(trimChars));
            }
			*/
        }

		public string get(string key)
		{
			return doc.SelectSingleNode(key).InnerText;
		}

		public List<string> getList(string key)
        {
			List<string> settings = new List<string>();
			XmlNodeList tasks = doc.SelectNodes(key);

			foreach (XmlNode node in tasks)
			{
				settings.Add(node.InnerText);
			}

            return settings;
        }

		protected XmlDocument doc = new XmlDocument();
    }
}
