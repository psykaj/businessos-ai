# Forecasting Engine Architecture

The **Forecasting Engine** provides predictive analytics to help SMEs anticipate revenue, deal pipelines, customer acquisition, and subscription retention.

## Supported Forecast Types
- **Revenue Forecast**: Predicts total income over a 30, 60, or 90 day horizon.
- **Sales Forecast**: Projects won deal values based on active pipeline stage conversions.
- **Lead Forecast**: Anticipates inbound lead volume from historical marketing trends.
- **Customer Growth Forecast**: Models user base growth based on retention rates.
- **Subscription Forecast**: Projects recurring monthly revenue stability and renewals.

## API Endpoints
- `GET /api/v1/bi/forecasting/{type}`: Retrieve existing predictive data points
- `POST /api/v1/bi/forecasting/generate`: Generate fresh predictions for a specific horizon
