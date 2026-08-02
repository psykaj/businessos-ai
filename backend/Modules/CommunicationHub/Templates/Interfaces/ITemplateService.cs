using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.CommunicationHub.Entities;
using backend.Modules.CommunicationHub.Templates.DTOs;

namespace backend.Modules.CommunicationHub.Templates.Interfaces;

public interface ITemplateService
{
    Task<IEnumerable<MessageTemplateDto>> GetTemplatesAsync(Guid organizationId, CommunicationChannelType? channel, string? category);
    Task<MessageTemplateDto?> GetTemplateByIdAsync(Guid id, Guid organizationId);
    Task<MessageTemplateDto> CreateTemplateAsync(Guid organizationId, CreateTemplateRequest request);
    Task<MessageTemplateDto?> UpdateTemplateAsync(Guid id, Guid organizationId, UpdateTemplateRequest request);
    Task<bool> DeleteTemplateAsync(Guid id, Guid organizationId);
    Task<TemplateRenderResponse?> RenderTemplateAsync(Guid id, Guid organizationId, Dictionary<string, string> variables);
}
