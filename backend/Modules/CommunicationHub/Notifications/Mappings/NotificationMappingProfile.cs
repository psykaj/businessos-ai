using AutoMapper;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Notifications.DTOs;

namespace backend.Modules.CommunicationHub.Notifications.Mappings;

public class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<CommunicationNotification, CommunicationNotificationDto>();
    }
}
