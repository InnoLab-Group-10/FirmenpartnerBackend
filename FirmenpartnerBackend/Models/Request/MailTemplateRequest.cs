using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class MailTemplateRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
