"use client";

import React, { useState } from "react";
import { AlertDto, alertsService } from "@/lib/alerts-service";
import { 
  Sheet, 
  SheetContent, 
  SheetDescription, 
  SheetHeader, 
  SheetTitle,
  SheetFooter
} from "@/components/ui/sheet";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Separator } from "@/components/ui/separator";
import { 
  AlertCircle, 
  AlertTriangle, 
  Info, 
  Clock, 
  Activity, 
  Zap, 
  Link as LinkIcon, 
  CheckCircle2,
  Workflow
} from "lucide-react";
import { toast } from "sonner";
import { useRouter } from "next/navigation";

interface AlertDetailsDrawerProps {
  alert: AlertDto | null;
  isOpen: boolean;
  onClose: () => void;
  onActionComplete: () => void;
}

export function AlertDetailsDrawer({ alert, isOpen, onClose, onActionComplete }: AlertDetailsDrawerProps) {
  const router = useRouter();
  const [isTakingAction, setIsTakingAction] = useState(false);
  const [isResolving, setIsResolving] = useState(false);

  if (!alert) return null;

  const handleTakeAction = async () => {
    setIsTakingAction(true);
    try {
      const res = await alertsService.takeAction(alert.id);
      if (res.success) {
        toast.success("Action Executed", { description: res.message });
        onActionComplete();
      }
    } catch (error) {
      toast.error("Action Failed", { description: "Failed to execute recommended action." });
    } finally {
      setIsTakingAction(false);
    }
  };

  const handleResolve = async () => {
    setIsResolving(true);
    try {
      await alertsService.resolveAlert(alert.id);
      toast.success("Alert Resolved");
      onActionComplete();
    } catch (error) {
      toast.error("Failed to resolve alert.");
    } finally {
      setIsResolving(false);
    }
  };

  const handleDismiss = async () => {
    try {
      await alertsService.dismissAlert(alert.id);
      toast.info("Alert Dismissed");
      onActionComplete();
    } catch (error) {
      toast.error("Failed to dismiss alert.");
    }
  };

  const handleAutomate = () => {
    toast.success("Opening Automation Studio", { description: "Pre-filling trigger for " + alert.title });
    onClose();
    router.push("/dashboard/automation?template=" + encodeURIComponent(alert.category));
  };

  const getSeverityIcon = (severity: string) => {
    switch (severity) {
      case "Critical": return <AlertCircle className="h-6 w-6 text-red-500" />;
      case "High": return <AlertTriangle className="h-6 w-6 text-orange-500" />;
      case "Medium": return <Info className="h-6 w-6 text-yellow-500" />;
      case "Low": default: return <Info className="h-6 w-6 text-blue-500" />;
    }
  };

  return (
    <Sheet open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <SheetContent className="w-full sm:max-w-md overflow-y-auto">
        <SheetHeader className="text-left">
          <div className="flex items-center gap-3 mb-2">
            <div className="p-2 rounded-full bg-slate-100 dark:bg-slate-800">
              {getSeverityIcon(alert.severity)}
            </div>
            <Badge variant="outline" className="font-semibold text-xs tracking-wider uppercase">{alert.category}</Badge>
            {alert.status === "Resolved" && (
              <Badge variant="default" className="bg-green-500 hover:bg-green-600">Resolved</Badge>
            )}
          </div>
          <SheetTitle className="text-xl font-bold leading-tight">{alert.title}</SheetTitle>
          <SheetDescription className="text-sm">
            Generated {new Date(alert.timestamp).toLocaleString()}
          </SheetDescription>
        </SheetHeader>

        <div className="mt-6 space-y-6">
          {/* Section: What happened? */}
          <section className="space-y-2">
            <h4 className="text-sm font-semibold flex items-center gap-2 text-foreground">
              <Activity className="h-4 w-4 text-primary" /> What happened?
            </h4>
            <div className="bg-slate-50 dark:bg-slate-900/50 rounded-lg p-3 text-sm text-slate-700 dark:text-slate-300 border">
              {alert.description}
            </div>
          </section>

          {/* Section: AI Explanation / Why am I seeing this? */}
          {alert.aiExplanation && (
            <section className="space-y-2">
              <h4 className="text-sm font-semibold flex items-center gap-2 text-foreground">
                <Zap className="h-4 w-4 text-purple-500" /> Why am I seeing this?
              </h4>
              <div className="bg-purple-50 dark:bg-purple-900/10 rounded-lg p-3 text-sm text-purple-900 dark:text-purple-300 border border-purple-100 dark:border-purple-900/50">
                {alert.aiExplanation}
              </div>
            </section>
          )}

          {/* Business Impact Grid */}
          <section className="grid grid-cols-2 gap-4">
            <div className="space-y-1">
              <span className="text-xs font-medium text-muted-foreground uppercase tracking-wider">Severity</span>
              <p className="font-semibold text-sm">{alert.severity}</p>
            </div>
            <div className="space-y-1">
              <span className="text-xs font-medium text-muted-foreground uppercase tracking-wider">Business Impact</span>
              <p className="font-semibold text-sm text-emerald-600 dark:text-emerald-400">
                {alert.businessImpact || "Impact estimate unavailable"}
              </p>
            </div>
            <div className="space-y-1">
              <span className="text-xs font-medium text-muted-foreground uppercase tracking-wider">Source Module</span>
              <p className="font-semibold text-sm">{alert.sourceModule}</p>
            </div>
            {alert.relatedRecordId && (
              <div className="space-y-1">
                <span className="text-xs font-medium text-muted-foreground uppercase tracking-wider">Record Ref</span>
                <p className="font-mono text-sm text-primary flex items-center gap-1 cursor-pointer hover:underline">
                  <LinkIcon className="h-3 w-3" /> {alert.relatedRecordId}
                </p>
              </div>
            )}
          </section>

          <Separator />

          {/* Action Recommendations */}
          <section className="space-y-3">
            <h4 className="text-sm font-semibold text-foreground">Recommended Actions</h4>
            
            {alert.actionRequired && alert.status !== "Resolved" ? (
              <div className="p-4 border border-primary/20 bg-primary/5 rounded-xl space-y-3">
                <p className="text-sm font-medium">{alert.recommendedAction}</p>
                <Button 
                  onClick={handleTakeAction} 
                  disabled={isTakingAction}
                  className="w-full shadow-sm"
                >
                  {isTakingAction ? "Executing..." : "Execute Action"}
                </Button>
              </div>
            ) : (
              <div className="text-sm text-muted-foreground">
                No immediate action required or alert is already resolved.
              </div>
            )}
            
            {/* Automation up-sell for repetitive alerts */}
            {(alert.category === "Inventory" || alert.category === "Finance") && alert.status !== "Resolved" && (
              <div className="flex items-start gap-3 p-3 bg-slate-50 dark:bg-slate-900 rounded-lg border">
                <Workflow className="h-5 w-5 text-indigo-500 mt-0.5 shrink-0" />
                <div className="space-y-2">
                  <div>
                    <h5 className="text-sm font-semibold">Automate this process</h5>
                    <p className="text-xs text-muted-foreground mt-0.5">This issue happens frequently. You can create an automation rule to handle this instantly.</p>
                  </div>
                  <Button variant="outline" size="sm" onClick={handleAutomate} className="h-8 text-xs font-semibold">
                    Create Automation
                  </Button>
                </div>
              </div>
            )}
          </section>
        </div>

        <SheetFooter className="mt-8 flex-col sm:flex-row gap-2 sm:space-x-0 pt-4 border-t">
          {alert.status !== "Resolved" && (
            <Button variant="outline" onClick={handleResolve} disabled={isResolving} className="sm:w-full">
              <CheckCircle2 className="mr-2 h-4 w-4" />
              Mark as Resolved
            </Button>
          )}
          <Button variant="ghost" onClick={handleDismiss} className="sm:w-full text-muted-foreground">
            Dismiss Alert
          </Button>
        </SheetFooter>
      </SheetContent>
    </Sheet>
  );
}
