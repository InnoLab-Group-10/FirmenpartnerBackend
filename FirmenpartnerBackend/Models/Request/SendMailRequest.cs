using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class SendMailRequest
    {
        public Guid? Template { get; set; } = null;
        [Required]
        public string Subject { get; set; }
        public List<Guid> Attachments { get; set; }
        public string? MailingList { get; set; } = null;
        public List<string>? AdditionalRecipients { get; set; } = new List<string>();
        public string? AdditionalText { get; set; } = null;
    }
}
