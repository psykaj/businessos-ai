using backend.Common;
using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.Interfaces;

public interface ILeadService
{
    Task<LeadResponseDto> CreateLeadAsync(CreateLeadDto dto, Guid organizationId);
    Task<LeadResponseDto?> GetLeadAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<LeadResponseDto>> GetAllLeadsAsync(Guid organizationId);
    Task<LeadResponseDto> UpdateLeadAsync(Guid id, UpdateLeadDto dto, Guid organizationId);
    Task DeleteLeadAsync(Guid id, Guid organizationId);
}

public interface IContactService
{
    Task<ContactResponseDto> CreateContactAsync(CreateContactDto dto, Guid organizationId);
    Task<ContactResponseDto?> GetContactAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<ContactResponseDto>> GetAllContactsAsync(Guid organizationId);
    Task<ContactResponseDto> UpdateContactAsync(Guid id, UpdateContactDto dto, Guid organizationId);
    Task DeleteContactAsync(Guid id, Guid organizationId);
}

public interface ICompanyService
{
    Task<CompanyResponseDto> CreateCompanyAsync(CreateCompanyDto dto, Guid organizationId);
    Task<CompanyResponseDto?> GetCompanyAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<CompanyResponseDto>> GetAllCompaniesAsync(Guid organizationId);
    Task<CompanyResponseDto> UpdateCompanyAsync(Guid id, UpdateCompanyDto dto, Guid organizationId);
    Task DeleteCompanyAsync(Guid id, Guid organizationId);
}

public interface IDealService
{
    Task<DealResponseDto> CreateDealAsync(CreateDealDto dto, Guid organizationId, Guid currentUserId);
    Task<DealResponseDto?> GetDealAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<DealResponseDto>> GetAllDealsAsync(Guid organizationId);
    Task<DealResponseDto> UpdateDealAsync(Guid id, UpdateDealDto dto, Guid organizationId);
    Task DeleteDealAsync(Guid id, Guid organizationId);
    Task<DealResponseDto> UpdateDealStageAsync(Guid id, PipelineStage stage, Guid organizationId, Guid currentUserId);
}

public interface ICrmActivityService
{
    Task<ActivityResponseDto> CreateActivityAsync(CreateActivityDto dto, Guid organizationId);
    Task<ActivityResponseDto?> GetActivityAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<ActivityResponseDto>> GetAllActivitiesAsync(Guid organizationId);
    Task<ActivityResponseDto> UpdateActivityAsync(Guid id, UpdateActivityDto dto, Guid organizationId);
    Task DeleteActivityAsync(Guid id, Guid organizationId);
    
    // Internal helper to automatically log activities
    Task LogActivityAsync(Guid organizationId, string relatedEntity, Guid relatedEntityId, ActivityType type, string description);
}

public interface ICrmTaskService
{
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto, Guid organizationId);
    Task<TaskResponseDto?> GetTaskAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<TaskResponseDto>> GetAllTasksAsync(Guid organizationId);
    Task<TaskResponseDto> UpdateTaskAsync(Guid id, UpdateTaskDto dto, Guid organizationId);
    Task DeleteTaskAsync(Guid id, Guid organizationId);
}

public interface INoteService
{
    Task<NoteResponseDto> CreateNoteAsync(CreateNoteDto dto, Guid organizationId, Guid currentUserId);
    Task<NoteResponseDto?> GetNoteAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<NoteResponseDto>> GetAllNotesAsync(Guid organizationId);
    Task<NoteResponseDto> UpdateNoteAsync(Guid id, UpdateNoteDto dto, Guid organizationId);
    Task DeleteNoteAsync(Guid id, Guid organizationId);
}

public interface ITagService
{
    Task<TagResponseDto> CreateTagAsync(CreateTagDto dto, Guid organizationId);
    Task<TagResponseDto?> GetTagAsync(Guid id, Guid organizationId);
    Task<IReadOnlyList<TagResponseDto>> GetAllTagsAsync(Guid organizationId);
    Task<TagResponseDto> UpdateTagAsync(Guid id, UpdateTagDto dto, Guid organizationId);
    Task DeleteTagAsync(Guid id, Guid organizationId);
}
