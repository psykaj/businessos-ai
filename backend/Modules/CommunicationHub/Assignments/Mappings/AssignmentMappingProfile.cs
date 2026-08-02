using AutoMapper;
using backend.Modules.CommunicationHub.Assignments.DTOs;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Assignments.Mappings;

public class AssignmentMappingProfile : Profile
{
    public AssignmentMappingProfile()
    {
        CreateMap<Assignment, AssignmentDto>();
    }
}
