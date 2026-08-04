# Customer Satisfaction & Churn Risk Analytics

## Revenue Retention & Decision Engine
* **Increase Revenue & Retention**: Predicts customer churn before accounts cancel by analyzing CSAT drops, negative NPS scores, and repeat complaint patterns.
* **Save Executive Time**: Eliminates spreadsheet calculations by aggregating real-time organization dashboards for Net Promoter Score and overall CSAT percentages.
* **Actionable Churn Alerting**: Classifies every tracked account into clear risk cohorts (`Low`, `Medium`, `High`, `Critical`) for proactive account management intervention.

---

## Scoring Formulas & Calculations

### CSAT (Customer Satisfaction Score)
Calculated across all historical ratings and survey responses for an individual customer:
$$\text{CSAT \%} = \left( \frac{\text{Average Rating}}{5.0} \right) \times 100$$

### NPS (Net Promoter Score Approximation)
Categorizes responses into Promoters (9-10), Passives (7-8), and Detractors (0-6), calculating the aggregate differential:
$$\text{NPS Score} = \% \text{ Promoters} - \% \text{ Detractors}$$

### Automated Churn Risk Classification
The `CsatService` automatically re-evaluates customer retention risk upon every feedback event:
* **Critical Risk**: NPS $< 0$ OR Repeat Complaint Rate $> 40\%$
* **High Risk**: NPS $< 20$ OR Repeat Complaint Rate $> 20\%$
* **Medium Risk**: CSAT $< 75\%$
* **Low Risk**: CSAT $\ge 75\%$ with low repeat complaint metrics

---

## API Dashboards
* `GET /api/v1/csat-metrics/dashboard?organizationId={id}`: Executive overview of overall CSAT %, NPS Score, Resolution Satisfaction rate, and count of High/Critical churn risk accounts.
* `GET /api/v1/csat-metrics/customer/{customerId}`: Deep dive into individual account sentiment trends and historical complaint frequency.
