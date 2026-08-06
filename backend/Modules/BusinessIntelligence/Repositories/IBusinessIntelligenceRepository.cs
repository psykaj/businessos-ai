using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.Specifications;

namespace backend.Modules.BusinessIntelligence.Repositories;

/// <summary>
/// Represents the specialized repository contract for evaluating business intelligence specifications and executing high-performance analytical queries.
/// </summary>
public interface IBusinessIntelligenceRepository
{
    /// <summary>
    /// Asynchronously retrieves a read-only list of entities satisfying the provided specification criteria.
    /// </summary>
    /// <typeparam name="T">The target entity type.</typeparam>
    /// <param name="specification">The configured query specification.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A collection of matching entities.</returns>
    Task<IReadOnlyList<T>> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Asynchronously counts the total number of entities that satisfy the criteria of the provided specification without retrieving the actual entities.
    /// </summary>
    /// <typeparam name="T">The target entity type.</typeparam>
    /// <param name="specification">The configured query specification.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The count of matching entities.</returns>
    Task<int> CountAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Asynchronously checks whether any entities exist matching the criteria in the provided specification.
    /// </summary>
    /// <typeparam name="T">The target entity type.</typeparam>
    /// <param name="specification">The configured query specification.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if at least one matching entity exists; otherwise false.</returns>
    Task<bool> AnyAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default) where T : class;
}
