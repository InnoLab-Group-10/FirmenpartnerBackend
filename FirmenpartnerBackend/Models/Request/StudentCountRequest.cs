using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class StudentCountRequest
    {
        public int Year { get; set; }
        public int Count { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
    }
}
