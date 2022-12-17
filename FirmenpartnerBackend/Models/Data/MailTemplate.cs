using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class MailTemplate : BaseModel, ISoftDeletable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
