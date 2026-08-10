import { BusinessHealthDto } from "@/lib/command-center-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Activity, ArrowDownRight, ArrowUpRight, Minus } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";

interface CommandCenterHealthProps {
  health?: BusinessHealthDto;
  isLoading: boolean;
}

export function CommandCenterHealth({ health, isLoading }: CommandCenterHealthProps) {
  if (isLoading) {
    return (
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Business Health</CardTitle>
          <Skeleton className="h-4 w-4 rounded-full" />
        </CardHeader>
        <CardContent>
          <Skeleton className="h-8 w-16 mb-2" />
          <Skeleton className="h-2 w-full mb-2" />
          <Skeleton className="h-4 w-full" />
        </CardContent>
      </Card>
    );
  }

  if (!health) {
    return (
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Business Health</CardTitle>
          <Activity className="h-4 w-4 text-muted-foreground" />
        </CardHeader>
        <CardContent>
          <div className="text-sm text-muted-foreground">Health data unavailable.</div>
        </CardContent>
      </Card>
    );
  }

  let statusColor = "bg-green-500/10 text-green-700 dark:text-green-400 border-green-500/20";
  let progressColor = "bg-green-500";
  
  if (health.Status === "AtRisk") {
    statusColor = "bg-amber-500/10 text-amber-700 dark:text-amber-400 border-amber-500/20";
    progressColor = "bg-amber-500";
  } else if (health.Status === "Critical") {
    statusColor = "bg-red-500/10 text-red-700 dark:text-red-400 border-red-500/20";
    progressColor = "bg-red-500";
  }

  return (
    <Card className="overflow-hidden relative">
      <div className={`absolute top-0 left-0 w-1 h-full ${progressColor}`} />
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle className="text-sm font-medium">Business Health</CardTitle>
        <Badge variant="outline" className={statusColor}>
          {health.Status}
        </Badge>
      </CardHeader>
      <CardContent>
        <div className="flex items-baseline space-x-2">
          <div className="text-3xl font-bold">{health.HealthScore}</div>
          <div className="text-sm text-muted-foreground">/ 100</div>
          {health.Trend === "Improving" && <ArrowUpRight className="h-4 w-4 text-green-500 ml-2" />}
          {health.Trend === "Declining" && <ArrowDownRight className="h-4 w-4 text-red-500 ml-2" />}
          {health.Trend === "Stable" && <Minus className="h-4 w-4 text-muted-foreground ml-2" />}
        </div>
        <Progress 
          value={health.HealthScore} 
          className="h-2 mt-3 mb-4" 
        />
        <p className="text-sm text-muted-foreground">{health.Explanation}</p>
      </CardContent>
    </Card>
  );
}
