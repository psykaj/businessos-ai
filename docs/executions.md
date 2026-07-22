# Workflow Execution Engine & Audit Logging

The Execution Engine processes workflows asynchronously with step-by-step logging, conditional branching, and retry protection.

## Execution Flow

1. **Trigger Reception**: `WorkflowTriggerDispatcher` matches trigger type and organization.
2. **Global Condition Check**: Evaluates workflow-level conditions.
3. **Action Loop**:
   - Evaluates action-level conditions.
   - Executes `IActionHandler`.
   - Performs up to 3 retries on failure with backoff.
   - Logs step input, output, duration, and status to `WorkflowExecutionLog`.
   - Merges step output context into execution context.
4. **Completion**: Updates `WorkflowExecution` record with status, duration, and context snapshot.

## APIs

- `GET /api/workflows/executions`: Get paged executions for organization.
- `GET /api/workflows/executions/{id}`: Get execution details.
- `GET /api/workflows/executions/workflow/{workflowId}`: Get execution history for a specific workflow.
- `GET /api/workflows/logs/execution/{executionId}`: Get step-by-step logs for an execution.
