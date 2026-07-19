import api from '../api-client';

export interface EmailTemplate {
  id: string;
  name: string;
  subject: string;
  htmlContent: string;
  variables: string; // JSON string
  isDefault: boolean;
  createdAt: string;
}

export const getEmailTemplates = async (): Promise<EmailTemplate[]> => {
  const response = await api.get('/api/email/templates');
  return response.data;
};

export const createEmailTemplate = async (template: Partial<EmailTemplate>): Promise<EmailTemplate> => {
  const response = await api.post('/api/email/templates', template);
  return response.data;
};
