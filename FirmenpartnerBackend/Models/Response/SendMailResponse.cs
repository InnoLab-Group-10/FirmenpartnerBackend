namespace FirmenpartnerBackend.Models.Response
{
    public class SendMailBaseResponse
    {

    }

    public class SendMailSingleResponse : SendMailBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
