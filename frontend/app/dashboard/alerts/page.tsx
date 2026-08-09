"use client";

import React, { useState, useEffect } from "react";
import { AlertDto, alertsService } from "@/lib/alerts-service";
import { DashboardShell } from "@/components/layout/dashboard-shell";
import { ProtectedRoute } from "@/components/auth/protected-route";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { AlertList } from "@/components/alerts/alert-list";
import { Bell, RefreshCw } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function AlertsPage() {
  const [alerts, setAlerts] = useState<AlertDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [activeTab, setActiveTab] = useState("all");

  const fetchAlerts = async () => {
    setIsLoading(true);
    try {
      let data: AlertDto[] = [];
      if (activeTab === "all") data = await alertsService.getAlerts();
      else if (activeTab === "unread") data = await alertsService.getUnreadAlerts();
      else if (activeTab === "action-required") data = await alertsService.getActionRequiredAlerts();
      else if (activeTab === "high-priority") {
        const all = await alertsService.getAlerts();
        data = all.filter(a => a.severity === "Critical" || a.severity === "High");
      }
      else if (activeTab === "resolved") {
        const all = await alertsService.getAlerts();
        data = all.filter(a => a.status === "Resolved" || a.status === "Dismissed");
      }
      setAlerts(data);
    } catch (error) {
      console.error("Error fetching alerts", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchAlerts();
  }, [activeTab]);

  return (
    <ProtectedRoute>
      <DashboardShell>
        <div className="max-w-5xl mx-auto py-6 space-y-6">
          <header className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b pb-6 dark:border-slate-800">
            <div>
              <div className="flex items-center gap-2 mb-1">
                <div className="p-2 rounded-xl bg-primary/10 text-primary">
                  <Bell className="h-6 w-6" />
                </div>
                <h1 className="text-2xl font-bold tracking-tight">Proactive Alert Center</h1>
              </div>
              <p className="text-muted-foreground text-sm">
                AI-curated business alerts, system warnings, and required actions.
              </p>
            </div>
            <Button variant="outline" size="sm" onClick={fetchAlerts} disabled={isLoading}>
              <RefreshCw className={`mr-2 h-4 w-4 ${isLoading ? 'animate-spin' : ''}`} />
              Refresh
            </Button>
          </header>

          <Tabs defaultValue="all" value={activeTab} onValueChange={setActiveTab} className="w-full">
            <TabsList className="mb-6 grid w-full grid-cols-5 h-auto rounded-lg">
              <TabsTrigger value="all" className="py-2.5">All</TabsTrigger>
              <TabsTrigger value="unread" className="py-2.5">Unread</TabsTrigger>
              <TabsTrigger value="action-required" className="py-2.5">Action Required</TabsTrigger>
              <TabsTrigger value="high-priority" className="py-2.5">High Priority</TabsTrigger>
              <TabsTrigger value="resolved" className="py-2.5">Resolved</TabsTrigger>
            </TabsList>
            
            <div className="min-h-[400px]">
              {isLoading ? (
                <div className="space-y-4">
                  {[1, 2, 3].map(i => (
                    <div key={i} className="h-28 rounded-xl bg-slate-100 dark:bg-slate-800/50 animate-pulse" />
                  ))}
                </div>
              ) : (
                <>
                  <TabsContent value="all" className="m-0 focus-visible:outline-none">
                    <AlertList alerts={alerts} onActionComplete={fetchAlerts} />
                  </TabsContent>
                  <TabsContent value="unread" className="m-0 focus-visible:outline-none">
                    <AlertList alerts={alerts} onActionComplete={fetchAlerts} />
                  </TabsContent>
                  <TabsContent value="action-required" className="m-0 focus-visible:outline-none">
                    <AlertList alerts={alerts} onActionComplete={fetchAlerts} />
                  </TabsContent>
                  <TabsContent value="high-priority" className="m-0 focus-visible:outline-none">
                    <AlertList alerts={alerts} onActionComplete={fetchAlerts} />
                  </TabsContent>
                  <TabsContent value="resolved" className="m-0 focus-visible:outline-none">
                    <AlertList alerts={alerts} onActionComplete={fetchAlerts} />
                  </TabsContent>
                </>
              )}
            </div>
          </Tabs>
        </div>
      </DashboardShell>
    </ProtectedRoute>
  );
}
