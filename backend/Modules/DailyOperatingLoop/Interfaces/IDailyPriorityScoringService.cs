using System;
using System.Collections.Generic;
using backend.Modules.DailyOperatingLoop.Entities;

namespace backend.Modules.DailyOperatingLoop.Interfaces;

public interface IDailyPriorityScoringService
{
    DailyPriority ScorePriority(DailyPriority priority);
    List<DailyPriority> RankPriorities(IEnumerable<DailyPriority> priorities, int limit = 5);
}
