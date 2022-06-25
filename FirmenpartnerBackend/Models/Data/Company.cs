using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class Company : BaseModel, ISoftDeletable
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public bool ContractSigned { get; set; }

        [Required]
        public int MaxStudents { get; set; }

        public string? Notes { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}