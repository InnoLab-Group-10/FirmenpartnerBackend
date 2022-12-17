using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class MailSettingRequest
    {
        [Required]
        public string Value { get; set; }
    }
}
