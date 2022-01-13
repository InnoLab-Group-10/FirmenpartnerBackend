using AutoMapper;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;

namespace FirmenpartnerBackend.Mapping
{
    public class RequestToModelProfile : Profile
    {
        public RequestToModelProfile()
        {
            CreateMap<CompanyRequest, Company>();

            CreateMap<CompanyLocationRequest, CompanyLocation>();

            CreateMap<CompanyAssignmentRequest, CompanyAssignment>();

            CreateMap<PersonRequest, Person>();
        }
    }
}
