using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using backend.Entities;
using backend.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly ILogger<WhatsAppService> _logger;

    public WhatsAppService(ApplicationDbContext context, HttpClient httpClient, ILogger<WhatsAppService> logger)
    {
        _context = context;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<WhatsAppSettings?> GetSettingsAsync(Guid organizationId)
    {
        return await _context.WhatsAppSettings
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId);
    }

    public async Task<WhatsAppSettings> SaveSettingsAsync(WhatsAppSettings settings)
    {
        var existing = await _context.WhatsAppSettings
            .FirstOrDefaultAsync(s => s.OrganizationId == settings.OrganizationId);

        if (existing == null)
        {
            _context.WhatsAppSettings.Add(settings);
        }
        else
        {
            existing.PhoneNumberId = settings.PhoneNumberId;
            existing.BusinessAccountId = settings.BusinessAccountId;
            existing.AccessToken = settings.AccessToken;
            existing.WebhookVerifyToken = settings.WebhookVerifyToken;
            _context.WhatsAppSettings.Update(existing);
        }

        await _context.SaveChangesAsync();
        return settings;
    }

    public async Task<bool> SendMessageAsync(Guid organizationId, string to, string templateName, string languageCode)
    {
        var settings = await GetSettingsAsync(organizationId);
        if (settings == null) return false;

        var url = $"https://graph.facebook.com/v19.0/{settings.PhoneNumberId}/messages";
        
        var payload = new
        {
            messaging_product = "whatsapp",
            to = to,
            type = "template",
            template = new
            {
                name = templateName,
                language = new { code = languageCode }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.AccessToken);
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Failed to send WhatsApp message: {error}");
            return false;
        }

        return true;
    }

    public async Task<List<WhatsAppTemplate>> SyncTemplatesAsync(Guid organizationId)
    {
        var settings = await GetSettingsAsync(organizationId);
        if (settings == null) throw new Exception("WhatsApp settings not found");

        var url = $"https://graph.facebook.com/v19.0/{settings.BusinessAccountId}/message_templates";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.AccessToken);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Failed to fetch WhatsApp templates: {error}");
            throw new Exception("Failed to fetch templates from Meta API");
        }

        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(content);

        var templates = new List<WhatsAppTemplate>();
        if (data.TryGetProperty("data", out var dataElement) && dataElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in dataElement.EnumerateArray())
            {
                var template = new WhatsAppTemplate
                {
                    OrganizationId = organizationId,
                    Name = item.GetProperty("name").GetString() ?? "",
                    Language = item.GetProperty("language").GetString() ?? "",
                    Category = item.GetProperty("category").GetString() ?? "",
                    Status = item.GetProperty("status").GetString() ?? "",
                    Components = item.GetProperty("components").ToString()
                };
                templates.Add(template);
            }
        }

        // Clean existing templates for this organization
        var existing = await _context.WhatsAppTemplates.Where(t => t.OrganizationId == organizationId).ToListAsync();
        _context.WhatsAppTemplates.RemoveRange(existing);

        _context.WhatsAppTemplates.AddRange(templates);
        await _context.SaveChangesAsync();

        return templates;
    }
}
