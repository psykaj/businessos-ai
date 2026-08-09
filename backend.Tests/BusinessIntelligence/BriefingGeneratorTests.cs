using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.AI.Interfaces;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;
using backend.Modules.BusinessIntelligence.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace backend.Tests.BusinessIntelligence
{
    public class BriefingGeneratorTests
    {
        private readonly Mock<IBusinessInsightAggregator> _mockAggregator;
        private readonly Mock<IInsightPrioritizer> _mockPrioritizer;
        private readonly Mock<IAIService> _mockAiService;
        private readonly Mock<ILogger<BriefingGenerator>> _mockLogger;
        private readonly BriefingGenerator _generator;

        public BriefingGeneratorTests()
        {
            _mockAggregator = new Mock<IBusinessInsightAggregator>();
            _mockPrioritizer = new Mock<IInsightPrioritizer>();
            _mockAiService = new Mock<IAIService>();
            _mockLogger = new Mock<ILogger<BriefingGenerator>>();

            _generator = new BriefingGenerator(
                _mockAggregator.Object,
                _mockPrioritizer.Object,
                _mockAiService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GenerateBriefingAsync_ShouldGenerateFallbackSummary_WhenAiServiceFails()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var date = DateTime.UtcNow.Date;
            
            var items = new List<BriefingItem>
            {
                new BriefingItem { Type = BriefingItemType.Risk, Priority = 90 },
                new BriefingItem { Type = BriefingItemType.Opportunity, Priority = 60 }
            };

            _mockAggregator.Setup(a => a.GatherInsightsAsync(orgId, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(items);
                
            _mockPrioritizer.Setup(p => p.Prioritize(It.IsAny<List<BriefingItem>>(), 15))
                .Returns(items);
                
            _mockAiService.Setup(a => a.ChatCompletionAsync(orgId, It.IsAny<string>(), It.IsAny<string>(), null))
                .ThrowsAsync(new Exception("AI service unavailable"));

            // Act
            var result = await _generator.GenerateBriefingAsync(orgId, date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(BriefingStatus.Generated, result.Status);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.RiskCount);
            Assert.Equal(1, result.OpportunityCount);
            Assert.Contains("Calculated manually due to AI unavailability.", result.RevenueSummary);
        }
        
        [Fact]
        public async Task GenerateBriefingAsync_ShouldParseAiSummary_WhenAiServiceSucceeds()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var date = DateTime.UtcNow.Date;
            
            var items = new List<BriefingItem>();

            _mockAggregator.Setup(a => a.GatherInsightsAsync(orgId, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(items);
                
            _mockPrioritizer.Setup(p => p.Prioritize(It.IsAny<List<BriefingItem>>(), 15))
                .Returns(items);
                
            string aiResponse = @"{
                ""summary"": ""Great day today!"",
                ""businessHealthScore"": 95,
                ""revenueSummary"": ""Revenue is up."",
                ""customerSummary"": ""Customers are happy."",
                ""financeSummary"": ""Finances are stable."",
                ""inventorySummary"": ""Inventory is full.""
            }";
                
            _mockAiService.Setup(a => a.ChatCompletionAsync(orgId, It.IsAny<string>(), It.IsAny<string>(), null))
                .ReturnsAsync(aiResponse);

            // Act
            var result = await _generator.GenerateBriefingAsync(orgId, date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Great day today!", result.Summary);
            Assert.Equal(95, result.BusinessHealthScore);
            Assert.Equal("Revenue is up.", result.RevenueSummary);
        }
    }
}
