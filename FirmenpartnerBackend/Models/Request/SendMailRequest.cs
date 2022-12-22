using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class SendMailRequest
    {
        [Required]
        public Guid Template { get; set; }
        [Required]
        public string Subject { get; set; }
        public List<Guid> Attachments { get; set; }
    }
}
