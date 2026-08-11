using System;
using System.Collections.Generic;
using System.Linq;
using backend.Modules.BusinessMemory.Entities;
using backend.Modules.BusinessMemory.Interfaces;
using backend.Modules.BusinessMemory.Enums;

namespace backend.Modules.BusinessMemory.Services;

public class MemoryRelevanceService : IMemoryRelevanceService
{
    public IEnumerable<Entities.BusinessMemory> ScoreAndRank(
        IEnumerable<Entities.BusinessMemory> memories, 
        string query, 
        int topN)
    {
        if (memories == null || !memories.Any())
            return Array.Empty<Entities.BusinessMemory>();

        var scoredMemories = memories.Select(m => new
        {
            Memory = m,
            Score = CalculateScore(m, query)
        })
        .OrderByDescending(x => x.Score)
        .Take(topN)
        .Select(x => x.Memory)
        .ToList();

        return scoredMemories;
    }

    private double CalculateScore(Entities.BusinessMemory memory, string query)
    {
        double score = 0;

        // 1. Recency Score (Decays over time)
        var daysOld = (DateTime.UtcNow - memory.CreatedAt).TotalDays;
        score += Math.Max(0, 10 - (daysOld * 0.1)); // Older memories get slightly lower score

        // 2. Importance Score
        score += memory.Importance switch
        {
            MemoryImportance.Critical => 15,
            MemoryImportance.High => 10,
            MemoryImportance.Medium => 5,
            MemoryImportance.Low => 2,
            _ => 0
        };

        // 3. Confidence Score
        score += memory.Confidence switch
        {
            MemoryConfidence.High => 10,
            MemoryConfidence.Medium => 5,
            MemoryConfidence.Low => 1,
            _ => 0
        };

        // 4. Keyword Match (Basic Relevance)
        // In a real system, this would be replaced/augmented by Vector Embeddings (pgvector/Azure AI Search)
        if (!string.IsNullOrWhiteSpace(query))
        {
            var keywords = query.ToLower().Split(new[] { ' ', '.', ',', '?' }, StringSplitOptions.RemoveEmptyEntries);
            var contentLower = memory.Content.ToLower();
            var titleLower = memory.Title.ToLower();
            
            int matches = keywords.Count(k => contentLower.Contains(k) || titleLower.Contains(k));
            score += matches * 2;
        }

        return score;
    }
}
