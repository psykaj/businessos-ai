using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Entities;
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

public class BusinessHealthCalculatorTests
{
    private readonly Mock<IBusinessIntelligenceRepository> _repositoryMock;
    private readonly Mock<ILogger<BusinessHealthCalculator>> _loggerMock;
    private readonly BusinessHealthCalculator _calculator;
    private readonly Guid _orgId = Guid.NewGuid();

    public BusinessHealthCalculatorTests()
    {
        _repositoryMock = new Mock<IBusinessIntelligenceRepository>();
        _loggerMock = new Mock<ILogger<BusinessHealthCalculator>>();

        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<ISpecification<FinanceInvoice>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FinanceInvoice>());
        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<ISpecification<CashFlowEntry>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CashFlowEntry>());
        _repositoryMock.Setup(x => x.CountAsync(It.IsAny<ISpecification<Customer>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(150);
        _repositoryMock.Setup(x => x.CountAsync(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);
        _repositoryMock.Setup(x => x.CountAsync(It.IsAny<ISpecification<FinanceInvoice>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(5);
        _repositoryMock.Setup(x => x.CountAsync(It.IsAny<ISpecification<CustomerSatisfactionScore>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        _calculator = new BusinessHealthCalculator(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CalculateHealthAsync_ShouldReturnScoreBetweenZeroAndOneHundredWithExplanation()
    {
        // Act
        var health = await _calculator.CalculateHealthAsync(_orgId);

        // Assert
        health.Should().NotBeNull();
        health.Score.Should().BeInRange(0, 100);
        health.Status.Should().BeOneOf("Excellent", "Good", "Needs Attention", "Critical");
        health.Explanation.Should().NotBeNullOrEmpty();
        health.Explanation.Should().Contain("BusinessOS AI assigned an overall health score of");
    }

    [Fact]
    public async Task CalculateHealthAsync_ShouldEvaluateAllSixBusinessPillars()
    {
        // Act
        var health = await _calculator.CalculateHealthAsync(_orgId);

        // Assert
        health.DimensionScores.Should().ContainKeys("Revenue", "Customer Growth", "Cash Flow", "Inventory", "Pending Invoices", "Customer Satisfaction");
        health.DimensionExplanations.Should().ContainKeys("Revenue", "Customer Growth", "Cash Flow", "Inventory", "Pending Invoices", "Customer Satisfaction");

        foreach (var kvp in health.DimensionScores)
        {
            kvp.Value.Should().BeInRange(0, 100, $"Pillar '{kvp.Key}' score must be between 0 and 100");
        }
    }
}
