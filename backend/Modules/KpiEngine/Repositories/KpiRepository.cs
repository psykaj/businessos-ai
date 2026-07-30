using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.KpiEngine.Entities;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.KpiEngine.Repositories;

public class KpiRepository : IKpiRepository
{
    private readonly ApplicationDbContext _context;

    public KpiRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<KPI>> GetAllAsync(Guid organizationId)
    {
        return await _context.KPIs
            .Where(k => k.OrganizationId == organizationId && !k.IsDeleted)
            .ToListAsync();
    }

    public async Task<KPI?> GetByIdAsync(Guid id, Guid organizationId)
    {
        return await _context.KPIs
            .FirstOrDefaultAsync(k => k.Id == id && k.OrganizationId == organizationId && !k.IsDeleted);
    }

    public async Task<IEnumerable<KPI>> GetByCategoryAsync(string category, Guid organizationId)
    {
        return await _context.KPIs
            .Where(k => k.Category == category && k.OrganizationId == organizationId && !k.IsDeleted)
            .ToListAsync();
    }

    public async Task<KPI> AddAsync(KPI kpi)
    {
        _context.KPIs.Add(kpi);
        await _context.SaveChangesAsync();
        return kpi;
    }

    public async Task UpdateAsync(KPI kpi)
    {
        _context.KPIs.Update(kpi);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(KPI kpi)
    {
        kpi.IsDeleted = true;
        _context.KPIs.Update(kpi);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<KPI>> GetAllActiveAsync()
    {
        return await _context.KPIs
            .Where(k => !k.IsDeleted)
            .ToListAsync();
    }
}
