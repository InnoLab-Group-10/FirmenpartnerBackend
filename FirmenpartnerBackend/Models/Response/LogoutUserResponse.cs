namespace FirmenpartnerBackend.Models.Response
{
    public class LogoutUserResponse : IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
