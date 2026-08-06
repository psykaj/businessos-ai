using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace backend.Modules.BusinessIntelligence.Specifications;

/// <summary>
/// Base class providing default behaviors and helper methods for configuring query specifications.
/// </summary>
/// <typeparam name="T">The entity type this specification targets.</typeparam>
public abstract class BaseSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class with no filtering criteria.
    /// </summary>
    protected BaseSpecification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class with specified criteria.
    /// </summary>
    /// <param name="criteria">The expression filter to apply.</param>
    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <inheritdoc />
    public Expression<Func<T, bool>>? Criteria { get; private set; }

    /// <inheritdoc />
    public List<Expression<Func<T, object>>> Includes { get; } = new();

    /// <inheritdoc />
    public List<string> IncludeStrings { get; } = new();

    /// <inheritdoc />
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    /// <inheritdoc />
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    /// <inheritdoc />
    public int Take { get; private set; }

    /// <inheritdoc />
    public int Skip { get; private set; }

    /// <inheritdoc />
    public bool IsPagingEnabled { get; private set; }

    /// <inheritdoc />
    public bool IsNoTracking { get; private set; } = true; // Default to read-only optimization for BI engine

    /// <summary>
    /// Adds an eager-loaded navigation property expression to the specification.
    /// </summary>
    /// <param name="includeExpression">The property expression to include.</param>
    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Adds a string-based navigation property path to the specification.
    /// </summary>
    /// <param name="includeString">The string path of navigation properties to include.</param>
    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    /// <summary>
    /// Applies ascending order to the specification query.
    /// </summary>
    /// <param name="orderByExpression">The property expression to order by.</param>
    protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Applies descending order to the specification query.
    /// </summary>
    /// <param name="orderByDescendingExpression">The property expression to order by descending.</param>
    protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    /// <summary>
    /// Configures pagination parameters for the specification query.
    /// </summary>
    /// <param name="skip">Number of elements to skip.</param>
    /// <param name="take">Number of elements to take.</param>
    protected virtual void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    /// <summary>
    /// Sets whether entity tracking should be enabled or disabled in Entity Framework Core.
    /// </summary>
    /// <param name="isNoTracking">If true, queries use AsNoTracking for read performance.</param>
    protected virtual void ApplyNoTracking(bool isNoTracking)
    {
        IsNoTracking = isNoTracking;
    }
}
