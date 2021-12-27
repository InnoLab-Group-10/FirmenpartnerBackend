namespace FirmenpartnerBackend.Models.Response
{
    public interface IBaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
