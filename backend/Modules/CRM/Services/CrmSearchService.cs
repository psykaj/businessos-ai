using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CRM.Services;

public class CrmSearchService : ICrmSearchService
{
    private readonly ApplicationDbContext _context;

    public CrmSearchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<GlobalSearchResultDto>> SearchAsync(string query, Guid organizationId)
    {
        var results = new List<GlobalSearchResultDto>();
        if (string.IsNullOrWhiteSpace(query)) return results;

        query = query.ToLower();

        // Search Leads
        var leads = await _context.Leads
            .Where(l => l.OrganizationId == organizationId && 
                        (l.FirstName.ToLower().Contains(query) || 
                         l.LastName.ToLower().Contains(query) || 
                         l.Email.ToLower().Contains(query) || 
                         l.CompanyName.ToLower().Contains(query)))
            .Take(10)
            .ToListAsync();

        results.AddRange(leads.Select(l => new GlobalSearchResultDto
        {
            EntityType = "Lead",
            EntityId = l.Id,
            Title = $"{l.FirstName} {l.LastName}".Trim(),
            Description = $"Lead at {l.CompanyName} - {l.Email}",
            MatchReason = "Matched Name, Email or Company"
        }));

        // Search Contacts
        var contacts = await _context.Contacts
            .Where(c => c.OrganizationId == organizationId && 
                        (c.FirstName.ToLower().Contains(query) || 
                         c.LastName.ToLower().Contains(query) || 
                         c.Email.ToLower().Contains(query)))
            .Take(10)
            .ToListAsync();

        results.AddRange(contacts.Select(c => new GlobalSearchResultDto
        {
            EntityType = "Contact",
            EntityId = c.Id,
            Title = $"{c.FirstName} {c.LastName}".Trim(),
            Description = $"Contact - {c.Email}",
            MatchReason = "Matched Name or Email"
        }));

        // Search Companies
        var companies = await _context.Companies
            .Where(c => c.OrganizationId == organizationId && 
                        (c.CompanyName.ToLower().Contains(query) || 
                         c.Website.ToLower().Contains(query)))
            .Take(10)
            .ToListAsync();

        results.AddRange(companies.Select(c => new GlobalSearchResultDto
        {
            EntityType = "Company",
            EntityId = c.Id,
            Title = c.CompanyName,
            Description = $"Company - {c.Website}",
            MatchReason = "Matched Company Name or Website"
        }));

        // Search Deals
        var deals = await _context.Deals
            .Where(d => d.OrganizationId == organizationId && d.Title.ToLower().Contains(query))
            .Take(10)
            .ToListAsync();

        results.AddRange(deals.Select(d => new GlobalSearchResultDto
        {
            EntityType = "Deal",
            EntityId = d.Id,
            Title = d.Title,
            Description = $"Deal - Amount: {d.Amount}",
            MatchReason = "Matched Deal Title"
        }));

        return results;
    }
}
