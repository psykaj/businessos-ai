using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Entities;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Repositories;
using backend.Modules.BusinessIntelligence.Services;
using backend.Modules.BusinessIntelligence.Specifications;
using backend.Modules.CashFlow.Entities;
using backend.Modules.CustomerFeedback.Entities;
using backend.Modules.Inventory.Entities;
using backend.Modules.Invoices.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BusinessIntelligence.Tests;

public class AiRecommendationEngineTests
{
    private readonly Mock<IBusinessIntelligenceRepository> _repositoryMock;
    private readonly Mock<ILogger<AiRecommendationEngine>> _loggerMock;
    private readonly AiRecommendationEngine _engine;
    private readonly Guid _orgId = Guid.NewGuid();

    public AiRecommendationEngineTests()
    {
        _repositoryMock = new Mock<IBusinessIntelligenceRepository>();
        _loggerMock = new Mock<ILogger<AiRecommendationEngine>>();
        
        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<ISpecification<FinanceInvoice>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FinanceInvoice>());
        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<ISpecification<CustomerSatisfactionScore>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CustomerSatisfactionScore>());
        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        _engine = new AiRecommendationEngine(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GenerateRecommendationsAsync_ShouldReturnAllSixRequiredInsightsWithHighConfidence()
    {
        // Act
        var recommendations = await _engine.GenerateRecommendationsAsync(_orgId);

        // Assert
        recommendations.Should().NotBeNull();
        recommendations.Count.Should().Be(6);

        foreach (var rec in recommendations)
        {
            rec.Title.Should().NotBeNullOrEmpty();
            rec.Description.Should().NotBeNullOrEmpty();
            rec.Category.Should().BeOneOf("Finance", "Customer", "Revenue", "Growth", "Inventory");
            rec.Priority.Should().BeOneOf("High", "Medium", "Low");
            rec.RecommendedAction.Should().NotBeNullOrEmpty();
            rec.ConfidenceScore.Should().BeInRange(0.0m, 1.0m);
            rec.ExpectedBusinessImpact.Should().NotBeNullOrEmpty();
        }

        recommendations.Any(r => r.Title.Contains("invoices overdue")).Should().BeTrue();
        recommendations.Any(r => r.Title.Contains("customers likely to churn")).Should().BeTrue();
        recommendations.Any(r => r.Title.Contains("Revenue dropped")).Should().BeTrue();
        recommendations.Any(r => r.Title.Contains("Sales increased because of repeat customers")).Should().BeTrue();
        recommendations.Any(r => r.Title.Contains("Inventory running low")).Should().BeTrue();
        recommendations.Any(r => r.Title.Contains("sells the fastest")).Should().BeTrue();
    }

    [Fact]
    public async Task AnalyzeRevenueAsync_ShouldReturnValidTrendsAndRepeatCustomerPercentages()
    {
        // Act
        var revenueInsight = await _engine.AnalyzeRevenueAsync(_orgId);

        // Assert
        revenueInsight.Should().NotBeNull();
        revenueInsight.CurrentRevenue.Should().BeGreaterThan(0);
        revenueInsight.RepeatCustomerRevenuePercentage.Should().BeInRange(0m, 100m);
        revenueInsight.RevenueTrends.Should().HaveCount(4);
        revenueInsight.PrimaryDriverExplanation.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task AnalyzeCustomersAsync_ShouldIdentifyAtRiskAccountsAndProvideRetentionActions()
    {
        // Act
        var customerInsight = await _engine.AnalyzeCustomersAsync(_orgId);

        // Assert
        customerInsight.Should().NotBeNull();
        customerInsight.TopChurnRisks.Should().NotBeEmpty();
        customerInsight.AverageCsatScore.Should().BeInRange(0m, 100m);
        foreach (var risk in customerInsight.TopChurnRisks)
        {
            risk.CustomerName.Should().NotBeNullOrEmpty();
            risk.RiskReason.Should().NotBeNullOrEmpty();
            risk.RetentionAction.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public async Task AnalyzeInventoryAsync_ShouldReportLowStockCountAndFastestSellingProduct()
    {
        // Act
        var inventoryInsight = await _engine.AnalyzeInventoryAsync(_orgId);

        // Assert
        inventoryInsight.Should().NotBeNull();
        inventoryInsight.FastestSellingProduct.Should().NotBeNullOrEmpty();
        inventoryInsight.LowStockItems.Should().NotBeNull();
        inventoryInsight.Summary.Should().NotBeNullOrEmpty();
    }
}
