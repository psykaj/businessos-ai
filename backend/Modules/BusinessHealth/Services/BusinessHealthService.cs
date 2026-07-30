using System;
using System.Threading.Tasks;
using backend.Modules.BusinessHealth.Entities;
using backend.Modules.BusinessHealth.Repositories;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessHealth.Services;

public class BusinessHealthService
{
    private readonly IBusinessHealthRepository _repository;
    private readonly ApplicationDbContext _context;

    public BusinessHealthService(IBusinessHealthRepository repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<BusinessHealthScore> CalculateHealthAsync(Guid organizationId)
    {
        // Mock calculations for demonstration
        var random = new Random();
        decimal finHealth = random.Next(60, 95);
        decimal opHealth = random.Next(70, 100);
        decimal custHealth = random.Next(50, 90);

        decimal overall = (finHealth + opHealth + custHealth) / 3m;
        string status = overall >= 85 ? "Excellent" : overall >= 70 ? "Healthy" : overall >= 50 ? "Warning" : "Critical";

        var score = new BusinessHealthScore
        {
            OrganizationId = organizationId,
            FinancialHealth = finHealth,
            OperationalHealth = opHealth,
            CustomerHealth = custHealth,
            OverallScore = overall,
            Status = status,
            CalculatedAt = DateTime.UtcNow
        };

        return await _repository.AddAsync(score);
    }
}
