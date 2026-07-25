import React from "react";
import { Badge } from "@/components/ui/badge";
import { HeartPulse, AlertTriangle, CheckCircle2, ShieldAlert } from "lucide-react";

interface HealthScoreBadgeProps {
  score?: number;
  riskLevel?: "Healthy" | "Stable" | "Needs Attention" | "High Risk" | string;
  showScore?: boolean;
}

export function HealthScoreBadge({ score, riskLevel = "Healthy", showScore = true }: HealthScoreBadgeProps) {
  const getBadgeConfig = () => {
    switch (riskLevel) {
      case "Healthy":
        return {
          variant: "outline" as const,
          className: "border-emerald-500/30 bg-emerald-500/10 text-emerald-600 dark:text-emerald-400 font-medium",
          icon: CheckCircle2,
          label: "Healthy",
        };
      case "Stable":
        return {
          variant: "outline" as const,
          className: "border-blue-500/30 bg-blue-500/10 text-blue-600 dark:text-blue-400 font-medium",
          icon: HeartPulse,
          label: "Stable",
        };
      case "Needs Attention":
        return {
          variant: "outline" as const,
          className: "border-amber-500/30 bg-amber-500/10 text-amber-600 dark:text-amber-400 font-medium",
          icon: AlertTriangle,
          label: "Needs Attention",
        };
      case "High Risk":
        return {
          variant: "outline" as const,
          className: "border-rose-500/30 bg-rose-500/10 text-rose-600 dark:text-rose-400 font-medium animate-pulse",
          icon: ShieldAlert,
          label: "High Risk",
        };
      default:
        return {
          variant: "outline" as const,
          className: "border-slate-500/30 bg-slate-500/10 text-slate-600 dark:text-slate-400",
          icon: HeartPulse,
          label: riskLevel,
        };
    }
  };

  const config = getBadgeConfig();
  const Icon = config.icon;

  return (
    <Badge variant={config.variant} className={`inline-flex items-center gap-1.5 px-2.5 py-0.5 text-xs ${config.className}`}>
      <Icon className="h-3.5 w-3.5" />
      <span>{config.label}</span>
      {showScore && score !== undefined && (
        <span className="ml-1 pl-1.5 border-l border-current/20 font-bold font-mono">{score}/100</span>
      )}
    </Badge>
  );
}
