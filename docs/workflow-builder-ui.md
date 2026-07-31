# Visual Workflow Builder

The Visual Workflow Builder (`WorkflowBuilder.tsx`) uses `@xyflow/react` (React Flow) to allow non-technical users to build logic visually.

## Components
1. **Sidebar (`Sidebar.tsx`)**: Contains draggable triggers, conditions, and actions grouped by domain (CRM, Finance, etc.).
2. **React Flow Canvas**: The main area where users drop and connect nodes.
3. **Nodes**:
   - `TriggerNode.tsx`: Represents the starting event (amber styling).
   - `ActionNode.tsx`: Represents a task execution (blue styling).
   - `ConditionNode.tsx`: Represents an If/Else fork (indigo styling, true/false source handles).
4. **Config Panel (`NodeConfigPanel.tsx`)**: A slide-out panel that appears when a node is clicked, allowing inline configuration (e.g., editing JSON payloads or logical expressions).

## Architecture
The node data (including UI position) is synchronized back to the backend via `useSaveWorkflow` hooks.
