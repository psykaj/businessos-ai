# Workflow Templates UI

The Workflow Templates UI provides business owners with out-of-the-box value. Instead of building from scratch, they can browse curated templates.

## Functionality
- **Display**: Grid layout of cards categorizing templates (e.g., Marketing, Customer Success, Inventory).
- **Clone Action**: Clicking "Use Template" clones the `WorkflowTemplate` into a draft `AutomationWorkflow` specific to the user's tenant organization.
- **Backend API Integration**: Connects via `useAutomationTemplates()` hook.
