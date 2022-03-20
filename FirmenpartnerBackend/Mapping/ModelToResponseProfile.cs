﻿using AutoMapper;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Response;

namespace FirmenpartnerBackend.Mapping
{
    public class ModelToResponseProfile : Profile
    {
        public ModelToResponseProfile()
        {
            CreateMap<Company, CompanyBaseResponse>().ReverseMap();
            CreateMap<Company, CompanySingleResponse>().ReverseMap();

            CreateMap<CompanyLocation, CompanyLocationBaseResponse>().ReverseMap();
            CreateMap<CompanyLocation, CompanyLocationSingleResponse>().ReverseMap();

            CreateMap<CompanyAssignment, CompanyAssignmentBaseResponse>().ReverseMap();
            CreateMap<CompanyAssignment, CompanyAssignmentSingleResponse>().ReverseMap();

            CreateMap<Person, PersonBaseResponse>().ReverseMap();
            CreateMap<Person, PersonSingleResponse>().ReverseMap();

            CreateMap<PersonBaseResponse, ContactBaseResponse>().ReverseMap();
        }
    }
}
