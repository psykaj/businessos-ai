# Customer Feedback & CSAT UI Documentation

## Overview
The Customer Feedback UI enables businesses to capture customer satisfaction scores, track rating distributions, and receive automated alerts on negative reviews.

## Interface Components
1. **CSAT Rating Gauge & Breakdown**: Overall average score out of 5.0 and percentage of positive vs negative feedback.
2. **Distribution Chart**: Visual star rating breakdown bar chart (1 to 5 stars).
3. **Negative Feedback Alert Box**: Automated churn prevention warning panel highlighting automated task generation for ratings <= 2.
4. **Feedback Submission Dialog**: Modal dialog to record manual customer ratings and comments.

## Component Files
- `frontend/app/dashboard/customer-feedback/page.tsx`
- `frontend/components/customer-success/csat-distribution-chart.tsx`
