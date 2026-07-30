"use client";

import { useBusinessHealth, useBusinessHealthHistory, useCalculateBusinessHealth } from "@/hooks/use-executive";
import { HealthGauge } from "@/components/executive/health-gauge";
import { TrendChart } from "@/components/executive/trend-chart";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { RefreshCw, Activity, DollarSign, Users } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { toast } from "sonner";
import { format } from "date-fns";

export default function BusinessHealthPage() {
  const { data: health, isLoading: healthLoading } = useBusinessHealth();
  const { data: history, isLoading: historyLoading } = useBusinessHealthHistory();
  const { mutate: calculateHealth, isPending: isCalculating } = useCalculateBusinessHealth();

  const handleRecalculate = () => {
    calculateHealth(undefined, {
      onSuccess: () => {
        toast.success("Business health recalculated successfully.");
      },
      onError: () => {
        toast.error("Failed to recalculate business health.");
      }
    });
  };

  const formattedHistory = history?.map(h => ({
    ...h,
    dateLabel: format(new Date(h.calculatedAt), "MMM dd")
  })).reverse() || [];

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-2 md:flex-row md:items-center md:justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Business Health</h1>
          <p className="text-muted-foreground">In-depth analysis of your company's overall health score.</p>
        </div>
        <Button onClick={handleRecalculate} disabled={isCalculating} variant="outline">
          <RefreshCw className={`mr-2 h-4 w-4 ${isCalculating ? "animate-spin" : ""}`} />
          Recalculate Now
        </Button>
      </div>

      <div className="grid gap-6 md:grid-cols-3">
        {healthLoading ? (
          <Skeleton className="h-[300px] col-span-3 md:col-span-1" />
        ) : (
          <div className="col-span-3 md:col-span-1">
            <HealthGauge 
              score={health?.overallScore || 0} 
              description={health?.status || "Unknown Status"}
            />
          </div>
        )}

        <div className="col-span-3 md:col-span-2">
          {historyLoading ? (
            <Skeleton className="h-[300px] w-full" />
          ) : (
            <TrendChart 
              title="Health Score Trend"
              description="Your business health over the last 12 periods"
              data={formattedHistory}
              xKey="dateLabel"
              yKey="overallScore"
              color="#10b981"
            />
          )}
        </div>
      </div>

      <h2 className="text-xl font-bold tracking-tight mt-8 mb-4">Health Breakdown</h2>
      
      <div className="grid gap-4 md:grid-cols-3">
        {healthLoading ? (
          <>
            <Skeleton className="h-32" />
            <Skeleton className="h-32" />
            <Skeleton className="h-32" />
          </>
        ) : (
          <>
            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">Financial Health</CardTitle>
                <DollarSign className="h-4 w-4 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-emerald-500">{health?.financialHealth?.toFixed(1) || 0}/100</div>
                <p className="text-xs text-muted-foreground mt-1">Based on revenue and cash flow.</p>
              </CardContent>
            </Card>

            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">Operational Health</CardTitle>
                <Activity className="h-4 w-4 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-blue-500">{health?.operationalHealth?.toFixed(1) || 0}/100</div>
                <p className="text-xs text-muted-foreground mt-1">Based on workflow efficiency.</p>
              </CardContent>
            </Card>

            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">Customer Health</CardTitle>
                <Users className="h-4 w-4 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-purple-500">{health?.customerHealth?.toFixed(1) || 0}/100</div>
                <p className="text-xs text-muted-foreground mt-1">Based on churn and satisfaction.</p>
              </CardContent>
            </Card>
          </>
        )}
      </div>
    </div>
  );
}
