namespace FirmenpartnerBackend.Models.Response
{
    public class FullCompanyInfoResponse
    {
        public CompanyBaseResponse Company { get; set; }
        public List<CompanyLocationBaseResponse> Locations { get; set; }
        public List<ContactBaseResponse> Contacts { get; set; }
    }

    public class ContactBaseResponse : PersonBaseResponse
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
