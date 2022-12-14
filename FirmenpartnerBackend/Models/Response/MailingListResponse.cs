namespace FirmenpartnerBackend.Models.Response
{
    public class MailingListBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IList<MailingListEntryResponse> Entries { get; set; }
    }

    public class MailingListSingleResponse : MailingListBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class MailingListMultiResponse : IMultiResponse<MailingListBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<MailingListBaseResponse> Results { get; set; }
    }
}
