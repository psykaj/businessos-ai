import api from '../api-client';

export interface WhatsAppSettings {
  id: string;
  phoneNumberId: string;
  businessAccountId: string;
  accessToken: string;
  webhookVerifyToken: string;
}

export interface WhatsAppTemplate {
  id: string;
  name: string;
  category: string;
  language: string;
  status: string;
}

export const getWhatsAppSettings = async (): Promise<WhatsAppSettings> => {
  const response = await api.get('/whatsapp/settings');
  return response.data;
};

export const saveWhatsAppSettings = async (settings: Partial<WhatsAppSettings>): Promise<WhatsAppSettings> => {
  const response = await api.post('/whatsapp/settings', settings);
  return response.data;
};

export const getWhatsAppTemplates = async (): Promise<WhatsAppTemplate[]> => {
  const response = await api.get('/whatsapp/templates');
  return response.data;
};
