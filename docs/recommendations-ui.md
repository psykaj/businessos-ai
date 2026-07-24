# AI Recommendations Dashboard UI

## Overview
The **Recommendations Dashboard** (`/dashboard/copilot/recommendations`) presents proactive revenue-generating and cost-reducing suggestions to business owners.

## Key Features
- **Real-Time Data Analysis**: "Run AI Analysis Scan" triggers a live scan of tenant CRM, Billing, and Campaign databases (`POST /api/ai-agent/recommendations/analyze`).
- **Priority & Category Filtering**: Filter suggestions by High/Medium/Low priority or business module.
- **One-Click Action Execution**: "Apply Action" executes the suggested business action and updates recommendation status.
- **Impact Explanations**: Highlights reason, business impact, and concrete suggested actions.
