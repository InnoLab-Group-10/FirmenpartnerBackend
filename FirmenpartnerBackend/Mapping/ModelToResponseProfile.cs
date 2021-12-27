using AutoMapper;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Response;

namespace FirmenpartnerBackend.Mapping
{
    public class ModelToResponseProfile : Profile
    {
        public ModelToResponseProfile()
        {
            CreateMap<Company, CompanyBaseResponse>();
            CreateMap<Company, CompanySingleResponse>();
        }
    }
}
