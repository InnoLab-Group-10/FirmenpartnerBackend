namespace FirmenpartnerBackend.Models.Response
{
    public class FullCompanyInfoResponse
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
