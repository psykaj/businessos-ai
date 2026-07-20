using backend.Common;
using backend.Entities;

namespace backend.Modules.CRM.Entities;

public class Company : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    // Navigation properties for relationships
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public ICollection<Deal> Deals { get; set; } = new List<Deal>();
}
