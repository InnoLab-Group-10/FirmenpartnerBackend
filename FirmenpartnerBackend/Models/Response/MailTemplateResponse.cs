namespace FirmenpartnerBackend.Models.Response
{
    public class MailTemplateBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class MailTemplateSingleResponse : MailTemplateBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class MailTemplateMultiResponse : IMultiResponse<MailTemplateBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<MailTemplateBaseResponse> Results { get; set; }
    }
}
