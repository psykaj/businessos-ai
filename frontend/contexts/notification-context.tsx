"use client";

import React, { createContext, useContext, useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { useAuth } from '@/contexts/auth-context';
import { toast } from 'sonner';
import { AppNotification, getUnreadNotifications } from '@/lib/api/notifications';

interface NotificationContextType {
  notifications: AppNotification[];
  unreadCount: number;
  refreshNotifications: () => Promise<void>;
}

const NotificationContext = createContext<NotificationContextType | undefined>(undefined);

export function NotificationProvider({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();
  const [notifications, setNotifications] = useState<AppNotification[]>([]);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  const fetchNotifications = async () => {
    try {
      if (!user) return;
      const unread = await getUnreadNotifications();
      setNotifications(unread);
    } catch (error) {
      console.error('Failed to fetch notifications:', error);
    }
  };

  useEffect(() => {
    const token = typeof window !== 'undefined' ? localStorage.getItem('businessos_token') : null;
    if (user && token) {
      // Disabled SignalR as it's causing connection errors and is not mandatory for basic functionality
      // const hubUrl = `${process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5294'}/hubs/notifications`;
      // const newConnection = new signalR.HubConnectionBuilder()
      //   .withUrl(hubUrl, {
      //     accessTokenFactory: () => token,
      //   })
      //   .withAutomaticReconnect()
      //   .build();

      // eslint-disable-next-line react-hooks/set-state-in-effect
      // setConnection(newConnection);
    } else {
      if (connection) {
        connection.stop();
        setConnection(null);
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [user]);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          console.log('SignalR Connected!');

          connection.on('ReceiveNotification', (notification: AppNotification) => {
            setNotifications(prev => [notification, ...prev]);
            toast(notification.title, {
              description: notification.message,
            });
          });

          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          connection.on('ReceiveBroadcast', (broadcast: any) => {
            toast(broadcast.title, {
              description: broadcast.message,
            });
            fetchNotifications(); // Refresh to get the actual notification entity if stored
          });
        })
        .catch(e => console.error('SignalR Connection Error: ', e));
    }
  }, [connection]);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    fetchNotifications();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [user]);

  const value = {
    notifications,
    unreadCount: notifications.length,
    refreshNotifications: fetchNotifications,
  };

  return (
    <NotificationContext.Provider value={value}>
      {children}
    </NotificationContext.Provider>
  );
}

export const useNotifications = () => {
  const context = useContext(NotificationContext);
  if (context === undefined) {
    throw new Error('useNotifications must be used within a NotificationProvider');
  }
  return context;
};
