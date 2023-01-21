using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class StudentCount : BaseModel, ISoftDeletable
    {
        public int Year { get; set; }
        public int Count { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
