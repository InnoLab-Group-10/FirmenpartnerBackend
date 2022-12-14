using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class Person : BaseModel, ISoftDeletable
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public string? Notes { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
