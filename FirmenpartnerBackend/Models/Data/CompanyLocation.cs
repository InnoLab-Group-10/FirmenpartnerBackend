﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmenpartnerBackend.Models.Data
{
    public class CompanyLocation : BaseModel, ISoftDeletable
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Zipcode { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTimestamp { get; set; }
    }
}
