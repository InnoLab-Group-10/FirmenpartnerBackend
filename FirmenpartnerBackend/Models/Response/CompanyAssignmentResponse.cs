namespace FirmenpartnerBackend.Models.Response
{
    public class CompanyAssignmentBaseResponse : ISingleResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid PersonId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
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
