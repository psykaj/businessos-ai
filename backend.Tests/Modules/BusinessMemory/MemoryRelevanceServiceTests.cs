using System;
using System.Collections.Generic;
using System.Linq;
using backend.Modules.BusinessMemory.Entities;
using backend.Modules.BusinessMemory.Enums;
using backend.Modules.BusinessMemory.Services;
using Xunit;

namespace backend.Tests.Modules.BusinessMemory;

public class MemoryRelevanceServiceTests
{
    private readonly MemoryRelevanceService _sut;

    public MemoryRelevanceServiceTests()
    {
        _sut = new MemoryRelevanceService();
    }

    [Fact]
    public void ScoreAndRank_ReturnsHigherScoreForImportantMemories()
    {
        // Arrange
        var memories = new List<backend.Modules.BusinessMemory.Entities.BusinessMemory>
        {
            new backend.Modules.BusinessMemory.Entities.BusinessMemory { Id = Guid.NewGuid(), ContextData = "meeting notes", CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new backend.Modules.BusinessMemory.Entities.BusinessMemory { Id = Guid.NewGuid(), ContextData = "old meeting notes", CreatedAt = DateTime.UtcNow.AddDays(-40) },
            new BusinessMemory { Id = Guid.NewGuid(), Title = "High Imp", Content = "Test", Importance = MemoryImportance.Critical, Confidence = MemoryConfidence.Medium, CreatedAt = DateTime.UtcNow }
        };

        // Act
        var result = _sut.ScoreAndRank(memories, "Test", 10).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("High Imp", result[0].Title); // Critical comes first
    }

    [Fact]
    public void ScoreAndRank_KeywordMatchBoostsScore()
    {
        // Arrange
        var memories = new List<backend.Modules.BusinessMemory.Entities.BusinessMemory>
        {
            new backend.Modules.BusinessMemory.Entities.BusinessMemory { Id = Guid.NewGuid(), ContextData = "urgent client meeting", RelevanceScore = 80 },
            new backend.Modules.BusinessMemory.Entities.BusinessMemory { Id = Guid.NewGuid(), ContextData = "monthly report", RelevanceScore = 40 },
            new BusinessMemory { Id = Guid.NewGuid(), Title = "B", Content = "I like bananas", Importance = MemoryImportance.Medium, Confidence = MemoryConfidence.Medium, CreatedAt = DateTime.UtcNow }
        };

        // Act
        var result = _sut.ScoreAndRank(memories, "bananas", 10).ToList();

        // Assert
        Assert.Equal("B", result[0].Title); // B contains keyword
    }
}
