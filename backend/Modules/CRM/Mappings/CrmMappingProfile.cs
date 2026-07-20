using AutoMapper;
using backend.Entities;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Entities;

namespace backend.Modules.CRM.Mappings;

public class CrmMappingProfile : Profile
{
    public CrmMappingProfile()
    {
        CreateMap<CreateLeadDto, Lead>();
        CreateMap<UpdateLeadDto, Lead>();
        CreateMap<Lead, LeadResponseDto>();

        CreateMap<CreateContactDto, Contact>();
        CreateMap<UpdateContactDto, Contact>();
        CreateMap<Contact, ContactResponseDto>();

        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();
        CreateMap<Company, CompanyResponseDto>();

        CreateMap<CreateDealDto, Deal>();
        CreateMap<UpdateDealDto, Deal>();
        CreateMap<Deal, DealResponseDto>();

        CreateMap<CreateActivityDto, CrmActivity>();
        CreateMap<UpdateActivityDto, CrmActivity>();
        CreateMap<CrmActivity, ActivityResponseDto>();

        CreateMap<CreateTaskDto, CrmTask>();
        CreateMap<UpdateTaskDto, CrmTask>();
        CreateMap<CrmTask, TaskResponseDto>();

        CreateMap<CreateNoteDto, Note>();
        CreateMap<UpdateNoteDto, Note>();
        CreateMap<Note, NoteResponseDto>();

        CreateMap<CreateTagDto, Tag>();
        CreateMap<UpdateTagDto, Tag>();
        CreateMap<Tag, TagResponseDto>();
    }
}
