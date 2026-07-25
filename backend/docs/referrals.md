# Customer Referral System Documentation

## Overview
The Referral System drives organic customer acquisition by turning satisfied customers into brand advocates through unique referral codes and reward tracking.

## Workflow & Features
1. **Code Generation**: Automated generation of unique referral codes (`REF-{OrgPrefix}-{ShortGuid}`).
2. **Referral Conversion**: Link referred customers during onboarding or purchase checkout.
3. **Reward Disbursement**: Automatically credit referrer loyalty points or account balance upon conversion (`RewardIssued = true`).
4. **Health Integration**: Successful referrals boost the referrer's `ReferralCount` metric and `HealthScore`.
5. **Analytics & Tracking**: Real-time tracking of pending, converted, and rewarded referrals alongside total reward payouts and conversion percentages.
