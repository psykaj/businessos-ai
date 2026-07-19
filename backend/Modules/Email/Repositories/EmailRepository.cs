using backend.Entities;
using backend.Modules.Email.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Email.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly ApplicationDbContext _context;

    public EmailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmailTemplate?> GetTemplateAsync(Guid organizationId, Guid templateId)
    {
        return await _context.EmailTemplates
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.Id == templateId && !t.IsDeleted);
    }

    public async Task<IEnumerable<EmailTemplate>> GetTemplatesAsync(Guid organizationId)
    {
        return await _context.EmailTemplates
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<EmailTemplate> AddTemplateAsync(EmailTemplate template)
    {
        _context.EmailTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task UpdateTemplateAsync(EmailTemplate template)
    {
        _context.EmailTemplates.Update(template);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTemplateAsync(EmailTemplate template)
    {
        template.IsDeleted = true;
        _context.EmailTemplates.Update(template);
        await _context.SaveChangesAsync();
    }
}
