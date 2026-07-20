namespace backend.Modules.CRM.DTOs;

public class CreateNoteDto
{
    public string RelatedEntity { get; set; } = string.Empty;
    public Guid RelatedEntityId { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class UpdateNoteDto : CreateNoteDto
{
}

public class NoteResponseDto : CreateNoteDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
