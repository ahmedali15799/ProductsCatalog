using System.Net;
using System.Net.Mail;
using Project.DAL.Models;

namespace Project.PL.Helper
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("ahmedalielqazaz@gmail.com", "zigtvxrprakpmvwg");
			client.Send("ahmedalielqazaz@gmail.com", email.Receptionists, email.Subject, email.Body);

		}
	}
}
