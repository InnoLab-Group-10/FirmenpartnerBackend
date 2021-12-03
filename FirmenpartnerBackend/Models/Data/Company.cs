namespace FirmenpartnerBackend.Models.Data
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public bool ContractSigned { get; set; }
    }
}
