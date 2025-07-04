using GroupA_Manager_Api.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GroupA_Manager_Api.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IManagerRepository<MailData> _repository;
        public MailService(IOptions<MailSettings> mailSettings,IManagerRepository<MailData> repository)
        {
            _mailSettings = mailSettings.Value;
            _repository = repository;
        }
        public async Task TestEmail()
        {
            string MailText = "Test Mail Sending";
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse("arbaazfb26@gmail.com"));
            email.Subject = "Testing Mail Api";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendEmailAsync(int EnquiryId) 
        {
            string FilePath = Directory.GetCurrentDirectory() + "/Template/MailTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            var email = new MimeMessage();
            var maildetails = _repository.ApproveEnquiry(EnquiryId);
            MailText = MailText.Replace("[username]", maildetails.userName).Replace("[password]", maildetails.password);
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(maildetails.ToEmail));
            email.Subject = "Congradulations your account has been approved";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendRejectedMaiAsync(int EnquiryId, string feedback)
        {
            var email = new MimeMessage();
            var rejectedMail = _repository.RejectEnquiry(EnquiryId, feedback);
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(rejectedMail));
            email.Subject = "Sorry, your account has been rejected";
            var builder = new BodyBuilder();
            builder.HtmlBody = $"Sorry your enquiry has been rejected.<br>Feedback: <strong>{feedback}</strong>";
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
