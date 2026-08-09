"use client";

import React, { useState } from "react";
import { AlertDto } from "@/lib/alerts-service";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { CheckCircle2, AlertTriangle, AlertCircle, Info, Clock, ArrowRight } from "lucide-react";
import { AlertDetailsDrawer } from "./alert-details-drawer";

interface AlertListProps {
  alerts: AlertDto[];
  onActionComplete: () => void;
}

export function AlertList({ alerts, onActionComplete }: AlertListProps) {
  const [selectedAlert, setSelectedAlert] = useState<AlertDto | null>(null);

  const getSeverityIcon = (severity: string) => {
    switch (severity) {
      case "Critical":
        return <AlertCircle className="h-5 w-5 text-red-500" />;
      case "High":
        return <AlertTriangle className="h-5 w-5 text-orange-500" />;
      case "Medium":
        return <Info className="h-5 w-5 text-yellow-500" />;
      case "Low":
      default:
        return <Info className="h-5 w-5 text-blue-500" />;
    }
  };

  const getSeverityBadgeClass = (severity: string) => {
    switch (severity) {
      case "Critical":
        return "bg-red-100 text-red-800 dark:bg-red-900/30 dark:text-red-300";
      case "High":
        return "bg-orange-100 text-orange-800 dark:bg-orange-900/30 dark:text-orange-300";
      case "Medium":
        return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/30 dark:text-yellow-300";
      case "Low":
      default:
        return "bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300";
    }
  };

  if (alerts.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center p-12 text-center border rounded-xl border-dashed">
        <div className="h-12 w-12 rounded-full bg-green-100 dark:bg-green-900/20 flex items-center justify-center mb-4">
          <CheckCircle2 className="h-6 w-6 text-green-600 dark:text-green-400" />
        </div>
        <h3 className="text-lg font-medium">Everything looks good.</h3>
        <p className="text-sm text-muted-foreground mt-2 max-w-sm">
          No critical issues need your attention right now. You're all caught up!
        </p>
      </div>
    );
  }

  return (
    <>
      <div className="space-y-4">
        {alerts.map((alert) => (
          <Card 
            key={alert.id} 
            className={`cursor-pointer hover:border-primary/50 transition-colors overflow-hidden ${alert.status === 'Unread' ? 'bg-slate-50 dark:bg-slate-900/50 border-l-4 border-l-primary' : ''}`}
            onClick={() => setSelectedAlert(alert)}
          >
            <div className="p-4 sm:p-5 flex flex-col sm:flex-row gap-4 sm:items-start justify-between">
              <div className="flex gap-4 items-start flex-1">
                <div className="mt-1 flex-shrink-0">
                  {getSeverityIcon(alert.severity)}
                </div>
                <div className="space-y-1.5 flex-1">
                  <div className="flex flex-wrap items-center gap-2">
                    <h4 className={`font-semibold text-base ${alert.status === 'Unread' ? 'text-foreground' : 'text-foreground/90'}`}>
                      {alert.title}
                    </h4>
                    {alert.status === "Unread" && (
                      <Badge variant="default" className="text-[10px] px-1.5 h-4">New</Badge>
                    )}
                  </div>
                  <p className="text-sm text-muted-foreground line-clamp-2 pr-4">{alert.description}</p>
                  
                  <div className="flex flex-wrap items-center gap-3 pt-2 text-xs font-medium">
                    <Badge variant="secondary" className={`font-semibold ${getSeverityBadgeClass(alert.severity)}`}>
                      {alert.severity}
                    </Badge>
                    <span className="text-muted-foreground flex items-center gap-1">
                      <Clock className="h-3.5 w-3.5" />
                      {new Date(alert.timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                    </span>
                    <span className="text-muted-foreground border-l pl-3 hidden sm:inline-block">
                      {alert.category}
                    </span>
                    {alert.businessImpact && (
                      <span className="text-emerald-600 dark:text-emerald-400 font-semibold border-l pl-3 hidden sm:inline-block">
                        Impact: {alert.businessImpact}
                      </span>
                    )}
                  </div>
                </div>
              </div>
              
              <div className="flex sm:flex-col items-center sm:items-end justify-between sm:justify-center flex-shrink-0 mt-2 sm:mt-0 border-t sm:border-t-0 pt-3 sm:pt-0">
                {alert.actionRequired && alert.status !== "Resolved" ? (
                  <Button size="sm" className="w-full sm:w-auto" onClick={(e) => { e.stopPropagation(); setSelectedAlert(alert); }}>
                    {alert.recommendedAction || "Review"}
                  </Button>
                ) : (
                  <Button variant="ghost" size="sm" className="w-full sm:w-auto text-muted-foreground">
                    View <ArrowRight className="ml-1 h-4 w-4" />
                  </Button>
                )}
              </div>
            </div>
          </Card>
        ))}
      </div>

      <AlertDetailsDrawer 
        alert={selectedAlert} 
        isOpen={!!selectedAlert} 
        onClose={() => setSelectedAlert(null)}
        onActionComplete={() => {
          setSelectedAlert(null);
          onActionComplete();
        }}
      />
    </>
  );
}
