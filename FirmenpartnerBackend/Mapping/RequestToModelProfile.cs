﻿using AutoMapper;
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

            CreateMap<ContactRequest, Contact>().ReverseMap();

            CreateMap<StudentRequest, Student>().ReverseMap();

            CreateMap<NotificationRequest, Notification>().ReverseMap();

            CreateMap<TimelineEntryRequest, TimelineEntry>().ReverseMap();

            CreateMap<MailingListRequest, MailingList>().ReverseMap();

            CreateMap<MailingListEntryRequest, MailingListEntry>().ReverseMap();

            CreateMap<MailSettingRequest, MailSetting>().ReverseMap();

            CreateMap<MailTemplateRequest, MailTemplate>().ReverseMap();

            CreateMap<ProgramRequest, Models.Data.Program>().ReverseMap();

            CreateMap<StudentCountRequest, StudentCount>().ReverseMap();
        }
    }
}
