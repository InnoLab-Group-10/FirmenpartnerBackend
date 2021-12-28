namespace FirmenpartnerBackend.Models.Response
{
    public interface IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
