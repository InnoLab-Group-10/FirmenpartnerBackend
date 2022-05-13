using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class CompanyAssignment : BaseModel, ISoftDeletable
    {
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

        [DataType(DataType.Date)]
        public DateTime? From { get; set; }

        [DataType(DataType.Date)]
        public DateTime? To { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTimestamp { get; set; }
    }
}
