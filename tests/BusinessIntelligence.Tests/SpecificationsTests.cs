using System;
using backend.Modules.BusinessIntelligence.Specifications;
using FluentAssertions;
using Xunit;

namespace BusinessIntelligence.Tests;

public class SpecificationsTests
{
    private readonly Guid _orgId = Guid.NewGuid();

    [Fact]
    public void OverdueInvoicesSpecification_ShouldInitializeWithCorrectOrdering()
    {
        var spec = new OverdueInvoicesSpecification(_orgId);

        spec.Criteria.Should().NotBeNull();
        spec.OrderByDescending.Should().NotBeNull();
    }

    [Fact]
    public void ChurnRiskCustomersSpecification_ShouldInitializeCriteria()
    {
        var spec = new ChurnRiskCustomersSpecification(_orgId);

        spec.Criteria.Should().NotBeNull();
    }

    [Fact]
    public void LowStockProductsSpecification_ShouldIncludeStockLevels()
    {
        var spec = new LowStockProductsSpecification(_orgId);

        spec.Criteria.Should().NotBeNull();
        spec.Includes.Should().NotBeEmpty();
    }

    [Fact]
    public void RecentCashFlowSpecification_ShouldInitializeDateThreshold()
    {
        var cutoff = DateTime.UtcNow.AddDays(-30);
        var spec = new RecentCashFlowSpecification(_orgId, cutoff);

        spec.Criteria.Should().NotBeNull();
    }
}
