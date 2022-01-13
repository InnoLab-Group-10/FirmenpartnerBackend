using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class CompanyAssignmentRequest
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public Guid PersonId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateOnly From { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? To { get; set; }
    }
}
