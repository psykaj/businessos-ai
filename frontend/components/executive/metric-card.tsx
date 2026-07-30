import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { TrendingUp, TrendingDown, Minus } from "lucide-react";
import { cn } from "@/lib/utils";

interface MetricCardProps {
  title: string;
  value: string | number;
  trend: number;
  description?: string;
  className?: string;
}

export function MetricCard({ title, value, trend, description, className }: MetricCardProps) {
  const isPositive = trend > 0;
  const isNeutral = trend === 0;

  return (
    <Card className={cn("overflow-hidden", className)}>
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle className="text-sm font-medium">{title}</CardTitle>
        <div
          className={cn(
            "flex items-center text-xs font-medium px-2 py-1 rounded-full",
            isPositive
              ? "bg-emerald-500/10 text-emerald-500"
              : isNeutral
              ? "bg-slate-500/10 text-slate-500"
              : "bg-red-500/10 text-red-500"
          )}
        >
          {isPositive ? (
            <TrendingUp className="mr-1 h-3 w-3" />
          ) : isNeutral ? (
            <Minus className="mr-1 h-3 w-3" />
          ) : (
            <TrendingDown className="mr-1 h-3 w-3" />
          )}
          {Math.abs(trend).toFixed(1)}%
        </div>
      </CardHeader>
      <CardContent>
        <div className="text-2xl font-bold">{value}</div>
        {description && (
          <p className="text-xs text-muted-foreground mt-1">{description}</p>
        )}
      </CardContent>
    </Card>
  );
}
