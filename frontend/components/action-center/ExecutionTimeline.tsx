"use client";

import { AiActionDto } from "@/types/action-center";
import { format } from "date-fns";
import { CheckCircle2, Clock, XCircle, Activity } from "lucide-react";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

interface ExecutionTimelineProps {
  actions: AiActionDto[];
  isLoading: boolean;
}

export function ExecutionTimeline({ actions, isLoading }: ExecutionTimelineProps) {
  // Sort actions by their most recent activity (executedDate if exists, else createdAt)
  const sortedActions = [...actions].sort((a, b) => {
    const dateA = new Date(a.executedDate || a.createdAt).getTime();
    const dateB = new Date(b.executedDate || b.createdAt).getTime();
    return dateB - dateA;
  });

  const getStatusIcon = (status: string) => {
    switch (status) {
      case "Executed":
        return <CheckCircle2 className="h-4 w-4 text-emerald-500" />;
      case "Failed":
      case "Rejected":
        return <XCircle className="h-4 w-4 text-rose-500" />;
      case "Approved":
        return <CheckCircle2 className="h-4 w-4 text-blue-500" />;
      case "Pending":
      default:
        return <Clock className="h-4 w-4 text-amber-500" />;
    }
  };

  return (
    <Card className="h-full flex flex-col">
      <CardHeader className="pb-3">
        <CardTitle className="text-lg font-semibold flex items-center gap-2">
          <Activity className="h-5 w-5" />
          Execution Timeline
        </CardTitle>
      </CardHeader>
      <CardContent className="flex-1 p-0 relative">
        <ScrollArea className="h-[400px] w-full px-6">
          {isLoading ? (
            <div className="space-y-6 pt-2">
              {[1, 2, 3, 4].map((i) => (
                <div key={i} className="flex gap-4">
                  <div className="mt-1 h-4 w-4 rounded-full bg-muted animate-pulse" />
                  <div className="flex-1 space-y-2">
                    <div className="h-4 w-1/3 bg-muted animate-pulse rounded" />
                    <div className="h-3 w-1/2 bg-muted animate-pulse rounded" />
                  </div>
                </div>
              ))}
            </div>
          ) : sortedActions.length === 0 ? (
            <div className="flex items-center justify-center h-full text-muted-foreground text-sm">
              No recent activity.
            </div>
          ) : (
            <div className="relative border-l border-muted ml-2 space-y-6 pt-2 pb-6">
              {sortedActions.map((action, i) => (
                <div key={action.id} className="relative pl-6">
                  {/* Timeline Node */}
                  <span className="absolute -left-[9px] top-1 bg-background rounded-full border border-muted-foreground/20 shadow-sm">
                    {getStatusIcon(action.status)}
                  </span>
                  
                  <div className="flex flex-col gap-1">
                    <div className="flex items-center gap-2 flex-wrap">
                      <span className="text-sm font-medium line-clamp-1 flex-1">
                        {action.title}
                      </span>
                      <Badge variant="outline" className="text-[10px] px-1 py-0 h-4">
                        {action.status}
                      </Badge>
                    </div>
                    
                    <span className="text-xs text-muted-foreground">
                      {format(new Date(action.executedDate || action.createdAt), "MMM d, yyyy 'at' h:mm a")}
                    </span>
                    
                    {action.executionResult && (
                      <p className="text-xs mt-1 bg-muted/50 p-2 rounded border line-clamp-2">
                        {action.executionResult}
                      </p>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </ScrollArea>
      </CardContent>
    </Card>
  );
}
