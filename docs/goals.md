# Goal Tracking Architecture

The **Goal Tracking Engine** empowers organizations to set quantitative targets and track real-time progress automatically calculated against operational metrics.

## Key Features
- **Flexible Target Definitions**: Define Revenue, Lead, Customer, or Subscription targets.
- **Automated KPI Syncing**: Periodically maps calculated KPIs to active organizational goals.
- **Automated Status Progression**: Updates status automatically (`NotStarted`, `InProgress`, `Achieved`, `Behind`, `Failed`).

## API Endpoints
- `GET /api/v1/bi/goals`: List organizational goals
- `GET /api/v1/bi/goals/{id}`: Get goal by ID
- `POST /api/v1/bi/goals`: Create a new business goal
- `PUT /api/v1/bi/goals/{id}`: Update an existing goal
- `DELETE /api/v1/bi/goals/{id}`: Delete a goal
- `POST /api/v1/bi/goals/sync`: Synchronize goal progress with live calculated KPIs
