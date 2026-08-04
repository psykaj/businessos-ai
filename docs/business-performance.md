# Business Performance Engine & KPIs - Architecture & Mathematical Formulation

The **Business Performance Engine** in **BusinessOS AI** provides real-time enterprise visibility into financial efficiency, operational productivity, and customer profitability. Designed around the mandatory **Business Value Rule**, every computed metric directly aids in reducing operational costs, increasing revenue, saving owner time, or improving decision accuracy.

---

## 1. System Architecture

The module operates on a clean, decoupled architecture:
* **Entities**: `BusinessMetric` stores historical snapshot points across time series periods (`Daily`, `Weekly`, `Monthly`, `Quarterly`, `Yearly`), indexed heavily by `(OrganizationId, Category, MetricName, Timestamp)`.
* **Services**: `BusinessPerformanceService` injects `ApplicationDbContext`, `IMapper`, and Redis `IDistributedCache` for real-time dashboard computation with 15-minute sliding TTL cache warming.
* **Background Jobs**: `BusinessPerformanceCalculationJob` periodically iterates over active tenant organizations to warm up cache computations asynchronously, ensuring instant page-load performance for executive users without database load spikes.

---

## 2. Mathematical Formulas & Core Calculations

### A. Monthly Recurring Revenue (MRR) & Annualized Run Rate (ARR)
* $$\text{MRR} = \sum (\text{Active Subscription Monthly Equivalent Recurring Value})$$
* $$\text{ARR} = \text{MRR} \times 12$$

### B. Average Order Value (AOV) & Customer Lifetime Value (LTV)
* $$\text{AOV} = \frac{\sum \text{Total Revenue Generated}}{\text{Total Invoice Transactions Count}}$$
* $$\text{LTV} = \left( \frac{\text{AOV} \times \text{Purchase Frequency}}{\text{Customer Churn Rate}} \right) \times \text{Gross Margin \%}$$
* *Simplified Empirical Model Used*: $$\text{LTV} \approx \left(\frac{\text{Total Revenue}}{\text{Active Customers}}\right) \times 3.5 \times \text{Gross Margin \%}$$

### C. Customer Acquisition Cost (CAC) & LTV/CAC Ratio
* $$\text{CAC} = \frac{\sum \text{Marketing Spend} + \sum \text{Sales Operational Costs}}{\text{New Customers Acquired in Period}}$$
* $$\text{LTV : CAC Ratio} = \frac{\text{Customer Lifetime Value (LTV)}}{\text{Customer Acquisition Cost (CAC)}}$$
* **Benchmark Thresholds**:
  * $< 1.0\text{x}$: Destroying capital on every acquisition.
  * $1.0\text{x} - 2.5\text{x}$: Underperforming; optimization needed.
  * $3.0\text{x} - 5.0\text{x}$: **Target Healthy SaaS Range**.
  * $> 5.0\text{x}$: Under-investing in marketing; accelerate growth budget immediately.

### D. Revenue Growth & Repeat Customer Rate
* $$\text{YoY Revenue Growth (\%)} = \frac{\text{Revenue}_{\text{current period}} - \text{Revenue}_{\text{previous prior period}}}{\text{Revenue}_{\text{previous prior period}}} \times 100$$
* $$\text{Repeat Customer Rate (\%)} = \frac{\text{Customers with } \ge 2 \text{ Purchases}}{\text{Total Unique Customers}} \times 100$$

---

## 3. Performance Optimization Techniques for Complex Queries

To evaluate thousands of financial transactions, customer orders, and telemetry records without locking OLTP tables:
1. **As No-Tracking Read Queries**: All analytical evaluation paths execute via EF Core `.AsNoTracking()` to eliminate Entity Change Tracker memory overhead and speed up materialization by ~40%.
2. **Database Projection & Aggregation**: Queries push `Sum()`, `Count()`, and `Average()` operations directly down to PostgreSQL via SQL translation rather than pulling raw entities into application RAM.
3. **Redis Caching Pipeline**: Computed KPI summaries are written to Redis clusters under keys formatted as `BusinessPerformance_Dashboard_{OrganizationId}` with 15-minute absolute expiration, decoupling reads from primary PostgreSQL execution paths during peak traffic periods.
