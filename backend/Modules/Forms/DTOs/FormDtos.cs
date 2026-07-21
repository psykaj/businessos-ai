using System.Text.Json.Serialization;

namespace backend.Modules.Forms.DTOs;

public class FormFieldDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string FieldType { get; set; } = "Text";
    public bool Required { get; set; }
    public int Order { get; set; }
    public string Options { get; set; } = string.Empty;
}

public class FormDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<FormFieldDto> Fields { get; set; } = new();
}

public class CreateFormDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Theme { get; set; } = "Default";
    public List<CreateFormFieldDto> Fields { get; set; } = new();
}

public class CreateFormFieldDto
{
    public string Label { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string FieldType { get; set; } = "Text";
    public bool Required { get; set; }
    public int Order { get; set; }
    public string Options { get; set; } = string.Empty;
}

public class UpdateFormDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<UpdateFormFieldDto> Fields { get; set; } = new();
}

public class UpdateFormFieldDto
{
    public Guid? Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string FieldType { get; set; } = "Text";
    public bool Required { get; set; }
    public int Order { get; set; }
    public string Options { get; set; } = string.Empty;
}

public class SubmitFormDto
{
    // JSON string containing key-value pairs of the submission
    public string SubmittedData { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
