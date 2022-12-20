using FirmenpartnerBackend.Models.Data;

namespace FirmenpartnerBackend.Service
{
    public interface ITemplateMailService
    {
        void SendMail(string subject, MailTemplate template, IEnumerable<(string path, string name)> attachments, IEnumerable<MailRecipient> recipients);
    }

    public record MailRecipient(string Name, string Address);
}