namespace FirmenpartnerBackend.Models.Response
{
    public class MailSettingBaseResponse
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public class MailSettingSingleResponse : MailSettingBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
