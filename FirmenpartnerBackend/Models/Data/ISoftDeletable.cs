namespace FirmenpartnerBackend.Models.Data
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTimestamp { get; set; }
    }
}
