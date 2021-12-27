namespace FirmenpartnerBackend.Models.Response
{
    public class LogoutUserResponse : IBaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
