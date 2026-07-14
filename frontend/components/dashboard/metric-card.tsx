import { cn } from "@/lib/utils";
import { TrendingUp, TrendingDown } from "lucide-react";
import type { MetricCardData } from "@/types/navigation";

interface MetricCardProps {
  data: MetricCardData;
  className?: string;
}

export function MetricCard({ data, className }: MetricCardProps) {
  const Icon = data.icon;
  const isPositive = data.trend === "up";

  return (
    <div
      className={cn(
        "group relative rounded-xl border border-border bg-card p-5 transition-all duration-200 hover:border-border/80 hover:shadow-sm",
        className
      )}
    >
      <div className="flex items-start justify-between">
        <div className="flex flex-col gap-1">
          <span className="text-sm font-medium text-muted-foreground">
            {data.title}
          </span>
          <span className="text-2xl font-semibold tracking-tight text-card-foreground">
            {data.value}
          </span>
        </div>

        <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-primary/[0.06]">
          <Icon className="h-5 w-5 text-primary/70" />
        </div>
      </div>

      <div className="mt-3 flex items-center gap-1.5">
        {isPositive ? (
          <TrendingUp className="h-3.5 w-3.5 text-emerald-600" />
        ) : (
          <TrendingDown className="h-3.5 w-3.5 text-red-500" />
        )}
        <span
          className={cn(
            "text-xs font-medium",
            isPositive ? "text-emerald-600" : "text-red-500"
          )}
        >
          {data.change}
        </span>
        <span className="text-xs text-muted-foreground">vs last month</span>
      </div>
    </div>
  );
}
