using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class CompanyAssignmentRequest
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public Guid PersonId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? From { get; set; }

        [DataType(DataType.Date)]
        public DateTime? To { get; set; }
    }
}
