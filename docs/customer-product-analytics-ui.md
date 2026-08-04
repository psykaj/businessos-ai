# Customer & Product Analytics UI & Action Framework

## Overview
The Customer & Product Analytics engines (`/dashboard/customer-analytics` and `/dashboard/product-analytics`) transform passive user tables and inventory catalogs into proactive financial optimization tools. Designed to maximize unit economics and eliminate operational loss centers, these interfaces adhere to the **Mandatory Business Value Rule**.

---

## Customer LTV & Churn Risk Intelligence (`/dashboard/customer-analytics`)

### Key Metrics & Unit Economics
- **LTV : CAC Ratio**: Calculates the ratio of total Lifetime Value ($LTV$) against Customer Acquisition Cost ($CAC$). Accounts are flagged with warning badges if their unit economic ratio drops below `3.0x`.
- **Repeat Purchase & Retention Rate**: Evaluates historical repeat buying cycles and active platform login cadence to output an algorithmic Churn Risk Score (`Low`, `Medium`, `High`, `Critical`).

### Interactive VIP Win-Back Workflows
- **Individual Action Hooks**: In the `LtvCacMatrix` component, accounts demonstrating critical churn risk display a prominent **"Trigger Win-Back Call"** button. Activating this hook invokes an optimistic AI sequence that dispatches priority check-in messages and queues success interventions.
- **Bulk VIP Win-Back Dispatcher**: At the header level, executive users can execute **"Trigger All VIP Win-Back Calls"**, protecting at-risk annual recurring commitments with a single interaction.
- **One-Click Upgrade Expansion**: For low-risk accounts approaching workflow capacity limits, the system provides a **"Send Upgrade Offer"** button that emails one-click discount acceptance links via Stripe API integrations.

---

## Product Margin Ranking & Stock Optimization (`/dashboard/product-analytics`)

### Gross Margin Ranking Matrix
The `MarginRankingTable` evaluates products not merely by top-line revenue, but strictly by **Gross Margin Percentage ($GM\%$)**, computed as:

$$GM\% = \frac{\text{Unit Price} - \text{Unit Cost}}{\text{Unit Price}} \times 100$$

### Automated Action Hooks
- **Loss Center Detection & Sunsetting**: Products operating at a negative margin (e.g., Legacy Custom Scripting Support at `-20%` due to excessive engineer labor hours) are highlighted with flashing warning indicators. The **"Sunset Loss Center & Save $19.2k"** action immediately prevents new subscription creation and transitions current accounts to automated AI Copilot tiers.
- **Infrastructure Reserve Reordering**: When physical hardware or cloud infrastructure reserve units drop below safety thresholds, users can activate **"Reorder Reserve Stock"** to automatically generate vendor purchase orders ahead of anticipated billing surges.
