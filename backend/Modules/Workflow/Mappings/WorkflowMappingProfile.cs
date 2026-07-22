using AutoMapper;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Mappings;

public class WorkflowMappingProfile : Profile
{
    public WorkflowMappingProfile()
    {
        CreateMap<Entities.Workflow, WorkflowDto>();
        CreateMap<WorkflowTrigger, WorkflowTriggerDto>();
        CreateMap<WorkflowAction, WorkflowActionDto>();
        CreateMap<WorkflowCondition, WorkflowConditionDto>();
        CreateMap<WorkflowExecution, WorkflowExecutionDto>();
        CreateMap<WorkflowExecutionLog, WorkflowExecutionLogDto>();
        CreateMap<Integration, IntegrationDto>()
            .ForMember(dest => dest.MaskedApiKey, opt => opt.Ignore());
    }
}
