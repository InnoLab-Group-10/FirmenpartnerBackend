using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class CsvRequest
    {
        [Required]
        public string Csv { get; set; }
    }
}
