using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.Entities;
using backend.Modules.BusinessGoals.Services;
using Xunit;

namespace backend.Tests.Modules.BusinessGoals.Services;

public class GoalProjectionServiceTests
{
    private readonly GoalProjectionService _service;

    public GoalProjectionServiceTests()
    {
        _service = new GoalProjectionService();
    }

    [Fact]
    public async Task ProjectGoalOutcomeAsync_ShouldReturnAchieved_WhenTargetIsMet()
    {
        var goal = new BusinessGoal
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            TargetDate = DateTime.UtcNow.AddDays(10),
            TargetValue = 100,
            CurrentValue = 100
        };

        var result = await _service.ProjectGoalOutcomeAsync(goal, CancellationToken.None);

        Assert.Equal("Achieved", result.Status);
        Assert.Equal(0, result.RequiredDailyRate);
        Assert.Equal(200, Math.Round(result.ProjectedValue));
    }

    [Fact]
    public async Task ProjectGoalOutcomeAsync_ShouldCalculateRequiredRate_WhenOnTrack()
    {
        var goal = new BusinessGoal
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            TargetDate = DateTime.UtcNow.AddDays(10),
            TargetValue = 100,
            CurrentValue = 50 // Exactly half way
        };

        var result = await _service.ProjectGoalOutcomeAsync(goal, CancellationToken.None);

        Assert.Equal("OnTrack", result.Status);
        Assert.Equal(5, result.RequiredDailyRate, 1);
        Assert.Equal(5, result.CurrentRate, 1);
        Assert.Equal(100, result.ProjectedValue, 1);
    }

    [Fact]
    public async Task ProjectGoalOutcomeAsync_ShouldReturnBehind_WhenPaceIsSlow()
    {
        var goal = new BusinessGoal
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            TargetDate = DateTime.UtcNow.AddDays(10),
            TargetValue = 100,
            CurrentValue = 20 // Slower pace
        };

        var result = await _service.ProjectGoalOutcomeAsync(goal, CancellationToken.None);

        Assert.Equal("Behind", result.Status);
        Assert.Equal(8, result.RequiredDailyRate, 1);
        Assert.Equal(2, result.CurrentRate, 1);
        Assert.Equal(40, result.ProjectedValue, 1);
    }
}
