namespace FirmenpartnerBackend.Models.Response
{
    public class NotificationBaseResponse : ISingleResponse
    {
        public Guid? Id { get; set; }
        public string Message { get; set; }

        public Guid RecipientId { get; set; }
    }

    public class NotificationSingleResponse : NotificationBaseResponse, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class NotificationMultiResponse : IMultiResponse<NotificationBaseResponse>, IResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<NotificationBaseResponse> Results { get; set; }
    }
}
