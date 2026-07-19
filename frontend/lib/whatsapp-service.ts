import apiClient from "./api-client";

export interface WhatsAppSettings {
  id?: string;
  phoneNumberId: string;
  businessAccountId: string;
  accessToken: string;
  webhookVerifyToken: string;
}

export interface WhatsAppTemplate {
  id: string;
  name: string;
  language: string;
  category: string;
  components: string;
  status: string;
}

export interface Campaign {
  id?: string;
  name: string;
  type: string;
  status: string;
  templateId?: string;
  scheduledAt?: string;
  totalMessages: number;
  sentMessages: number;
  deliveredMessages: number;
  readMessages: number;
  failedMessages: number;
  createdAt?: string;
}

export const WhatsAppService = {
  getSettings: async (): Promise<WhatsAppSettings> => {
    const response = await apiClient.get<WhatsAppSettings>("/api/whatsapp/settings");
    return response.data;
  },

  saveSettings: async (data: WhatsAppSettings): Promise<WhatsAppSettings> => {
    const response = await apiClient.post<WhatsAppSettings>("/api/whatsapp/settings", data);
    return response.data;
  },

  syncTemplates: async (): Promise<WhatsAppTemplate[]> => {
    const response = await apiClient.post<WhatsAppTemplate[]>("/api/whatsapp/sync-templates");
    return response.data;
  },
  
  getTemplates: async (): Promise<WhatsAppTemplate[]> => {
    // For now, sync acts as get. In a real app we'd have a separate get endpoint
    return await WhatsAppService.syncTemplates();
  },

  getCampaigns: async (): Promise<Campaign[]> => {
    const response = await apiClient.get<Campaign[]>("/api/campaign");
    return response.data;
  },

  createCampaign: async (data: Partial<Campaign>): Promise<Campaign> => {
    const response = await apiClient.post<Campaign>("/api/campaign", data);
    return response.data;
  }
};
