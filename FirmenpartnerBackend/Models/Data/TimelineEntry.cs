using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class TimelineEntry : BaseModel, ISoftDeletable
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Timestamp { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public string Message { get; set; }

        public string? Link { get; set; }

        public string? LinkText { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
