namespace FirmenpartnerBackend.Models.Response
{
    public class ProgramBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }

    public class ProgramSingleResponse : ProgramBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ProgramMultiResponse : IMultiResponse<ProgramBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<ProgramBaseResponse> Results { get; set; }
    }
}
