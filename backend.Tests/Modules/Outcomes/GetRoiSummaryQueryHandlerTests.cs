using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Outcomes.Entities;
using backend.Modules.Outcomes.Queries;
using backend.Modules.Outcomes.DTOs;

namespace backend.Tests.Modules.Outcomes;

public class GetRoiSummaryQueryHandlerTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Handle_CalculatesTotalsCorrectly()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var handler = new GetRoiSummaryQueryHandler(context);
        var businessId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        context.BusinessOutcomes.AddRange(
            new BusinessOutcome { 
                BusinessId = businessId, 
                OutcomeType = OutcomeType.RevenueIncrease, 
                SourceType = OutcomeSourceType.Automation,
                SourceId = "1",
                RevenueImpact = 1000, 
                Confidence = OutcomeConfidence.High,
                OccurredAt = now.AddDays(-5),
                Status = OutcomeStatus.Active
            },
            new BusinessOutcome { 
                BusinessId = businessId, 
                OutcomeType = OutcomeType.CostReduction, 
                SourceType = OutcomeSourceType.ActionCenter,
                SourceId = "2",
                CostImpact = 500, 
                TimeSavedMinutes = 120,
                Confidence = OutcomeConfidence.Medium,
                OccurredAt = now.AddDays(-2),
                Status = OutcomeStatus.Active
            }
        );
        await context.SaveChangesAsync();

        var query = new GetRoiSummaryQuery
        {
            BusinessId = businessId,
            StartDate = now.AddDays(-10),
            EndDate = now,
            EstimatedHourlyCost = 100 // $100 per hour
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(1000m, result.RevenueGenerated);
        Assert.Equal(0m, result.RevenueRecovered);
        Assert.Equal(500m, result.CostSaved);
        Assert.Equal(120, result.TimeSavedMinutes);
        
        // 120 mins = 2 hours. 2 hours * 100 = 200
        Assert.Equal(200m, result.EstimatedTimeValue);
        
        Assert.Equal(1, result.SuccessfulAutomations);
        Assert.Equal(1, result.SuccessfulActions);
        
        Assert.True(result.ConfidenceSummary.ContainsKey("High"));
        Assert.True(result.ConfidenceSummary.ContainsKey("Medium"));
        Assert.Equal(1, result.ConfidenceSummary["High"]);
        Assert.Equal(1, result.ConfidenceSummary["Medium"]);
    }
}
