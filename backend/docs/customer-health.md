# Customer Health Engine Documentation

## Overview
The Customer Health Engine automatically computes account health scores (0-100) and risk levels based on multi-variable telemetry and activity tracking.

## Calculation Formula & Weights
1. **Base Baseline**: 80 Points
2. **Interaction Recency**:
   - > 60 days inactive: -30 points
   - > 30 days inactive: -15 points
   - <= 7 days active: +10 points
3. **Purchase Frequency**:
   - +3 points per purchase (max +20)
   - Last purchase > 90 days ago: -20 points; <= 14 days ago: +10 points
4. **Lifetime Value (LTV)**:
   - LTV > $1,000: +15 points
   - LTV > $500: +10 points
   - LTV > $100: +5 points
5. **Penalties**:
   - Outstanding payments > $500: -25 points; > $0: -10 points
   - Support ticket volume: -5 points per ticket (max -25)
6. **CSAT Factors**:
   - Rating >= 4.5: +15 points
   - Rating <= 2.0: -25 points
7. **Referral Activity**:
   - +5 points per successful referral (max +15)

## Risk Level Mapping
- **80 – 100**: `Healthy`
- **60 – 79**: `Stable`
- **40 – 59**: `Needs Attention`
- **0 – 39**: `High Risk` (Triggers automated `SuccessTask` alert)
