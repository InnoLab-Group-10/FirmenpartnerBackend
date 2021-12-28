namespace FirmenpartnerBackend.Models.Response
{
    public class DeleteResponse : IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
