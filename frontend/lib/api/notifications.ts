import api from '../api-client';

export interface AppNotification {
  id: string;
  organizationId: string;
  userId: string;
  title: string;
  message: string;
  type: string;
  status: string;
  createdAt: string;
  readAt?: string;
}

export const getNotifications = async (limit = 50): Promise<AppNotification[]> => {
  const response = await api.get(`/api/notifications?limit=${limit}`);
  return response.data;
};

export const getUnreadNotifications = async (): Promise<AppNotification[]> => {
  const response = await api.get('/api/notifications/unread');
  return response.data;
};

export const markAllNotificationsRead = async (): Promise<void> => {
  await api.post('/api/notifications/mark-all-read');
};
