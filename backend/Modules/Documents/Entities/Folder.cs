using backend.Common;

namespace backend.Modules.Documents.Entities;

public class Folder : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public Guid? ParentFolderId { get; set; }
    public Folder? ParentFolder { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Path { get; set; } = "/";
    public string? Color { get; set; }
    public string? Icon { get; set; }

    public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}
