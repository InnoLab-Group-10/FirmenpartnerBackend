using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class CompanyRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool ContractSigned { get; set; }

        public string? Notes { get; set; }
    }
}
