namespace FirmenpartnerBackend.Models.Response
{
    public class AssignRoleResponse : IBaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
