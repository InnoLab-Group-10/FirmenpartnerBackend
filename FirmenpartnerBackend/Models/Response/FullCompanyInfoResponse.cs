namespace FirmenpartnerBackend.Models.Response
{
    public class FullCompanyInfoResponse
    {
        public CompanyBaseResponse Company { get; set; }
        public List<CompanyLocationBaseResponse> Locations { get; set; }
        public List<ContactAssignmentBaseResponse> Contacts { get; set; }
    }

    public class ContactAssignmentBaseResponse : ContactBaseResponse
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
