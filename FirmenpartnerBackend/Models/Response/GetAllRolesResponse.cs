namespace FirmenpartnerBackend.Models.Response
{
    public class GetAllRolesResponse : IBaseResponse
    {
        public List<string> Roles { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
