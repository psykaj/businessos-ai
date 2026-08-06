using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Queries.GetBusinessHealth;
using backend.Modules.BusinessIntelligence.Queries.GetDashboardSummary;
using backend.Modules.BusinessIntelligence.Queries.GetRecommendations;
using backend.Modules.BusinessIntelligence.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BusinessIntelligence.Tests;

public class CqrsQueriesAndValidatorsTests
{
    private readonly Guid _orgId = Guid.NewGuid();

    [Fact]
    public void GetDashboardSummaryQueryValidator_ShouldFail_WhenOrganizationIdIsEmpty()
    {
        var validator = new GetDashboardSummaryQueryValidator();
        var query = new GetDashboardSummaryQuery(Guid.Empty);

        var result = validator.Validate(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(query.OrganizationId));
    }

    [Fact]
    public void GetDashboardSummaryQueryValidator_ShouldPass_WhenOrganizationIdIsValid()
    {
        var validator = new GetDashboardSummaryQueryValidator();
        var query = new GetDashboardSummaryQuery(_orgId);

        var result = validator.Validate(query);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task GetRecommendationsQueryHandler_ShouldUseMemoryCacheOnSubsequentCalls()
    {
        // Arrange
        var mockEngine = new Mock<IAiRecommendationEngine>();
        var mockLogger = new Mock<ILogger<GetRecommendationsQueryHandler>>();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var expectedData = new List<RecommendationDto>
        {
            new() { Title = "Test AI Recommendation", Priority = "High", Category = "Finance" }
        };

        mockEngine.Setup(x => x.GenerateRecommendationsAsync(_orgId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedData)
            .Verifiable();

        var handler = new GetRecommendationsQueryHandler(mockEngine.Object, memoryCache, mockLogger.Object);

        // Act - First Call
        var res1 = await handler.Handle(new GetRecommendationsQuery(_orgId), CancellationToken.None);
        // Act - Second Call (should come from cache)
        var res2 = await handler.Handle(new GetRecommendationsQuery(_orgId), CancellationToken.None);

        // Assert
        res1.Should().BeEquivalentTo(res2);
        mockEngine.Verify(x => x.GenerateRecommendationsAsync(_orgId, It.IsAny<CancellationToken>()), Times.Once, "The recommendation engine should only be evaluated once due to IMemoryCache caching.");
    }

    [Fact]
    public async Task GetBusinessHealthQueryHandler_ShouldCacheResult()
    {
        // Arrange
        var mockHealth = new Mock<IBusinessHealthCalculator>();
        var mockLogger = new Mock<ILogger<GetBusinessHealthQueryHandler>>();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var healthDto = new BusinessHealthDto { Score = 88, Status = "Good", Explanation = "Solid performance." };

        mockHealth.Setup(x => x.CalculateHealthAsync(_orgId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(healthDto);

        var handler = new GetBusinessHealthQueryHandler(mockHealth.Object, memoryCache, mockLogger.Object);

        // Act
        await handler.Handle(new GetBusinessHealthQuery(_orgId), CancellationToken.None);
        var result = await handler.Handle(new GetBusinessHealthQuery(_orgId), CancellationToken.None);

        // Assert
        result.Score.Should().Be(88);
        mockHealth.Verify(x => x.CalculateHealthAsync(_orgId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
