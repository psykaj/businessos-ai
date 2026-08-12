import React from "react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { ArrowRight, IndianRupee, Clock, Zap, TrendingUp, CheckCircle2 } from "lucide-react";
import { useRoiSummary } from "@/hooks/use-outcomes";
import { useRouter } from "next/navigation";
import { Skeleton } from "@/components/ui/skeleton";

interface CommandCenterBusinessValueProps {
  isLoading?: boolean;
}

export function CommandCenterBusinessValue({ isLoading: externalLoading }: CommandCenterBusinessValueProps) {
  const router = useRouter();
  
  // We'll fetch the last 30 days as default for the Command Center view
  const params = React.useMemo(() => {
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
    return {
      startDate: thirtyDaysAgo.toISOString(),
      estimatedHourlyCost: 1000
    };
  }, []);
  
  const { data: summary, isLoading: queryLoading } = useRoiSummary(params);

  const isLoading = externalLoading || queryLoading;

  return (
    <Card className="border-primary/20 shadow-sm bg-gradient-to-br from-background to-primary/5">
      <CardHeader className="pb-3 flex flex-row items-start justify-between">
        <div>
          <CardTitle className="text-xl flex items-center gap-2">
            <TrendingUp className="w-5 h-5 text-primary" />
            Business Value Created
          </CardTitle>
          <CardDescription>
            Measurable impact from BusinessOS AI (Last 30 days)
          </CardDescription>
        </div>
        <Button variant="outline" size="sm" onClick={() => router.push("/dashboard/outcomes")}>
          View Outcomes <ArrowRight className="w-4 h-4 ml-2" />
        </Button>
      </CardHeader>
      <CardContent>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          
          {/* Revenue Recovered */}
          <div className="space-y-2 p-4 rounded-xl bg-background/60 border">
            <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground">
              <div className="p-1.5 bg-green-100 text-green-700 rounded-md">
                <IndianRupee className="w-4 h-4" />
              </div>
              Revenue Recovered
            </div>
            {isLoading ? (
              <Skeleton className="h-8 w-24" />
            ) : (
              <div className="text-2xl font-bold text-green-600">
                {new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(summary?.revenueRecovered || 0)}
              </div>
            )}
          </div>

          {/* Time Saved */}
          <div className="space-y-2 p-4 rounded-xl bg-background/60 border">
            <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground">
              <div className="p-1.5 bg-blue-100 text-blue-700 rounded-md">
                <Clock className="w-4 h-4" />
              </div>
              Estimated Time Saved
            </div>
            {isLoading ? (
              <Skeleton className="h-8 w-24" />
            ) : (
              <div className="text-2xl font-bold">
                {((summary?.timeSavedMinutes || 0) / 60).toFixed(1)} <span className="text-base font-normal text-muted-foreground">hrs</span>
              </div>
            )}
          </div>

          {/* Successful Actions */}
          <div className="space-y-2 p-4 rounded-xl bg-background/60 border">
            <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground">
              <div className="p-1.5 bg-purple-100 text-purple-700 rounded-md">
                <CheckCircle2 className="w-4 h-4" />
              </div>
              Successful Actions
            </div>
            {isLoading ? (
              <Skeleton className="h-8 w-24" />
            ) : (
              <div className="text-2xl font-bold">
                {summary?.successfulActions || 0}
              </div>
            )}
          </div>

          {/* Automations */}
          <div className="space-y-2 p-4 rounded-xl bg-background/60 border">
            <div className="flex items-center gap-2 text-sm font-medium text-muted-foreground">
              <div className="p-1.5 bg-amber-100 text-amber-700 rounded-md">
                <Zap className="w-4 h-4" />
              </div>
              Successful Automations
            </div>
            {isLoading ? (
              <Skeleton className="h-8 w-24" />
            ) : (
              <div className="text-2xl font-bold">
                {summary?.successfulAutomations || 0}
              </div>
            )}
          </div>

        </div>
      </CardContent>
    </Card>
  );
}
