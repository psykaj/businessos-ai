using FluentValidation;
using backend.Modules.CommunicationHub.Assignments.DTOs;

namespace backend.Modules.CommunicationHub.Assignments.Validators;

public class AssignmentFilterValidator : AbstractValidator<AssignmentFilterDto>
{
    public AssignmentFilterValidator()
    {
        // Add specific filtering rule validations if needed
    }
}
