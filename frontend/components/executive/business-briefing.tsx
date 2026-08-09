"use client";

import React, { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { businessIntelligenceService } from "@/lib/business-intelligence-service";
import { alertsService } from "@/lib/alerts-service";
import { Card } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  CheckCircle2, 
  TrendingUp, 
  Users, 
  CreditCard, 
  PackageSearch,
  AlertTriangle,
  Zap,
  Target,
  AlertCircle,
  Lightbulb,
  ArrowRight
} from "lucide-react";
import { toast } from "sonner";
import { useRouter } from "next/navigation";
import { AlertDetailsDrawer } from "@/components/alerts/alert-details-drawer";

export function BusinessBriefing() {
  const router = useRouter();
  const [selectedAlertId, setSelectedAlertId] = useState<string | null>(null);

  const { data: briefing, isLoading: isBriefingLoading } = useQuery({
    queryKey: ["ai-business-briefing-today"],
    queryFn: () => businessIntelligenceService.getBriefingToday(),
    staleTime: 5 * 60 * 1000,
  });

  const { data: alerts, isLoading: isAlertsLoading, refetch: refetchAlerts } = useQuery({
    queryKey: ["proactive-alerts-action-required"],
    queryFn: () => alertsService.getActionRequiredAlerts(),
    staleTime: 5 * 60 * 1000,
  });

  if (isBriefingLoading || isAlertsLoading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-12 w-3/4 max-w-md" />
        <div className="grid grid-cols-2 md:grid-cols-5 gap-4">
          {[...Array(5)].map((_, i) => <Skeleton key={i} className="h-24 w-full" />)}
        </div>
        <Skeleton className="h-24 w-full" />
        <div className="grid md:grid-cols-2 gap-6">
          <Skeleton className="h-64 w-full" />
          <Skeleton className="h-64 w-full" />
        </div>
      </div>
    );
  }

  if (!briefing) return null;

  const topRisks = briefing.executiveSummary.topRisks || [];
  const topOpportunities = briefing.executiveSummary.topOpportunities || [];
  const nextActions = briefing.executiveSummary.nextBestActions || [];
  const aiSummary = briefing.executiveSummary.aiSummary;

  const getHealthColor = (score: number) => {
    if (score >= 80) return "text-green-600 dark:text-green-400";
    if (score >= 60) return "text-yellow-600 dark:text-yellow-400";
    return "text-red-600 dark:text-red-400";
  };

  const selectedAlert = alerts?.find(a => a.id === selectedAlertId) || null;

  return (
    <div className="space-y-8 animate-in fade-in-50 duration-500">
      
      {/* 1. Primary Heading */}
      <div>
        <h1 className="text-3xl font-extrabold tracking-tight text-slate-900 dark:text-white">
          Good morning, here's what needs your attention.
        </h1>
        <p className="text-slate-500 mt-2">
          Your AI Business Briefing for today.
        </p>
      </div>

      {/* 3. Top Summary */}
      <div className="grid grid-cols-2 md:grid-cols-5 gap-3">
        <Card className="p-4 flex flex-col justify-center border-primary/20 bg-primary/5">
          <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground mb-1">
            <ActivityIcon className="h-4 w-4 text-primary" /> Health
          </div>
          <div className={`text-2xl font-black ${getHealthColor(briefing.health.score)}`}>
            {briefing.health.score}/100
          </div>
        </Card>
        
        <Card className="p-4 flex flex-col justify-center">
          <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground mb-1">
            <TrendingUp className="h-4 w-4 text-emerald-500" /> Revenue
          </div>
          <div className="text-2xl font-bold flex items-end gap-2">
            ${briefing.revenue.currentRevenue.toLocaleString()}
            {briefing.revenue.percentageChange > 0 && (
              <span className="text-xs font-semibold text-emerald-500 mb-1">↑ {briefing.revenue.percentageChange}%</span>
            )}
          </div>
        </Card>

        <Card className="p-4 flex flex-col justify-center">
          <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground mb-1">
            <Users className="h-4 w-4 text-blue-500" /> Customers
          </div>
          <div className="text-2xl font-bold">
            {briefing.customers.totalCustomers.toLocaleString()}
          </div>
        </Card>

        <Card className="p-4 flex flex-col justify-center">
          <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground mb-1">
            <CreditCard className="h-4 w-4 text-orange-500" /> Overdue
          </div>
          <div className="text-2xl font-bold">
            {briefing.overdueInvoiceCount} <span className="text-sm font-normal text-muted-foreground ml-1">inv</span>
          </div>
        </Card>

        <Card className="p-4 flex flex-col justify-center">
          <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground mb-1">
            <PackageSearch className="h-4 w-4 text-purple-500" /> Low Stock
          </div>
          <div className="text-2xl font-bold">
            {briefing.inventory.lowStockCount} <span className="text-sm font-normal text-muted-foreground ml-1">items</span>
          </div>
        </Card>
      </div>

      {/* 4. AI Summary */}
      {aiSummary && (
        <Card className="p-5 border-indigo-100 bg-indigo-50/50 dark:border-indigo-900/50 dark:bg-indigo-900/10">
          <div className="flex gap-4 items-start">
            <div className="p-2 rounded-full bg-indigo-100 dark:bg-indigo-900 text-indigo-600 dark:text-indigo-400 mt-0.5 shrink-0">
              <Zap className="h-5 w-5" />
            </div>
            <div>
              <h3 className="text-sm font-bold text-indigo-900 dark:text-indigo-300 mb-1">AI Summary</h3>
              <p className="text-indigo-800 dark:text-indigo-200 text-sm leading-relaxed">
                {aiSummary}
              </p>
              <div className="flex gap-4 mt-3 text-[10px] uppercase font-bold text-indigo-400">
                <span>{new Date(briefing.generatedAt).toLocaleString()}</span>
                <span>•</span>
                <span>Data Period: Last 7 Days</span>
                <span>•</span>
                <span>95% Confidence</span>
              </div>
            </div>
          </div>
        </Card>
      )}

      {/* 5. Priority Section (Needs Your Attention) */}
      <div className="space-y-4">
        <div className="flex items-center justify-between">
          <h2 className="text-xl font-bold flex items-center gap-2">
            <AlertTriangle className="h-5 w-5 text-orange-500" /> Needs Your Attention
          </h2>
          <Button variant="ghost" size="sm" onClick={() => router.push('/dashboard/alerts')}>
            View All <ArrowRight className="ml-1 h-4 w-4" />
          </Button>
        </div>
        
        {alerts && alerts.length > 0 ? (
          <div className="grid md:grid-cols-2 gap-4">
            {alerts.slice(0, 4).map(alert => (
              <Card key={alert.id} className="p-4 border-l-4 border-l-orange-500 hover:bg-slate-50 dark:hover:bg-slate-900/50 cursor-pointer transition-colors" onClick={() => setSelectedAlertId(alert.id)}>
                <div className="flex justify-between items-start gap-4 mb-2">
                  <h4 className="font-semibold text-sm leading-snug">{alert.title}</h4>
                  <Badge variant="outline" className={alert.severity === 'Critical' ? 'text-red-500 border-red-200' : 'text-orange-500 border-orange-200'}>
                    {alert.severity}
                  </Badge>
                </div>
                <div className="flex justify-between items-end mt-4">
                  <div className="text-xs font-semibold text-emerald-600 dark:text-emerald-400">
                    {alert.businessImpact}
                  </div>
                  <Button size="sm" variant="secondary" className="h-7 text-xs px-2" onClick={(e) => {
                    e.stopPropagation();
                    setSelectedAlertId(alert.id);
                  }}>
                    {alert.recommendedAction || "Review"}
                  </Button>
                </div>
              </Card>
            ))}
          </div>
        ) : (
          <Card className="p-8 text-center border-dashed">
            <div className="mx-auto w-12 h-12 bg-green-100 rounded-full flex items-center justify-center mb-3">
              <CheckCircle2 className="h-6 w-6 text-green-600" />
            </div>
            <h3 className="font-semibold text-slate-900">All caught up</h3>
            <p className="text-sm text-slate-500">No critical issues need your attention right now.</p>
          </Card>
        )}
      </div>

      <div className="grid md:grid-cols-2 gap-6 pt-4 border-t">
        {/* 6. Opportunities */}
        <div className="space-y-4">
          <h2 className="text-lg font-bold flex items-center gap-2">
            <Lightbulb className="h-5 w-5 text-emerald-500" /> Business Opportunities
          </h2>
          <div className="space-y-3">
            {topOpportunities.map((opp, idx) => (
              <Card key={idx} className="p-4 bg-emerald-50/30 dark:bg-emerald-950/20 border-emerald-100 dark:border-emerald-900/30">
                <h4 className="font-semibold text-sm">{opp.title}</h4>
                <p className="text-xs text-muted-foreground mt-1 mb-3">{opp.recommendedAction}</p>
                <div className="flex items-center justify-between">
                  <span className="text-xs font-bold text-emerald-600 dark:text-emerald-400">
                    Potential Impact: +${opp.estimatedValue.toLocaleString()}
                  </span>
                  <Button size="sm" variant="outline" className="h-7 text-xs border-emerald-200 hover:bg-emerald-100 dark:border-emerald-800 dark:hover:bg-emerald-900 text-emerald-700 dark:text-emerald-300">
                    Execute
                  </Button>
                </div>
              </Card>
            ))}
          </div>
        </div>

        {/* 7. Risks */}
        <div className="space-y-4">
          <h2 className="text-lg font-bold flex items-center gap-2">
            <AlertCircle className="h-5 w-5 text-red-500" /> Business Risks
          </h2>
          <div className="space-y-3">
            {topRisks.map((risk, idx) => (
              <Card key={idx} className="p-4 bg-red-50/30 dark:bg-red-950/20 border-red-100 dark:border-red-900/30">
                <div className="flex justify-between items-start mb-1">
                  <h4 className="font-semibold text-sm pr-4">{risk.title}</h4>
                  <Badge variant="destructive" className="text-[10px] h-4 py-0 shrink-0 uppercase tracking-wider">{risk.severity}</Badge>
                </div>
                <p className="text-xs text-muted-foreground mt-1 mb-3 leading-relaxed">
                  <span className="font-medium text-slate-700 dark:text-slate-300">Mitigation:</span> {risk.mitigation}
                </p>
                <Button size="sm" variant="outline" className="h-7 text-xs w-full border-red-200 hover:bg-red-100 dark:border-red-800 dark:hover:bg-red-900 text-red-700 dark:text-red-300">
                  Review Risk
                </Button>
              </Card>
            ))}
          </div>
        </div>
      </div>

      {/* 8. Recommended Actions (Integrated with Day 27 Action Center logic) */}
      <div className="pt-6">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-bold flex items-center gap-2">
            <Target className="h-5 w-5 text-primary" /> Recommended Actions
          </h2>
          <Button variant="ghost" size="sm" onClick={() => router.push('/dashboard/action-center')}>
            Action Center <ArrowRight className="ml-1 h-4 w-4" />
          </Button>
        </div>
        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-4">
          {nextActions.map((action, idx) => (
            <Card key={idx} className="p-4 flex flex-col justify-between h-full hover:shadow-md transition-shadow">
              <div>
                <Badge variant="secondary" className="mb-2">{action.department}</Badge>
                <h4 className="font-semibold text-sm">{action.title}</h4>
                <p className="text-xs text-muted-foreground mt-2">{action.expectedResult}</p>
              </div>
              <Button size="sm" className="w-full mt-4" onClick={() => {
                toast.success("Action Scheduled", { description: action.title });
              }}>
                {action.actionText}
              </Button>
            </Card>
          ))}
        </div>
      </div>

      {/* Details Drawer */}
      <AlertDetailsDrawer 
        alert={selectedAlert} 
        isOpen={!!selectedAlertId} 
        onClose={() => setSelectedAlertId(null)}
        onActionComplete={() => {
          setSelectedAlertId(null);
          refetchAlerts();
        }}
      />
    </div>
  );
}

// Simple internal icon wrapper
function ActivityIcon(props: any) {
  return (
    <svg
      {...props}
      xmlns="http://www.w3.org/2000/svg"
      width="24"
      height="24"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="M22 12h-4l-3 9L9 3l-3 9H2" />
    </svg>
  );
}
