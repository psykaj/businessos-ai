using backend.Common;

namespace backend.Modules.Forms.Entities;

public class FormField : BaseEntity
{
    public Guid FormId { get; set; }
    public Form? Form { get; set; }

    public string Label { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string FieldType { get; set; } = "Text"; // Text, Email, Phone, Number, Dropdown, Radio, Checkbox, Date, Textarea
    public bool Required { get; set; } = false;
    public int Order { get; set; } = 0;
    
    // For Dropdown/Radio/Checkbox - JSON serialized string of options
    public string Options { get; set; } = string.Empty; 
}
