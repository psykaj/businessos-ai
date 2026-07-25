# Loyalty & Rewards Program Documentation

## Overview
The Loyalty & Rewards Engine provides flexible customer retention incentives through configurable point earning, redemption, and milestone tracking.

## Core Features
1. **Multi-Program Support**: Create and manage multiple active, draft, or archived loyalty programs per organization.
2. **Point Earning Logic**: Automatically calculate points earned based on `PointsPerPurchase` multiplier and transaction value.
3. **Point Redemption**: Validation of minimum point thresholds and balance verification before redemption.
4. **Point Expiration**: Configurable `PointsExpiryDays` for automated point expiration tracking.
5. **Manual Adjustments**: Admin capability to grant bonus points or adjust balances with mandatory audit reasons.
6. **Milestone Tasks**: Automatic generation of `SuccessTask` notifications when customers cross key balance milestones (e.g., 1,000 points).
