using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class Company : BaseModel
    {

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        public bool ContractSigned { get; set; }
    }
}