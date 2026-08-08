export type WorkflowStatus = 'Active' | 'Paused' | 'Draft' | 'Archived';
export type WorkflowStepType = 'Condition' | 'Action' | 'AIDecision' | 'Approval';

export interface WorkflowStep {
  id: string;
  workflowId: string;
  stepOrder: number;
  stepType: WorkflowStepType;
  name: string;
  description?: string;
  configuration?: string;
  condition?: string;
  isRequired: boolean;
  status: string;
  createdAt: string;
  updatedAt: string;
}

export interface Workflow {
  id: string;
  organizationId: string;
  name: string;
  description?: string;
  category?: string;
  triggerType: string;
  isActive: boolean;
  priority: number;
  requiresApproval: boolean;
  status: WorkflowStatus;
  steps: WorkflowStep[];
  createdAt: string;
  updatedAt: string;
}

export interface WorkflowTemplate {
  id: string;
  name: string;
  description: string;
  category: string;
  triggerType: string;
  configuration: string;
  priority: number;
  createdAt: string;
}

export interface WorkflowExecution {
  id: string;
  workflowId: string;
  organizationId: string;
  triggerType: string;
  status: string; // 'Running' | 'Completed' | 'Failed' | 'WaitingForApproval' | 'Cancelled'
  errorMessage?: string;
  startedAt: string;
  completedAt?: string;
  executionContext?: string;
  workflow?: Workflow;
  executionSteps?: WorkflowExecutionStep[];
}

export interface WorkflowExecutionStep {
  id: string;
  executionId: string;
  stepId: string;
  status: string;
  errorMessage?: string;
  outputData?: string;
  startedAt: string;
  completedAt?: string;
}
