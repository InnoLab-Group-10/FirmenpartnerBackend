using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class Student : Person
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public int Semester { get; set; }

        [Required]
        public Guid ProgramId { get; set; }

        [Required]
        [ForeignKey(nameof(ProgramId))]
        public Program Program { get; set; }
    }
}
