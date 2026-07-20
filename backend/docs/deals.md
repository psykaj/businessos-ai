# Deals & Sales Pipeline

Deals represent ongoing sales opportunities with an estimated value and close date.

## Sales Pipeline Stages

The pipeline is fully configurable but defaults to:
1. New Lead
2. Contacted
3. Qualified
4. Proposal Sent
5. Negotiation
6. Won
7. Lost

## Deal Stage History

Every time a deal moves from one stage to another, a record is added to the `DealStageHistories` table. This powers reporting and analytics on sales velocity and bottlenecks.

## Endpoints

- `GET /api/crm/deals`
- `POST /api/crm/deals`
- `PATCH /api/crm/deals/{id}/stage` - Move a deal to a new stage.
