using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class Program : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
