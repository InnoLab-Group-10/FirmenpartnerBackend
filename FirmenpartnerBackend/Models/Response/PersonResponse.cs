using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Response
{
    public class PersonBaseResponse : ISingleResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public string? Notes { get; set; }
    }

    public class PersonSingleResponse : PersonBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class PersonMultiResponse : IMultiResponse<PersonBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<PersonBaseResponse> Results { get; set; }
    }
}
