using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessIntelligence.Specifications;

/// <summary>
/// Helper class responsible for translating specification descriptors into executable Entity Framework Core IQueryable expressions.
/// </summary>
public static class SpecificationEvaluator
{
    /// <summary>
    /// Applies the configured filtering, sorting, paging, and eager-loading criteria of a specification to an input queryable stream.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="inputQuery">The base IQueryable stream (usually an EF Core DbSet).</param>
    /// <param name="spec">The specific ISpecification instance containing criteria.</param>
    /// <returns>The modified and optimized IQueryable query ready for evaluation.</returns>
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> spec) where T : class
    {
        var query = inputQuery;

        if (spec.IsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        foreach (var include in spec.Includes)
        {
            query = query.Include(include);
        }

        foreach (var includeString in spec.IncludeStrings)
        {
            query = query.Include(includeString);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}
