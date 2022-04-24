using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class NotificationRequest
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public Guid RecipientId { get; set; }
    }
}
