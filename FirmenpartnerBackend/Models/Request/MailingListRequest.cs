using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class MailingListRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IList<MailingListEntryRequest> Entries { get; set; }
    }
}
