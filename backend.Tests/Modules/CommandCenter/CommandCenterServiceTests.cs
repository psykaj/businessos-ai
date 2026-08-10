using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using backend.Persistence;
using backend.Modules.CommandCenter.Services;
using backend.Modules.CommandCenter.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Modules.BusinessIntelligence.Services;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.ActionCenter.Queries;
using backend.Modules.ActionCenter.DTOs;
using backend.Modules.AiAgent.Executions.Interfaces;
using backend.Modules.AiAgent.Executions.DTOs;
using MediatR;
using backend.Entities;

namespace backend.Tests.Modules.CommandCenter;

public class CommandCenterServiceTests
{
    private readonly Mock<IBusinessHealthCalculator> _mockHealthCalculator;
    private readonly Mock<IProactiveAlertService> _mockAlertService;
    private readonly Mock<IBusinessBriefingService> _mockBriefingService;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IAiExecutionEngine> _mockAiExecutionEngine;
    private readonly Mock<ILogger<CommandCenterService>> _mockLogger;
    private readonly ApplicationDbContext _dbContext;
    private readonly CommandCenterService _service;

    public CommandCenterServiceTests()
    {
        _mockHealthCalculator = new Mock<IBusinessHealthCalculator>();
        _mockAlertService = new Mock<IProactiveAlertService>();
        _mockBriefingService = new Mock<IBusinessBriefingService>();
        _mockMediator = new Mock<IMediator>();
        _mockAiExecutionEngine = new Mock<IAiExecutionEngine>();
        _mockLogger = new Mock<ILogger<CommandCenterService>>();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        _dbContext = new ApplicationDbContext(options);

        _service = new CommandCenterService(
            _dbContext,
            _mockHealthCalculator.Object,
            _mockAlertService.Object,
            _mockBriefingService.Object,
            _mockMediator.Object,
            _mockAiExecutionEngine.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task GetCommandCenterSummary_WhenHealthFails_GracefulDegradation_ReturnsOtherData()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var userId = "user123";

        // Mock Business Health to throw an exception
        _mockHealthCalculator
            .Setup(x => x.CalculateHealthAsync(orgId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Business health service down"));

        // Mock Alerts to succeed
        _mockAlertService
            .Setup(x => x.GetUnreadAlertsAsync(orgId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProactiveAlertDto>
            {
                new ProactiveAlertDto { Id = Guid.NewGuid(), Title = "Test Alert" }
            });

        // Mock Actions to succeed
        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetPendingActionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AiActionDto>());

        // Mock Briefing to succeed
        _mockBriefingService
            .Setup(x => x.GetTodayBriefingAsync(orgId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BusinessBriefingDto { OrganizationId = orgId });

        // Act
        var result = await _service.GetCommandCenterSummaryAsync(orgId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.BusinessHealth); // Gracefully failed
        Assert.NotNull(result.PriorityAlerts);
        Assert.Single(result.PriorityAlerts);
        Assert.NotNull(result.ExecutiveSummary);
    }

    [Fact]
    public async Task AskBusinessOsAi_ReturnsActionableAnswer()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var userId = "user123";
        var request = new CommandCenterAskRequestDto { Question = "Why did sales drop?" };

        _mockAiExecutionEngine
            .Setup(x => x.ExecuteCommandAsync(orgId, userId, It.IsAny<ExecuteCommandRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ExecutionResponseDto
            {
                ResultSummary = "Sales dropped because of X.",
            });

        // Act
        var result = await _service.AskBusinessOsAiAsync(orgId, userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sales dropped because of X.", result.Answer);
        Assert.Equal("High", result.Confidence);
    }

    [Fact]
    public async Task AskBusinessOsAi_WhenAiFails_GracefulDegradation()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var userId = "user123";
        var request = new CommandCenterAskRequestDto { Question = "Why did sales drop?" };

        _mockAiExecutionEngine
            .Setup(x => x.ExecuteCommandAsync(orgId, userId, It.IsAny<ExecuteCommandRequestDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("AI down"));

        // Act
        var result = await _service.AskBusinessOsAiAsync(orgId, userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("I'm currently unable to process your request due to a technical issue. Please try again later.", result.Answer);
        Assert.Equal("Low", result.Confidence);
    }
}
