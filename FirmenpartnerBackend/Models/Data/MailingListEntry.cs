using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class MailingListEntry : BaseModel, ISoftDeletable
    {
        public string? Prefix { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Company { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
