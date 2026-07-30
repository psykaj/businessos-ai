# Forecasting Dashboard UI

## Overview
The Forecasting Dashboard (`/app/dashboard/forecasting`) visualizes future projections for critical metrics (Revenue, Expenses, Profit).

## Key Features
- **Metric Selector**: Toggle between different forecasted metrics (Revenue, Expenses, Profit).
- **Forecast Summary Card**: Displays the predicted value, lower/upper bounds, and an AI confidence score with a visual progress bar.
- **Trend & Forecast Chart**: A `ComposedChart` combining historical actuals (solid line), future predictions (dashed line), and the confidence interval (shaded area).
- **Regenerate Forecast**: Allows users to explicitly request a new forecast calculation from the AI engine.

## Technical Implementation
- `ForecastChart` component uses Recharts (`ComposedChart`, `Line`, `Area`).
- State is managed via React Query (`useLatestForecast`) and mutations (`useGenerateForecast`).
- Dynamic charting merges historical mock data with the AI's predicted data points to create a seamless timeline.
