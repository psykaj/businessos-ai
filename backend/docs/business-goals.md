# Business Goals & Scorecards

## Business Goals
Tracks top-level company objectives (e.g., "Reach $1M ARR", "Reduce Churn by 5%").
- **Metrics**: Each goal has a `TargetValue` and `CurrentValue`.
- **Tracking**: `GoalTrackingService` updates the progress dynamically based on data from other modules.

## Scorecards
Evaluates the performance of specific employees, teams, or departments.
- **Structure**: Links to a `TargetId` (e.g., Sales Team) and an `OwnerId` (e.g., VP of Sales).
- **Scores**: Contains a `Score` (0-100) and `EvaluationPeriod` to track performance over time.

## Integration
Scorecards and Business Goals feed directly into the **Business Health** module, providing a holistic view of whether the company is executing its strategy effectively.
