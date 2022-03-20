using AutoMapper;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;

namespace FirmenpartnerBackend.Mapping
{
    public class RequestToModelProfile : Profile
    {
        public RequestToModelProfile()
        {
            CreateMap<CompanyRequest, Company>().ReverseMap();

            CreateMap<CompanyLocationRequest, CompanyLocation>().ReverseMap();

            CreateMap<CompanyAssignmentRequest, CompanyAssignment>().ReverseMap();

            CreateMap<PersonRequest, Person>().ReverseMap();
        }
    }
}
