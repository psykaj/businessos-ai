# Customer Referral UI Documentation

## Overview
The Referral UI provides referral code generation, link sharing, referral conversion tracking, and reward payout management.

## Interface Components
1. **Referral Code Generator**: Dialog modal to create unique referral codes (`REF-XXXX-XXXXXX`).
2. **Referral Conversion Dialog**: Links referred customer accounts to active codes and automatically dispatches rewards to referrers.
3. **Acquisition Funnel Chart**: Visual breakdown of Referral Links Shared -> Converted Customers -> Rewards Distributed.
4. **Top Advocates Panel**: Leaderboard highlighting top customer advocates driving new acquisition.

## Component Files
- `frontend/app/dashboard/referrals/page.tsx`
- `frontend/components/customer-success/referral-funnel-chart.tsx`
