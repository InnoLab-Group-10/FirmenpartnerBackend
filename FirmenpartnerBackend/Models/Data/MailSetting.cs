using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class MailSetting
    {
        [Required, Key]
        public string Id { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
