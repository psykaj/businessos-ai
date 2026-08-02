using FluentValidation;
using backend.Modules.CommunicationHub.Notifications.DTOs;

namespace backend.Modules.CommunicationHub.Notifications.Validators;

public class NotificationDtoValidator : AbstractValidator<CommunicationNotificationDto>
{
    public NotificationDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}
