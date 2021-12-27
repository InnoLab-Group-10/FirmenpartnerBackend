using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class BaseModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
