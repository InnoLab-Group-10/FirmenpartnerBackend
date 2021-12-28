namespace FirmenpartnerBackend.Models.Response
{
    public class GetAllUsersResponse : IResponse
    {
        public List<string> Users { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
