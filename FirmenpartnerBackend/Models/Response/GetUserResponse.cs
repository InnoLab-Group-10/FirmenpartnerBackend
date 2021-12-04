namespace FirmenpartnerBackend.Models.Response
{
    public class GetUserResponse : BaseResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
