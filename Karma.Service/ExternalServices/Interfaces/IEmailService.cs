using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Karma.Service.ExternalServices.Interfaces
{
	public interface IEmailService
	{
		public  Task SendEmail(string to, string subject, string body);
		
	}
}

