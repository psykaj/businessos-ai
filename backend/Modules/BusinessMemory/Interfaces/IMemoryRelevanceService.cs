using System.Collections.Generic;
using backend.Modules.BusinessMemory.Entities;

namespace backend.Modules.BusinessMemory.Interfaces;

public interface IMemoryRelevanceService
{
    IEnumerable<Entities.BusinessMemory> ScoreAndRank(
        IEnumerable<Entities.BusinessMemory> memories, 
        string query, 
        int topN);
}
