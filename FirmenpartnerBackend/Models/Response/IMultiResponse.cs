namespace FirmenpartnerBackend.Models.Response
{
    public interface IMultiResponse<T> where T : ISingleResponse
    {
        public List<T> Results { get; set; }
    }
}
