using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Models.Data;
using NETCore.MailKit.Core;
using System.Text;

namespace FirmenpartnerBackend.Service
{
    public class TemplateMailService : ITemplateMailService
    {
        private readonly IEmailService emailService;
        private readonly IMailSettingsService mailSettingsService;
        private readonly MailConfig mailConfig;
        private readonly FileUploadConfig fileUploadConfig;

        public TemplateMailService(IMailSettingsService mailSettingsService, IEmailService emailService, MailConfig mailConfig, FileUploadConfig fileUploadConfig)
        {
            this.mailSettingsService = mailSettingsService;
            this.emailService = emailService;
            this.mailConfig = mailConfig;
            this.fileUploadConfig = fileUploadConfig;
        }

        public void SendMail(string subject, string body, IEnumerable<MailRecipient> recipients)
        {
            foreach (MailRecipient recipient in recipients)
            {
                emailService.Send(recipient.Email, subject, body, true);
            }
        }

        public string GetMailHtml(MailTemplate template, IEnumerable<(Guid guid, string name)> attachments)
        {
            string baseHtml = File.ReadAllText(mailConfig.TemplatePath);

            string headerBackgroundImageId = mailSettingsService.GetSetting(MailSettings.HEADER_BG_IMAGE).Result.Value;
            string headerLogoImageId = mailSettingsService.GetSetting(MailSettings.HEADER_LOGO).Result.Value;

            string? headerBackgroundImageUrl = headerBackgroundImageId == "" ? null : Path.Combine(fileUploadConfig.HostingPath, headerBackgroundImageId);
            string? headerLogoImageUrl = headerLogoImageId == "" ? null : Path.Combine(fileUploadConfig.HostingPath, headerLogoImageId);

            string headerBackgroundColor = mailSettingsService.GetSetting(MailSettings.HEADER_BG_COLOR).Result.Value;
            string bodyBackgroundColor = mailSettingsService.GetSetting(MailSettings.BODY_BG_COLOR).Result.Value;
            string bodyColor = mailSettingsService.GetSetting(MailSettings.BODY_COLOR).Result.Value;
            string footerBackgroundColor = mailSettingsService.GetSetting(MailSettings.FOOTER_BG_COLOR).Result.Value;
            string footerColor = mailSettingsService.GetSetting(MailSettings.FOOTER_COLOR).Result.Value;
            string footerText = mailSettingsService.GetSetting(MailSettings.FOOTER_TEXT).Result.Value;
            string mailBackgroundColor = mailSettingsService.GetSetting(MailSettings.MAIL_BG_COLOR).Result.Value;

            string bodyText = template.Content;

            StringBuilder attachmentListBuilder = new StringBuilder();
            if (attachments.Count() > 0)
            {
                attachmentListBuilder.Append("<ul>");
                foreach (var attachment in attachments)
                {
                    string url = Path.Combine(fileUploadConfig.HostingPath, attachment.guid.ToString());
                    attachmentListBuilder.Append($"<li><a href=\"{url}\">{attachment.name}</a></li>");
                }
                attachmentListBuilder.Append("</ul>");
            }

            return baseHtml
                .Replace("%MAIL_BG_COLOR%", mailBackgroundColor)
                .Replace("%HEADER_BG_COLOR%", headerBackgroundColor)
                .Replace("%HEADER_BG_IMAGE%", headerBackgroundImageUrl ?? "")
                .Replace("%HEADER_BG_IMAGE_HIDDEN%", headerBackgroundImageUrl == null ? "hidden" : "")
                .Replace("%BODY_BG_COLOR%", bodyBackgroundColor)
                .Replace("%BODY_COLOR%", bodyColor)
                .Replace("%FOOTER_BG_COLOR%", footerBackgroundColor)
                .Replace("%FOOTER_COLOR%", footerColor)
                .Replace("%HEADER_LOGO%", headerLogoImageUrl ?? "")
                .Replace("%HEADER_LOGO_HIDDEN%", headerLogoImageUrl == null ? "hidden" : "")
                .Replace("%BODY_TEXT%", bodyText)
                .Replace("%ATTACHMENTS%", attachmentListBuilder.ToString())
                .Replace("%ATTACHMENTS_HIDDEN%", attachments.Count() == 0 ? "hidden" : "")
                .Replace("%FOOTER_TEXT%", footerText);
        }
    }
}
