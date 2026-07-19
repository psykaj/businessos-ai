import api from '../api-client';

export interface AIConversation {
  id: string;
  organizationId: string;
  userId: string;
  title: string;
  provider: string;
  model: string;
  status: string;
  updatedAt: string;
}

export interface AIMessage {
  id: string;
  conversationId: string;
  role: 'user' | 'assistant';
  content: string;
  tokenUsage: number;
  createdAt: string;
}

export const getConversations = async (): Promise<AIConversation[]> => {
  const response = await api.get('/api/ai/conversations');
  return response.data;
};

export const createConversation = async (data: { title: string; provider?: string; model?: string }): Promise<AIConversation> => {
  const response = await api.post('/api/ai/conversations', data);
  return response.data;
};

export const getConversation = async (id: string): Promise<AIConversation & { messages: AIMessage[] }> => {
  const response = await api.get(`/api/ai/conversations/${id}`);
  return response.data;
};

export const sendMessage = async (id: string, message: string): Promise<AIConversation> => {
  const response = await api.post(`/api/ai/conversations/${id}/messages`, { message });
  return response.data;
};
