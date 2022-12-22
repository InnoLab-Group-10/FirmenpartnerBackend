using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class MailingListEntryRequest
    {
        public string? Prefix { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Suffix { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
