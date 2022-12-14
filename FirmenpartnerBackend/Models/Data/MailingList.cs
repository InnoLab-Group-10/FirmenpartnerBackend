using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class MailingList : BaseModel, ISoftDeletable
    {
        [Required]
        public string Name { get; set; }
        public IList<MailingListEntry> Entries { get; set; } = new List<MailingListEntry>();
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
