using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class Notification : BaseModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string RecipientId { get; set; }

        [Required]
        [ForeignKey(nameof(RecipientId))]
        public ApplicationUser Recipient { get; set; }
    }
}
