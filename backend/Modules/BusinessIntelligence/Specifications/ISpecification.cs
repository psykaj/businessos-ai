using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace backend.Modules.BusinessIntelligence.Specifications;

/// <summary>
/// Represents the query specification contract for filtering, including, ordering, and paginating entity queries.
/// </summary>
/// <typeparam name="T">The type of the target entity.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the criteria expression used to filter entities in the query.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the list of include expressions for eager loading related data.
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Gets the list of string-based include paths for nested eager loading.
    /// </summary>
    List<string> IncludeStrings { get; }

    /// <summary>
    /// Gets the ascending ordering expression.
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }

    /// <summary>
    /// Gets the descending ordering expression.
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }

    /// <summary>
    /// Gets the number of results to take for pagination or top-N querying.
    /// </summary>
    int Take { get; }

    /// <summary>
    /// Gets the number of results to skip for pagination.
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// Gets a value indicating whether pagination (Take/Skip) is enabled for this specification.
    /// </summary>
    bool IsPagingEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether entity tracking should be disabled (AsNoTracking) for optimization.
    /// </summary>
    bool IsNoTracking { get; }
}
