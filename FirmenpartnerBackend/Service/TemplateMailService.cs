using FirmenpartnerBackend.Models.Data;
using MimeKit;
using NETCore.MailKit;
using NETCore.MailKit.Core;

namespace FirmenpartnerBackend.Service
{
    public class TemplateMailService : ITemplateMailService
    {
        private readonly IMailKitProvider mailKitProvider;
        private readonly IMailSettingsService mailSettingsService;

        public TemplateMailService(IEmailService emailService, IMailSettingsService mailSettingsService, IMailKitProvider mailKitProvider)
        {
            this.mailSettingsService = mailSettingsService;
            this.mailKitProvider = mailKitProvider;
        }

        public void SendMail(string subject, MailTemplate template, IEnumerable<(string path, string name)> attachments, IEnumerable<MailRecipient> recipients)
        {
            MimeMessage message = new MimeMessage();

            foreach (MailRecipient recipient in recipients)
            {
                message.Cc.Add(new MailboxAddress(recipient.Name, recipient.Address));
            }

            message.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = template.Content;

            // TODO add header, footer, logo etc specified in mail settings

            foreach (var attachment in attachments)
            {
                byte[] bytes = File.ReadAllBytes(attachment.path);
                builder.Attachments.Add(attachment.name, bytes);
            }

            message.Body = builder.ToMessageBody();

            using (var client = mailKitProvider.SmtpClient)
            {
                client.Send(message);
            }
        }
    }
}
