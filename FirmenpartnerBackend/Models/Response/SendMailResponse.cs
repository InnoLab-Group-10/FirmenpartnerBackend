namespace FirmenpartnerBackend.Models.Response
{
    public class SendMailBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string MailingListName { get; set; }
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class SendMailSingleResponse : SendMailBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class SendMailMultiResponse : IMultiResponse<SendMailBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<SendMailBaseResponse> Results { get; set; }
    }
}
