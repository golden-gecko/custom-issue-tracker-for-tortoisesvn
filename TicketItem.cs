namespace CustomIssueTracker
{
	/**
	 * @author Wojciech Holisz <wojciech.holisz@gmail.com>
	 */
    public class TicketItem
    {
        public int Number;
        public string Status;
        public string Client;
        public string Project;
        public string Title;
        public string Url;

        public TicketItem(int number, string status, string client, string project, string title, string url)
        {
            Number = number;
            Status = status;
            Client = client;
            Project = project;
            Title = title;
            Url = url;
        }
    }
}
