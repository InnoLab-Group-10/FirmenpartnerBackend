namespace FirmenpartnerBackend.Service
{
    public interface ITemplateMailService
    {
        string GetMailHtml(string body, IEnumerable<(Guid guid, string name)> attachments);
        void SendMail(string subject, string body, IEnumerable<MailRecipient> recipients);
    }

    public record MailRecipient(string? Prefix, string FirstName, string LastName, string? Suffix, string Email);
}