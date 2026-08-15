import { AlertCircle, CheckCircle2, ShieldAlert, AlertTriangle } from "lucide-react";
import { cn } from "@/lib/utils";

interface TodayHealthProps {
  health: string;
}

export function TodayHealth({ health }: TodayHealthProps) {
  const getHealthConfig = (status: string) => {
    switch (status) {
      case "Healthy":
        return {
          icon: CheckCircle2,
          color: "text-emerald-500",
          bgColor: "bg-emerald-500/10",
          borderColor: "border-emerald-500/20",
          label: "Healthy"
        };
      case "NeedsAttention":
        return {
          icon: AlertCircle,
          color: "text-amber-500",
          bgColor: "bg-amber-500/10",
          borderColor: "border-amber-500/20",
          label: "Needs Attention"
        };
      case "AtRisk":
        return {
          icon: AlertTriangle,
          color: "text-orange-500",
          bgColor: "bg-orange-500/10",
          borderColor: "border-orange-500/20",
          label: "At Risk"
        };
      case "Critical":
        return {
          icon: ShieldAlert,
          color: "text-rose-500",
          bgColor: "bg-rose-500/10",
          borderColor: "border-rose-500/20",
          label: "Critical"
        };
      default:
        return {
          icon: CheckCircle2,
          color: "text-muted-foreground",
          bgColor: "bg-muted",
          borderColor: "border-border",
          label: "Unknown"
        };
    }
  };

  const config = getHealthConfig(health);
  const Icon = config.icon;

  return (
    <div className="flex items-center gap-3">
      <div className={cn("flex items-center gap-2 px-3 py-1.5 rounded-full border text-sm font-medium", config.bgColor, config.borderColor, config.color)}>
        <Icon className="w-4 h-4" />
        {config.label}
      </div>
      <span className="text-sm font-medium text-muted-foreground uppercase tracking-wider">Business Health</span>
    </div>
  );
}
