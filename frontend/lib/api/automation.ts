import api from '../api-client';

export interface AutomationRule {
  id: string;
  organizationId: string;
  name: string;
  trigger: string;
  conditions: string; // JSON string
  actions: string; // JSON string
  isEnabled: boolean;
  createdAt: string;
}

export interface AutomationLog {
  id: string;
  organizationId: string;
  ruleId: string;
  status: string;
  triggeredAt: string;
  errorMessage?: string;
}

export const getAutomationRules = async (): Promise<AutomationRule[]> => {
  const response = await api.get('/automation/rules');
  return response.data;
};

export const createAutomationRule = async (rule: Partial<AutomationRule>): Promise<AutomationRule> => {
  const response = await api.post('/automation/rules', rule);
  return response.data;
};

export const getAutomationLogs = async (ruleId: string, limit = 100): Promise<AutomationLog[]> => {
  const response = await api.get(`/automation/rules/${ruleId}/logs?limit=${limit}`);
  return response.data;
};
