using backend.Exceptions;
using backend.Interfaces;
using backend.Modules.Forms.DTOs;
using backend.Modules.Forms.Entities;
using backend.Modules.Forms.Interfaces;
using backend.Modules.LeadCapture.Interfaces;

namespace backend.Modules.Forms.Services;

public class FormService : IFormService
{
    private readonly IFormRepository _formRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILeadCaptureService _leadCaptureService;

    public FormService(
        IFormRepository formRepository,
        IUnitOfWork unitOfWork,
        ILeadCaptureService leadCaptureService)
    {
        _formRepository = formRepository;
        _unitOfWork = unitOfWork;
        _leadCaptureService = leadCaptureService;
    }

    public async Task<(IReadOnlyList<FormDto> Items, int TotalCount)> GetFormsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _formRepository.GetFormsPagedAsync(organizationId, pageNumber, pageSize, search, status, cancellationToken);
        
        var dtos = items.Select(f => new FormDto
        {
            Id = f.Id,
            Name = f.Name,
            Description = f.Description,
            Slug = f.Slug,
            Status = f.Status,
            Theme = f.Theme,
            CreatedAt = f.CreatedAt
        }).ToList();

        return (dtos, totalCount);
    }

    public async Task<FormDto> GetFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetFormWithFieldsAsync(id, cancellationToken);
        if (form == null || form.OrganizationId != organizationId)
            throw new NotFoundException("Form not found");

        return MapToDto(form);
    }

    public async Task<FormDto> GetFormBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetFormBySlugAsync(slug, cancellationToken);
        if (form == null)
            throw new NotFoundException("Form not found or not published");

        return MapToDto(form);
    }

    public async Task<FormDto> CreateFormAsync(Guid organizationId, CreateFormDto dto, CancellationToken cancellationToken = default)
    {
        var form = new Form
        {
            OrganizationId = organizationId,
            Name = dto.Name,
            Description = dto.Description,
            Theme = dto.Theme,
            Slug = GenerateSlug(dto.Name),
            Status = "Draft",
            Fields = dto.Fields.Select(f => new FormField
            {
                Label = f.Label,
                Placeholder = f.Placeholder,
                FieldType = f.FieldType,
                Required = f.Required,
                Order = f.Order,
                Options = f.Options
            }).ToList()
        };

        await _formRepository.AddAsync(form, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToDto(form);
    }

    public async Task<FormDto> UpdateFormAsync(Guid organizationId, Guid id, UpdateFormDto dto, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetFormWithFieldsAsync(id, cancellationToken);
        if (form == null || form.OrganizationId != organizationId)
            throw new NotFoundException("Form not found");

        form.Name = dto.Name;
        form.Description = dto.Description;
        form.Theme = dto.Theme;
        if (!string.IsNullOrWhiteSpace(dto.Status))
        {
            form.Status = dto.Status;
        }

        // Update fields (simplistic approach: remove old, add new for brevity, or update existing)
        form.Fields.Clear();
        foreach (var f in dto.Fields)
        {
            form.Fields.Add(new FormField
            {
                FormId = form.Id,
                Label = f.Label,
                Placeholder = f.Placeholder,
                FieldType = f.FieldType,
                Required = f.Required,
                Order = f.Order,
                Options = f.Options
            });
        }

        _formRepository.Update(form);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToDto(form);
    }

    public async Task DeleteFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetByIdAsync(id, cancellationToken);
        if (form == null || form.OrganizationId != organizationId)
            throw new NotFoundException("Form not found");

        _formRepository.Delete(form);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<FormDto> PublishFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetFormWithFieldsAsync(id, cancellationToken);
        if (form == null || form.OrganizationId != organizationId)
            throw new NotFoundException("Form not found");

        form.Status = "Published";
        _formRepository.Update(form);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToDto(form);
    }

    public async Task<FormDto> DuplicateFormAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default)
    {
        var original = await _formRepository.GetFormWithFieldsAsync(id, cancellationToken);
        if (original == null || original.OrganizationId != organizationId)
            throw new NotFoundException("Form not found");

        var duplicate = new Form
        {
            OrganizationId = organizationId,
            Name = $"{original.Name} (Copy)",
            Description = original.Description,
            Theme = original.Theme,
            Slug = GenerateSlug($"{original.Name} Copy"),
            Status = "Draft",
            Fields = original.Fields.Select(f => new FormField
            {
                Label = f.Label,
                Placeholder = f.Placeholder,
                FieldType = f.FieldType,
                Required = f.Required,
                Order = f.Order,
                Options = f.Options
            }).ToList()
        };

        await _formRepository.AddAsync(duplicate, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToDto(duplicate);
    }

    public async Task SubmitFormAsync(Guid organizationId, Guid formId, SubmitFormDto dto, string device, string browser, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetFormWithFieldsAsync(formId, cancellationToken);
        if (form == null || form.OrganizationId != organizationId || form.Status != "Published")
            throw new NotFoundException("Form not found or not active");

        await _leadCaptureService.HandleFormSubmissionAsync(form.OrganizationId, formId, dto, device, browser, cancellationToken);
    }

    public async Task PublicSubmitFormAsync(Guid formId, SubmitFormDto dto, string device, string browser, CancellationToken cancellationToken = default)
    {
        var form = await _formRepository.GetFormWithFieldsAsync(formId, cancellationToken);
        if (form == null || form.Status != "Published")
            throw new NotFoundException("Form not found or not active");

        // Delegate to LeadCaptureService to handle CRM integration and journey
        await _leadCaptureService.HandleFormSubmissionAsync(form.OrganizationId, formId, dto, device, browser, cancellationToken);
    }

    private static FormDto MapToDto(Form form)
    {
        return new FormDto
        {
            Id = form.Id,
            Name = form.Name,
            Description = form.Description,
            Slug = form.Slug,
            Status = form.Status,
            Theme = form.Theme,
            CreatedAt = form.CreatedAt,
            Fields = form.Fields.Select(f => new FormFieldDto
            {
                Id = f.Id,
                Label = f.Label,
                Placeholder = f.Placeholder,
                FieldType = f.FieldType,
                Required = f.Required,
                Order = f.Order,
                Options = f.Options
            }).ToList()
        };
    }

    private static string GenerateSlug(string name)
    {
        return name.ToLower().Replace(" ", "-") + "-" + Guid.NewGuid().ToString("N")[..6];
    }
}
