using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.Specifications;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Repositories;

/// <summary>
/// Implementation of <see cref="IBusinessIntelligenceRepository"/> leveraging Entity Framework Core and Specification pattern for analytics querying.
/// </summary>
public sealed class BusinessIntelligenceRepository : IBusinessIntelligenceRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessIntelligenceRepository"/> class with the database context.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    public BusinessIntelligenceRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default) where T : class
    {
        var query = ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default) where T : class
    {
        var query = _dbContext.Set<T>().AsNoTracking();
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }
        return await query.CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> AnyAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default) where T : class
    {
        var query = _dbContext.Set<T>().AsNoTracking();
        if (specification.Criteria != null)
        {
            return await query.AnyAsync(specification.Criteria, cancellationToken);
        }
        return await query.AnyAsync(cancellationToken);
    }

    private IQueryable<T> ApplySpecification<T>(ISpecification<T> spec) where T : class
    {
        return SpecificationEvaluator.GetQuery(_dbContext.Set<T>(), spec);
    }
}
