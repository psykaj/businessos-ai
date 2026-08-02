using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Templates.DTOs;
using backend.Modules.CommunicationHub.Templates.Interfaces;
using Microsoft.Extensions.Logging;

namespace backend.Modules.CommunicationHub.Templates.Services;

public class TemplateService : ITemplateService
{
    private readonly ITemplateRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TemplateService> _logger;

    public TemplateService(ITemplateRepository repository, IMapper mapper, ILogger<TemplateService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<MessageTemplateDto>> GetTemplatesAsync(Guid organizationId, CommunicationChannelType? channel, string? category)
    {
        var items = await _repository.GetByOrganizationIdAsync(organizationId, channel, category);
        return _mapper.Map<IEnumerable<MessageTemplateDto>>(items);
    }

    public async Task<MessageTemplateDto?> GetTemplateByIdAsync(Guid id, Guid organizationId)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return null;
        return _mapper.Map<MessageTemplateDto>(item);
    }

    public async Task<MessageTemplateDto> CreateTemplateAsync(Guid organizationId, CreateTemplateRequest request)
    {
        var entity = new MessageTemplate
        {
            OrganizationId = organizationId,
            Title = request.Title,
            Content = request.Content,
            ChannelType = request.ChannelType,
            Category = request.Category,
            ShortcutCode = request.ShortcutCode,
            ParametersJson = request.ParametersJson,
            UsageCount = 0
        };

        await _repository.CreateAsync(entity);
        _logger.LogInformation("Created communication template '{Title}' with shortcut '{Code}' for org {OrgId}", entity.Title, entity.ShortcutCode, organizationId);
        return _mapper.Map<MessageTemplateDto>(entity);
    }

    public async Task<MessageTemplateDto?> UpdateTemplateAsync(Guid id, Guid organizationId, UpdateTemplateRequest request)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return null;

        item.Title = request.Title;
        item.Content = request.Content;
        item.ChannelType = request.ChannelType;
        item.Category = request.Category;
        item.ShortcutCode = request.ShortcutCode;
        item.ParametersJson = request.ParametersJson;

        await _repository.UpdateAsync(item);
        return _mapper.Map<MessageTemplateDto>(item);
    }

    public async Task<bool> DeleteTemplateAsync(Guid id, Guid organizationId)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return false;

        await _repository.DeleteAsync(item);
        return true;
    }

    public async Task<TemplateRenderResponse?> RenderTemplateAsync(Guid id, Guid organizationId, Dictionary<string, string> variables)
    {
        var item = await _repository.GetByIdAsync(id, organizationId);
        if (item == null) return null;

        var content = item.Content;
        foreach (var kv in variables)
        {
            content = content.Replace($"{{{{{kv.Key}}}}}", kv.Value);
            content = content.Replace($"{{{kv.Key}}}", kv.Value);
        }

        // Increment usage count
        item.UsageCount++;
        await _repository.UpdateAsync(item);

        return new TemplateRenderResponse { RenderedContent = content };
    }
}
