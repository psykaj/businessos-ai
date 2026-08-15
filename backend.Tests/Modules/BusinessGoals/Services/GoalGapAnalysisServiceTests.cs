using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Entities;
using backend.Modules.BusinessGoals.Services;
using Moq;
using Xunit;

namespace backend.Tests.Modules.BusinessGoals.Services;

public class GoalGapAnalysisServiceTests
{
    private readonly Mock<IGoalProjectionService> _projectionServiceMock;
    private readonly GoalGapAnalysisService _service;

    public GoalGapAnalysisServiceTests()
    {
        _projectionServiceMock = new Mock<IGoalProjectionService>();
        _service = new GoalGapAnalysisService(_projectionServiceMock.Object);
    }

    [Fact]
    public async Task AnalyzeGapAsync_ShouldReturnCorrectGap_WhenTargetNotMet()
    {
        var goal = new BusinessGoal
        {
            TargetValue = 100,
            CurrentValue = 40
        };

        _projectionServiceMock.Setup(s => s.ProjectGoalOutcomeAsync(goal, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProjectionResult
            {
                Status = "Behind",
                ProjectedValue = 80,
                CurrentRate = 4,
                RequiredDailyRate = 6
            });

        var result = await _service.AnalyzeGapAsync(goal, CancellationToken.None);

        Assert.Equal(60, result.Gap);
        Assert.Equal("Behind", result.Risk);
        Assert.Equal(80, result.ProjectedValue);
        Assert.Contains("below the required", result.PrimaryDrivers);
    }
}
