namespace FirmenpartnerBackend.Models.Response
{
    public class StudentCountBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public Guid CompanyId { get; set; }
    }

    public class StudentCountSingleResponse : StudentCountBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class StudentCountMultiResponse : IMultiResponse<StudentCountBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<StudentCountBaseResponse> Results { get; set; }
    }
}
