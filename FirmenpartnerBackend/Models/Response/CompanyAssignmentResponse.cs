using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmenpartnerBackend.Models.Response
{
    public class CompanyAssignmentBaseResponse : ISingleResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid PersonId { get; set; }

        public DateOnly From { get; set; }

        public DateOnly? To { get; set; }
    }

    public class CompanyAssignmentSingleResponse : CompanyAssignmentBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class CompanyAssignmentMultiResponse : IMultiResponse<CompanyAssignmentBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<CompanyAssignmentBaseResponse> Results { get; set; }
    }
}
