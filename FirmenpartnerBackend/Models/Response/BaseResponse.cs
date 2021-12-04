namespace FirmenpartnerBackend.Models.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
