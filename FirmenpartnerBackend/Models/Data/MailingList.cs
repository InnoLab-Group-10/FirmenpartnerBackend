using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class MailingList : BaseModel, ISoftDeletable
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<Person> Recipients { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
