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

            CreateMap<CompanyLocation, CompanyLocationBaseResponse>();
            CreateMap<CompanyLocation, CompanyLocationSingleResponse>();

            CreateMap<CompanyAssignment, CompanyAssignmentBaseResponse>();
            CreateMap<CompanyAssignment, CompanyAssignmentSingleResponse>();

            CreateMap<Person, PersonBaseResponse>();
            CreateMap<Person, PersonSingleResponse>();

            CreateMap<PersonBaseResponse, ContactBaseResponse>();
        }
    }
}
