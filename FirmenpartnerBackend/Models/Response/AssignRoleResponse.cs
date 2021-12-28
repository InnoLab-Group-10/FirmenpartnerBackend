namespace FirmenpartnerBackend.Models.Response
{
    public class AssignRoleResponse : IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
