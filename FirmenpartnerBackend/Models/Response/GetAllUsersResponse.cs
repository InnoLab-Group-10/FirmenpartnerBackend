namespace FirmenpartnerBackend.Models.Response
{
    public class GetAllUsersResponse : IBaseResponse
    {
        public List<string> Users { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
