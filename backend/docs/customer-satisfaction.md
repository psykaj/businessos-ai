# Customer Satisfaction (CSAT) Documentation

## Overview
The Customer Satisfaction module enables SMEs to capture customer feedback across multiple channels (Web, InApp, Email, SMS), analyze CSAT trends, and proactively prevent churn.

## Key Capabilities
1. **Feedback Submission**: Standardized CSAT rating (1 to 5 stars) and feedback text capture.
2. **Distribution & Averages**: Compute real-time average CSAT ratings and 1-star to 5-star distribution counts.
3. **Negative Feedback Alerts**: Automatically creates a high-priority `SuccessTask` whenever a customer submits a rating <= 2 stars.
4. **Health Score Integration**: Connects rating metrics directly to the Customer Health Engine.
5. **NPS Readiness**: Schema prepared for Net Promoter Score (`FeedbackType = "NPS"`) expansion.
