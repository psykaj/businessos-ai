import { CommandAutomationSummaryDto } from "@/lib/command-center-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Workflow, PlayCircle, AlertTriangle, Clock } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import Link from "next/link";
import { Button, buttonVariants } from "@/components/ui/button";

interface CommandCenterAutomationsProps {
  summary?: CommandAutomationSummaryDto;
  isLoading: boolean;
}

export function CommandCenterAutomations({ summary, isLoading }: CommandCenterAutomationsProps) {
  if (isLoading) {
    return (
      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-sm font-medium flex items-center">
            <Workflow className="w-4 h-4 mr-2" /> Automation Health
          </CardTitle>
        </CardHeader>
        <CardContent className="grid grid-cols-2 gap-4">
          <div><Skeleton className="h-6 w-12 mb-1"/><Skeleton className="h-3 w-20"/></div>
          <div><Skeleton className="h-6 w-12 mb-1"/><Skeleton className="h-3 w-20"/></div>
        </CardContent>
      </Card>
    );
  }

  if (!summary) return null;

  return (
    <Card className="h-full flex flex-col">
      <CardHeader className="pb-2">
        <CardTitle className="text-sm font-medium flex items-center justify-between">
          <div className="flex items-center">
            <Workflow className="w-4 h-4 mr-2 text-primary" /> 
            Automation Health
          </div>
          <Link href="/dashboard/automation" className="text-xs text-primary hover:underline font-normal">
            View All
          </Link>
        </CardTitle>
      </CardHeader>
      <CardContent className="flex-1 flex flex-col justify-center">
        <div className="grid grid-cols-2 gap-y-6 gap-x-4">
          <div>
            <div className="flex items-center text-muted-foreground mb-1">
              <PlayCircle className="w-3.5 h-3.5 mr-1" />
              <span className="text-xs">Active</span>
            </div>
            <div className="text-2xl font-semibold">{summary.activeWorkflowsCount}</div>
          </div>
          
          <div>
            <div className="flex items-center text-muted-foreground mb-1">
              <ZapIcon className="w-3.5 h-3.5 mr-1 text-yellow-500" />
              <span className="text-xs">Executions</span>
            </div>
            <div className="text-2xl font-semibold">{summary.actionsExecutedToday}</div>
          </div>

          <div>
            <div className="flex items-center text-muted-foreground mb-1">
              <Clock className="w-3.5 h-3.5 mr-1 text-blue-500" />
              <span className="text-xs">Time Saved</span>
            </div>
            <div className="text-2xl font-semibold">{summary.timeSavedHours}h</div>
          </div>

          <div>
            <div className="flex items-center text-muted-foreground mb-1">
              <AlertTriangle className={`w-3.5 h-3.5 mr-1 ${summary.failedExecutionsToday > 0 ? 'text-red-500' : ''}`} />
              <span className="text-xs">Failed</span>
            </div>
            <div className="text-2xl font-semibold">
              {summary.failedExecutionsToday > 0 ? (
                <span className="text-red-500">{summary.failedExecutionsToday}</span>
              ) : (
                "0"
              )}
            </div>
          </div>
        </div>

        {summary.failedExecutionsToday > 0 && (
          <Link href="/dashboard/automation" className={buttonVariants({ variant: "destructive", size: "sm", className: "w-full mt-4 h-8" })}>Review Failures</Link>
        )}
      </CardContent>
    </Card>
  );
}

function ZapIcon(props: any) {
  return (
    <svg
      {...props}
      xmlns="http://www.w3.org/2000/svg"
      width="24"
      height="24"
      viewBox="0 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2" />
    </svg>
  );
}
