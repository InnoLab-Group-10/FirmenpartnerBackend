using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class Company : BaseModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public bool ContractSigned { get; set; }

        [Required]
        public int MaxStudents { get; set; }

        public string? Notes { get; set; }

    }
}