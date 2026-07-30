# Executive Decision Intelligence Platform

## Overview
The Executive Decision Intelligence Platform is the brain of BusinessOS AI. It integrates multiple business modules (CRM, Finance, etc.) to give executives a centralized command center where data becomes actionable intelligence.

## Key Modules
1. **KPI Engine**: Calculates real-time Key Performance Indicators across the organization (Revenue, Lead Conversion, Deal Size).
2. **Forecasting**: Predicts future trends using historical data and AI models.
3. **Executive Insights**: Generates automated, human-readable insights based on data anomalies or trends.
4. **Business Goals & Scorecards**: Tracks strategic goals and evaluates employee/department performance.
5. **Business Health**: Provides a unified score (0-100) combining Financial, Operational, and Customer health.
6. **Benchmarks**: Compares company metrics against industry standards.
7. **AI Recommendations**: Offers actionable steps to improve business outcomes.
8. **Decision Center**: Logs executive decisions for transparency and future evaluation.

## Architecture
- **Clean Architecture**: Services, Repositories, DTOs, and Controllers are completely isolated.
- **Background Processing**: Heavy KPI and Health calculations are offloaded to `IHostedService` background jobs.
- **Caching**: StackExchange.Redis is used to cache complex KPI and Forecast calculations, significantly improving API performance.

## Design Philosophy
- **Actionable Insights**: We don't just show charts; we tell the user what the data means and what to do next.
- **Business Value First**: Every feature must save time, increase revenue, reduce costs, or improve decisions.
