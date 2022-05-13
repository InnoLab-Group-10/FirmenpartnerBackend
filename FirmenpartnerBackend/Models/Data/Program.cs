using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class Program : BaseModel, ISoftDeletable
    {
        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTimestamp { get; set; }
    }
}
