using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class FileEntry : BaseModel, ISoftDeletable
    {
        public string Name { get; set; }

        public long Size { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Timestamp { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
