namespace FirmenpartnerBackend.Models.Response
{
    public class UserBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? Notes { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UserSingleResponse : UserBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class UserMultiResponse : IMultiResponse<UserBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<UserBaseResponse> Results { get; set; }
    }
}
