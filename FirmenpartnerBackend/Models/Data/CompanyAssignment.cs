using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class CompanyAssignment
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [Required]
        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly From { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? To { get; set; }
    }
}
