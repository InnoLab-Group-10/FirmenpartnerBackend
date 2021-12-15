using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class CompanyLocation
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Address { get; set; }

        [Required]
        [MaxLength(64)]
        public string City { get; set; }

        [Required]
        [MaxLength(12)]
        public string Zipcode { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}
