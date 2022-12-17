using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class SendMailRequest
    {
        [Required]
        public Guid Template { get; set; }
        [Required]
        public Guid MailingList { get; set; }
    }
}
