namespace FirmenpartnerBackend.Models.Response
{
    public class ChangePasswordResponse : IBaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
