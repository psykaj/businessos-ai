using System;
using System.Collections.Generic;
using backend.Modules.CommunicationHub.Entities;

namespace backend.Modules.CommunicationHub.Templates.DTOs;

public class MessageTemplateDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public CommunicationChannelType? ChannelType { get; set; }
    public string Category { get; set; } = "General";
    public string ShortcutCode { get; set; } = string.Empty;
    public string ParametersJson { get; set; } = "[]";
    public int UsageCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTemplateRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public CommunicationChannelType? ChannelType { get; set; }
    public string Category { get; set; } = "General";
    public string ShortcutCode { get; set; } = string.Empty;
    public string ParametersJson { get; set; } = "[]";
}

public class UpdateTemplateRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public CommunicationChannelType? ChannelType { get; set; }
    public string Category { get; set; } = "General";
    public string ShortcutCode { get; set; } = string.Empty;
    public string ParametersJson { get; set; } = "[]";
}

public class TemplateRenderRequest
{
    public Dictionary<string, string> Variables { get; set; } = new();
}

public class TemplateRenderResponse
{
    public string RenderedContent { get; set; } = string.Empty;
}
