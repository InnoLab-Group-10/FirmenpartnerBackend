namespace FirmenpartnerBackend.Models.Response
{
    public class CompanyBaseResponse : BaseSingleResponse
    {
        public string Name { get; set; }
        public bool ContractSigned { get; set; }
    }

    public class CompanySingleResponse : CompanyBaseResponse, IBaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class CompanyMultiResponse : BaseMultiResponse<CompanyBaseResponse>, IBaseResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
