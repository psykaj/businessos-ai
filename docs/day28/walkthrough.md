# Day 28: AI Automation & Workflow Engine

I have successfully implemented the AI Automation & Workflow Engine!

The new engine provides a robust, state-machine-driven automation pipeline capable of evaluating complex conditions and making intelligent AI-driven business decisions.

## What was built

### 1. Domain Entities & Database
- `AiWorkflow`: The core entity specifying triggers, active status, and priority.
- `WorkflowStep`: Configurable sequence items that can be `Action`, `Condition`, `AIDecision`, or `Approval`.
- `WorkflowExecution` & `WorkflowExecutionStep`: Tracks every event triggering the system and safely records the state, errors, and output.
- `WorkflowTemplate`: Seeded with pre-configured automations.
- I mapped these using `EF Core` to tables prefixed with `AiEngine_` to prevent namespace collisions and preserve the pristine state of Day 13/22 models.

### 2. Execution Engine Core
- `WorkflowTriggerService`: Accepts events like `InvoiceOverdue` and safely finds active workflows that match.
- `WorkflowConditionEvaluator`: A secure JSON parser that processes comparison rules (`> 10000`) and logic combinations (`AND`, `OR`) without exposing the backend to arbitrary code execution risks.
- `AIDecisionService`: Injects `IAIService` to ask the AI if a specific business logic flow should proceed, parsing structured JSON responses containing decisions and confidence scores.
- `WorkflowApprovalService`: Safely halts a workflow execution if a human approval is required, updating its state to `WaitingForApproval`.
- `WorkflowActionExecutor`: Interprets configured actions to interface with `ActionCenter`, `Emails`, etc.

### 3. CQRS API Endpoints
Endpoints have been secured with FluentValidation and CQRS Commands/Queries under `api/automation`:
- `GET /api/automation/workflows`
- `POST /api/automation/workflows`
- `POST /api/automation/workflows/{id}/activate`
- `POST /api/automation/workflows/{id}/test`
- `GET /api/automation/executions`
- `POST /api/automation/actions/{id}/approve`
- `POST /api/automation/actions/{id}/reject`
- `GET /api/automation/templates`

### 4. Sample Templates
I seeded the database with 4 production-ready templates:
1. **Recover Overdue Payments** (`InvoiceOverdue >= 7 days`)
2. **Recover Inactive Customers** (`CustomerInactive >= 30 days` + `AIDecision`)
3. **Prevent Stockout** (`InventoryLow`)
4. **New Lead Follow-up** (`NewLeadCreated`)

## How to test it locally

1. **Run the backend**:
   ```bash
   dotnet run --project backend.csproj
   ```
2. During startup, the seed data step will insert the `AiEngine_WorkflowTemplates`.
3. Open the Swagger UI or postman to `GET /api/automation/templates` to verify the templates.
4. Hit `POST /api/automation/workflows` to create a workflow from the template, and then `POST /api/automation/workflows/{id}/activate` to turn it on.
5. Hit `POST /api/automation/workflows/{id}/test` with JSON data (e.g. `{"Amount": 10001}`) to watch the engine parse conditions and record executions securely in the DB!

> [!TIP]
> The engine handles `integration_required` scenarios safely without crashing, gracefully recording errors in the `WorkflowExecutionStep` outputs when external services (like Whatsapp or Email APIs) are unavailable.

## Frontend Implementation

The frontend provides a beautifully simple, non-technical experience for business owners to set up AI workflows.

### 1. Automation Dashboard (`/automation`)
Displays a comprehensive view of:
- **Active Automations** and **Time Saved** (estimated impact).
- **Revenue Opportunities** derived from workflows like "Recover Overdue Payments."
- **Recent Actions** showing exactly what BusinessOS AI is doing in real-time.
- **Recommended Templates** (1-click installation for everyday business problems).

### 2. AI Workflow Assistant (`/automation/create`)
Built a revolutionary wizard to let users type plain English commands:
- Users describe a workflow: *"When a customer is inactive for 30 days, send them a win-back email."*
- The **AI Assistant** parses the intent and builds the visual representation securely on the fly.

### 3. Visual Workflow Preview
A clean, visual node-style component that explains to non-technical users exactly what will happen:
- **Trigger** (e.g., Invoice Overdue) → **Condition** (Amount > 1000) → **AI Decision** → **Action** (Send Reminder).

### 4. Safety & Approvals
- The UI warns users when an automation interacts with customers and recommends the **"Ask me for approval"** mode.
- Approval steps are integrated directly with the Day 27 Action Center.

## How to test the complete Day 28 workflow

1. Start both the backend (`dotnet run`) and frontend (`npm run dev`).
2. Navigate to `http://localhost:3000/automation`.
3. Browse the **Recommended Templates** and install one, or click **Create Automation**.
4. In the Wizard, try the **Use AI Assistant** button and type a prompt to see the flow magically generated.
5. Save the workflow, return to the Dashboard, and click the automation to view its details, including the Execution History and visual preview.
