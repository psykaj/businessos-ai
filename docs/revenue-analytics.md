# Revenue Analytics, Retention & MRR/ARR Methodology

The **Revenue Analytics Module** delivers audit-grade visibility into subscription momentum, expansion revenue streams, revenue leakage, and customer lifetime monetary flows.

---

## 1. MRR & ARR Component Decomposition

Monthly Recurring Revenue (MRR) is separated into distinct evolutionary components:
* **New MRR**: Revenue added from net-new logo acquisition within the active calendar month.
* **Expansion MRR**: Incremental revenue gained from existing subscribers upgrading tiers or expanding license seats.
* **Contraction MRR**: Revenue lost when existing customers downgrade subscription plans or drop seat counts without full cancellation.
* **Churned MRR**: Recurring revenue completely erased when accounts terminate their subscription agreements.

$$\text{Net New MRR} = \text{New MRR} + \text{Expansion MRR} - \text{Contraction MRR} - \text{Churned MRR}$$

$$\text{Current Ending MRR} = \text{Starting MRR} + \text{Net New MRR}$$

---

## 2. Retention Analytics: NRR vs. GRR

### A. Net Revenue Retention (NRR)
NRR measures the compound revenue trajectory of a historical cohort of customers over a defined period, including upgrades, cross-sells, downgrades, and churns.
$$\text{NRR (\%)} = \frac{\text{Starting MRR} + \text{Expansion MRR} - \text{Contraction MRR} - \text{Churned MRR}}{\text{Starting MRR}} \times 100$$
* **Best Practice**: Top-quartile Enterprise SaaS companies achieve **>120% NRR**, meaning the business grows by 20% year-over-year from existing accounts alone even with zero net-new sales.

### B. Gross Revenue Retention (GRR)
GRR isolates revenue preservation ability by excluding expansion upgrades, providing an unvarnished view of customer loyalty and product utility.
$$\text{GRR (\%)} = \frac{\text{Starting MRR} - \text{Contraction MRR} - \text{Churned MRR}}{\text{Starting MRR}} \times 100 \quad (\text{Max } 100\%)$$

---

## 3. Predictive Churn & Revenue Expansion Models

* **Revenue Snapshot Background Job**: `RevenueSnapshotJob` executes daily across all tenant schemas, freezing historical MRR/ARR states into immutable `RevenueSnapshot` records to prevent historical rewriting when contracts alter later.
* **Expansion Opportunity Flagging**: The analytical engine continually checks active accounts where usage utilization approaches >85% of tier license entitlements, surfacing structured growth opportunities directly to the executive command center.
