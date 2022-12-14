using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class MailingListEntryRequest
    {
        [Required]
        public string Mail { get; set; }
        public string? Note { get; set; }
    }
}
