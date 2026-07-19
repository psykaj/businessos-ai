"use client";

import { useEffect, useState } from "react";
import { formatDistanceToNow } from "date-fns";
import { Bell, Check, CheckCircle2, Circle, Clock, Info, ShieldAlert, Trash2 } from "lucide-react";
import { useNotifications } from "@/contexts/notification-context";
import { AppNotification, getNotifications, markAllNotificationsRead } from "@/lib/api/notifications";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

export default function NotificationsPage() {
  const { refreshNotifications } = useNotifications();
  const [items, setItems] = useState<AppNotification[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchHistory = async () => {
    try {
      setLoading(true);
      const data = await getNotifications(100);
      setItems(data);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchHistory();
  }, []);

  const handleMarkAllRead = async () => {
    try {
      await markAllNotificationsRead();
      await fetchHistory();
      await refreshNotifications();
    } catch (error) {
      console.error("Failed to mark read", error);
    }
  };

  const getTypeIcon = (type: string) => {
    switch (type.toLowerCase()) {
      case "success": return <CheckCircle2 className="h-5 w-5 text-emerald-500" />;
      case "error": return <ShieldAlert className="h-5 w-5 text-destructive" />;
      case "warning": return <Info className="h-5 w-5 text-amber-500" />;
      default: return <Info className="h-5 w-5 text-primary" />;
    }
  };

  return (
    <div className="space-y-6 max-w-4xl mx-auto py-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Notification Center</h1>
          <p className="text-muted-foreground">Stay updated with your organization's activities.</p>
        </div>
        <Button onClick={handleMarkAllRead} variant="outline" className="gap-2">
          <Check className="h-4 w-4" />
          Mark all as read
        </Button>
      </div>

      <Card>
        <CardHeader className="border-b">
          <CardTitle className="text-lg flex items-center gap-2">
            <Bell className="h-5 w-5" />
            Recent Notifications
          </CardTitle>
          <CardDescription>
            Your latest alerts and messages
          </CardDescription>
        </CardHeader>
        <CardContent className="p-0">
          {loading ? (
            <div className="p-8 text-center text-muted-foreground animate-pulse">Loading notifications...</div>
          ) : items.length === 0 ? (
            <div className="p-12 text-center text-muted-foreground flex flex-col items-center gap-3">
              <Bell className="h-10 w-10 text-muted-foreground/30" />
              <p>You're all caught up!</p>
            </div>
          ) : (
            <div className="divide-y divide-border">
              {items.map((notification) => (
                <div 
                  key={notification.id} 
                  className={`flex items-start gap-4 p-4 md:p-6 transition-colors hover:bg-muted/30 ${notification.status === 'Unread' ? 'bg-primary/5' : ''}`}
                >
                  <div className="shrink-0 mt-1">
                    {getTypeIcon(notification.type)}
                  </div>
                  <div className="flex-1 min-w-0">
                    <div className="flex items-center justify-between gap-2 mb-1">
                      <h4 className="font-semibold text-sm truncate pr-4 text-foreground">
                        {notification.title}
                      </h4>
                      <div className="flex items-center gap-2 shrink-0">
                        {notification.status === 'Unread' && (
                          <Badge variant="default" className="text-[10px] h-5 px-1.5">New</Badge>
                        )}
                        <span className="text-xs text-muted-foreground flex items-center gap-1">
                          <Clock className="h-3 w-3" />
                          {formatDistanceToNow(new Date(notification.createdAt), { addSuffix: true })}
                        </span>
                      </div>
                    </div>
                    <p className="text-sm text-muted-foreground break-words">
                      {notification.message}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
