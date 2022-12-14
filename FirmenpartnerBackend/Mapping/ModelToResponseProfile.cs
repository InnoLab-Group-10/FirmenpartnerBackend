using AutoMapper;
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
            CreateMap<CompanyLocation, CompanyGetAllLocationBaseResponse>().ReverseMap();

            CreateMap<CompanyAssignment, CompanyAssignmentBaseResponse>().ReverseMap();
            CreateMap<CompanyAssignment, CompanyAssignmentSingleResponse>().ReverseMap();

            CreateMap<Person, PersonBaseResponse>().ReverseMap();
            CreateMap<Person, PersonSingleResponse>().ReverseMap();

            CreateMap<Contact, ContactBaseResponse>().ReverseMap();
            CreateMap<Contact, ContactSingleResponse>().ReverseMap();

            CreateMap<Student, StudentBaseResponse>().ReverseMap();
            CreateMap<Student, StudentCsvBaseResponse>().ReverseMap();
            CreateMap<Student, StudentSingleResponse>().ReverseMap();

            CreateMap<Models.Data.Program, ProgramBaseResponse>().ReverseMap();
            CreateMap<Models.Data.Program, ProgramSingleResponse>().ReverseMap();

            CreateMap<Notification, NotificationBaseResponse>().ReverseMap();
            CreateMap<Notification, NotificationSingleResponse>().ReverseMap();

            CreateMap<TimelineEntry, TimelineEntryBaseResponse>().ReverseMap();
            CreateMap<TimelineEntry, TimelineEntrySingleResponse>().ReverseMap();

            CreateMap<FileEntry, FileBaseResponse>().ReverseMap();
            CreateMap<FileEntry, FileSingleResponse>().ReverseMap();

            CreateMap<MailingList, MailingListBaseResponse>().ReverseMap();
            CreateMap<MailingList, MailingListSingleResponse>().ReverseMap();

            CreateMap<MailingListEntry, MailingListEntryResponse>().ReverseMap();

            CreateMap<PersonBaseResponse, CompanyGetAllContactAssignmentBaseResponse>().ReverseMap();
        }
    }
}
