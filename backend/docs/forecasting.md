# AI Forecasting

## Overview
The Forecasting module predicts future business metrics based on historical trends. It serves as the foundation for the predictive analytics capability of the Executive Intelligence suite.

## AI Models (Current & Future)
- **Current**: A mock ARIMA-based calculation (`AI_ARIMA_V1`) that analyzes recent revenue data and extrapolates with a slight growth factor and randomized confidence intervals.
- **Future**: Designed to seamlessly integrate with ML.NET or external AI APIs (OpenAI, AWS Forecast) by swapping the implementation in `ForecastingService`.

## Database Entities
- `Forecast`: Stores predicted values (`PredictedValue`, `LowerBound`, `UpperBound`) for specific time periods (`TargetDate`), allowing comparison against actuals.

## Endpoints
- `GET /api/v1/forecasting/generate`: Generates a new forecast for a metric.
- `GET /api/v1/forecasting/latest`: Retrieves the most recently generated forecast.
