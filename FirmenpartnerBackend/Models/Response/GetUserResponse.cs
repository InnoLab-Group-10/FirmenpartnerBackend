namespace FirmenpartnerBackend.Models.Response
{
    public class GetUserResponse : IBaseResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
