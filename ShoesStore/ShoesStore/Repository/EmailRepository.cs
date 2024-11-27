using ShoesStore.IRepository;
using System.Net;
using System.Net.Mail;
using System.Net.WebSockets;

namespace ShoesStore.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration configuration;

        public EmailRepository(IConfiguration configuration) {
            this.configuration  = configuration;
        }
        public async Task SendEmail(string receptor, string subject, string body)
        {
            var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var smtpClient = new SmtpClient(host,port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials  =new NetworkCredential(email, password);

			var message = new MailMessage(email!, receptor, subject, body)
			{
				IsBodyHtml = true
			};
			await smtpClient.SendMailAsync(message);


        }
    }
}
