// Fallback mock data in case backend is not ready
const mockWorkflows = [
  {
    id: "wf-1",
    name: "Welcome New Customer",
    description: "Send a welcome email and assign a CS rep when a new customer is created.",
    status: "Active",
    createdAt: new Date().toISOString(),
    nodes: [],
    edges: []
  },
  {
    id: "wf-2",
    name: "Invoice Reminder",
    description: "Send follow-up emails 3 days before invoice is due.",
    status: "Draft",
    createdAt: new Date().toISOString(),
    nodes: [],
    edges: []
  }
];

const mockTemplates = [
  {
    id: "tpl-1",
    name: "Welcome New Customer",
    description: "Send a welcome email and assign a CS rep when a new customer is created.",
    category: "Customer Success",
  },
  {
    id: "tpl-2",
    name: "Low Stock Alert",
    description: "Notify purchasing team when inventory drops below threshold.",
    category: "Inventory",
  },
  {
    id: "tpl-3",
    name: "VIP Customer Reward",
    description: "Award loyalty points on large purchases.",
    category: "Marketing",
  }
];

const mockExecutions = [
  {
    id: "exec-1",
    workflowId: "wf-1",
    workflowName: "Welcome New Customer",
    status: "Completed",
    startedAt: new Date(Date.now() - 1000 * 60 * 5).toISOString(),
    completedAt: new Date(Date.now() - 1000 * 60 * 4).toISOString(),
    triggerSource: "CustomerCreated"
  },
  {
    id: "exec-2",
    workflowId: "wf-2",
    workflowName: "Invoice Reminder",
    status: "Failed",
    startedAt: new Date(Date.now() - 1000 * 60 * 60).toISOString(),
    completedAt: new Date(Date.now() - 1000 * 60 * 59).toISOString(),
    triggerSource: "Schedule",
    errorDetails: "SMTP Connection Timeout"
  }
];

// In a real app, this would use axios to hit the backend endpoints.
// We mock it for the visual builder to ensure smooth UI development.
export const automationStudioApi = {
  getWorkflows: async () => {
    return Promise.resolve(mockWorkflows);
  },
  getWorkflowById: async (id: string) => {
    return Promise.resolve(mockWorkflows.find(w => w.id === id) || mockWorkflows[0]);
  },
  saveWorkflow: async (id: string, payload: Record<string, unknown>) => {
    return Promise.resolve({ success: true, id, ...payload });
  },
  getTemplates: async () => {
    return Promise.resolve(mockTemplates);
  },
  cloneTemplate: async (id: string) => {
    const tpl = mockTemplates.find(t => t.id === id);
    return Promise.resolve({ ...tpl, id: `wf-${Math.random()}`, status: "Draft", createdAt: new Date().toISOString() });
  },
  getExecutions: async () => {
    return Promise.resolve(mockExecutions);
  }
};
