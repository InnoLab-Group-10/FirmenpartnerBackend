using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Data
{
    public class Person
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; }

        [MaxLength(32)]
        public string? Prefix { get; set; }

        [MaxLength(32)]
        public string? Suffix { get; set; }
    }
}
