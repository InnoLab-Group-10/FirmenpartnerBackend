using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class TimelineEntryRequest
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
    }
}
