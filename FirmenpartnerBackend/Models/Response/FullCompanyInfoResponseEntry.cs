namespace FirmenpartnerBackend.Models.Response
{
    public class FullCompanyInfoResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<FullCompanyInfoResponseEntry> Results { get; set; }
    }

    public class FullCompanyInfoResponseEntry
    {
        public CompanyBaseResponse Company { get; set; }
        public List<CompanyGetAllLocationBaseResponse> Locations { get; set; }
        public List<CompanyGetAllContactAssignmentBaseResponse> Contacts { get; set; }
    }

    public class CompanyGetAllLocationBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }

    public class CompanyGetAllContactAssignmentBaseResponse : ContactBaseResponse
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
