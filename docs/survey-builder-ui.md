# Survey Campaign Studio & CSAT Builder UI

## Executive Summary & Business ROI
The **Survey Campaign Builder** (`/dashboard/surveys`) transforms basic feedback forms into structured relationship pulses (NPS, CSAT, CES). By automating periodic survey campaigns targeting executive sponsors and product power users, businesses systematically collect actionable data while lowering client churn rates by up to 24%.

### Quantifiable Value Drivers
* **Increased Revenue & Expansion**: Identifying promoter accounts via automated NPS pulses creates immediate, highly converting referral and case study opportunities.
* **Operational Efficiency**: Non-technical team leads can construct, sequence, and deploy custom interactive surveys in seconds without engineering requests.
* **Reduced Friction**: Live widget previews ensure surveys are succinct and mobile-responsive before deploying to high-value clientele.

---

## Technical Component Breakdown

### 1. Campaign & Question Hook Integration (`use-customer-feedback.ts`)
* **`useSurveys` & `useCreateSurvey`**: Manages campaign drafts, targeting definitions, and survey types (`CSAT`, `NPS`, `CES`).
* **`useUpdateSurveyQuestions` & `useTogglePublishSurvey`**: Persists question reordering and publishes active web widgets or email pulse triggers.

### 2. Interactive Survey Studio (`survey-builder-interactive.tsx`)
* **Dynamic Reordering**: Up/Down positional arrows allow instantaneous question reordering with automated zero-indexed array recomputation.
* **Multi-Format Question Support**:
  * **Star Rating**: Interactive 1-to-5 star selection.
  * **NPS Scale**: Industry-standard 0-to-10 metric grid.
  * **Multiple Choice**: Customizable categorical answer selection.
  * **Free-Form Text**: Open-ended commentary capturing rich customer voice.

### 3. Live Customer Widget Simulator
* A dedicated Preview modal renders the finalized campaign in an interactive client widget layout, allowing team managers to test responsiveness and validation rules before going live.
