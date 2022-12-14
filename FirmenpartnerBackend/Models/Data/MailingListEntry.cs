using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class MailingListEntry : BaseModel, ISoftDeletable
    {
        [Required]
        public string Mail { get; set; }
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
