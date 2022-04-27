namespace FirmenpartnerBackend.Models.Response
{
    public class CsvResponse : IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
