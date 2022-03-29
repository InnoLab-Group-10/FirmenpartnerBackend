using FirmenpartnerBackend.Models.Data;

namespace FirmenpartnerBackend.Models.Response
{
    public class CompanyLocationBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Zipcode { get; set; }

        public Company Company { get; set; }
    }

    public class CompanyLocationSingleResponse : CompanyLocationBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class CompanyLocationMultiResponse : IMultiResponse<CompanyLocationBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<CompanyLocationBaseResponse> Results { get; set; }
    }
}
