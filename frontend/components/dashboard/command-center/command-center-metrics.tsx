import { CommandMetricDto } from "@/lib/command-center-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ArrowDownRight, ArrowUpRight, Minus } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import Link from "next/link";

interface CommandCenterMetricsProps {
  metrics?: CommandMetricDto[];
  isLoading: boolean;
}

export function CommandCenterMetrics({ metrics, isLoading }: CommandCenterMetricsProps) {
  if (isLoading) {
    return (
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
        {[1, 2, 3, 4].map((i) => (
          <Card key={i}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <Skeleton className="h-4 w-20" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-7 w-24 mb-1" />
              <Skeleton className="h-3 w-16" />
            </CardContent>
          </Card>
        ))}
      </div>
    );
  }

  if (!metrics || metrics.length === 0) {
    return null;
  }

  // Helper to figure out the right dashboard link based on category
  const getLinkForCategory = (category: string) => {
    const map: Record<string, string> = {
      "Finance": "/dashboard/finance",
      "Sales": "/dashboard/crm",
      "CRM": "/dashboard/crm",
      "Inventory": "/dashboard/inventory",
      "Growth": "/dashboard/growth-center"
    };
    return map[category] || "/dashboard";
  };

  return (
    <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
      {metrics.map((metric) => {
        const isUp = metric.TrendDirection === "Up";
        const isDown = metric.TrendDirection === "Down";
        const isFlat = metric.TrendDirection === "Flat";
        
        // Generally up is good, down is bad, but for "Outstanding Payments" or "Churn", it's the opposite.
        // A simple heuristic for this demo:
        const isNegativeMetric = metric.Name.toLowerCase().includes("outstanding") || 
                                 metric.Name.toLowerCase().includes("churn") ||
                                 metric.Name.toLowerCase().includes("failed");
                                 
        const trendColor = isUp ? (isNegativeMetric ? "text-red-500" : "text-green-500") 
                         : isDown ? (isNegativeMetric ? "text-green-500" : "text-red-500") 
                         : "text-muted-foreground";

        return (
          <Link href={getLinkForCategory(metric.Category)} key={metric.Id} className="block group">
            <Card className="transition-all hover:border-primary/50 hover:shadow-sm">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium text-muted-foreground group-hover:text-foreground transition-colors">
                  {metric.Name}
                </CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{metric.FormattedValue}</div>
                <div className="flex items-center text-xs mt-1">
                  <span className={`flex items-center font-medium ${trendColor}`}>
                    {isUp && <ArrowUpRight className="h-3 w-3 mr-1" />}
                    {isDown && <ArrowDownRight className="h-3 w-3 mr-1" />}
                    {isFlat && <Minus className="h-3 w-3 mr-1" />}
                    {Math.abs(metric.TrendPercentage).toFixed(1)}%
                  </span>
                  <span className="text-muted-foreground ml-2">vs last period</span>
                </div>
              </CardContent>
            </Card>
          </Link>
        );
      })}
    </div>
  );
}
