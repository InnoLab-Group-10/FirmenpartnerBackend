namespace FirmenpartnerBackend.Models.Response
{
    public class ContactBaseResponse : PersonBaseResponse
    {

    }

    public class ContactSingleResponse : ContactBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ContactMultiResponse : IMultiResponse<ContactBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<ContactBaseResponse> Results { get; set; }
    }
}
