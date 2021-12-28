using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class CompanyRequest
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        public bool ContractSigned { get; set; }
    }
}
