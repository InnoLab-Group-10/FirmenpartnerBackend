namespace FirmenpartnerBackend.Models.Response
{
    public class TimelineEntryBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        public string? Link { get; set; }
        public string? LinkText { get; set; }
    }

    public class TimelineEntrySingleResponse : TimelineEntryBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class TimelineEntryMultiResponse : IMultiResponse<TimelineEntryBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<TimelineEntryBaseResponse> Results { get; set; }
    }
}
