using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class Contact
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [Required]
        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
    }
}
