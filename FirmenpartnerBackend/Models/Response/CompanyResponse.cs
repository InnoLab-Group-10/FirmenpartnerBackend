namespace FirmenpartnerBackend.Models.Response
{
    public class CompanyBaseResponse : ISingleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool ContractSigned { get; set; }

        public string? Notes { get; set; }
    }

    public class CompanySingleResponse : CompanyBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class CompanyMultiResponse : IMultiResponse<CompanyBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<CompanyBaseResponse> Results { get; set; }
    }
}
