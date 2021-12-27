namespace FirmenpartnerBackend.Models.Response
{
    public class BaseMultiResponse<T> where T : BaseSingleResponse
    {
        public List<T> Results { get; set; }
    }
}
