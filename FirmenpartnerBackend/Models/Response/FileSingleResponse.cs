namespace FirmenpartnerBackend.Models.Response
{
    public class FileBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid OwnerId { get; set; }
    }

    public class FileSingleResponse : FileBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class FileMultiResponse : IMultiResponse<FileBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<FileBaseResponse> Results { get; set; }
    }
}
