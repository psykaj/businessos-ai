import { CommandActivityDto } from "@/lib/command-center-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Activity } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { formatDistanceToNow } from "date-fns";

interface CommandCenterActivityProps {
  activities?: CommandActivityDto[];
  isLoading: boolean;
}

export function CommandCenterActivity({ activities, isLoading }: CommandCenterActivityProps) {
  if (isLoading) {
    return (
      <Card className="h-full">
        <CardHeader className="pb-4 border-b">
          <CardTitle className="text-sm font-medium flex items-center">
            <Activity className="w-4 h-4 mr-2" /> Recent Business Activity
          </CardTitle>
        </CardHeader>
        <CardContent className="pt-4 space-y-4">
          {[1, 2, 3, 4].map((i) => (
            <div key={i} className="flex gap-3">
              <Skeleton className="w-2 h-2 rounded-full mt-1.5" />
              <div className="space-y-2 flex-1">
                <Skeleton className="h-3 w-[80%]" />
                <Skeleton className="h-2 w-16" />
              </div>
            </div>
          ))}
        </CardContent>
      </Card>
    );
  }

  if (!activities || activities.length === 0) {
    return (
      <Card className="h-full">
        <CardHeader className="pb-4 border-b">
          <CardTitle className="text-sm font-medium flex items-center">
            <Activity className="w-4 h-4 mr-2 text-muted-foreground" /> Recent Business Activity
          </CardTitle>
        </CardHeader>
        <CardContent className="pt-6 text-center text-sm text-muted-foreground">
          No recent activity to show.
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className="h-full flex flex-col">
      <CardHeader className="pb-4 border-b shrink-0">
        <CardTitle className="text-sm font-medium flex items-center">
          <Activity className="w-4 h-4 mr-2" /> Recent Business Activity
        </CardTitle>
      </CardHeader>
      <CardContent className="pt-4 flex-1 overflow-auto max-h-[300px]">
        <div className="relative border-l border-muted ml-2 space-y-6">
          {activities.map((activity, idx) => {
            const timeAgo = formatDistanceToNow(new Date(activity.Timestamp), { addSuffix: true });
            
            return (
              <div key={activity.Id || idx} className="relative pl-6">
                <span className="absolute left-[-5px] top-1.5 h-2.5 w-2.5 rounded-full bg-primary/20 ring-4 ring-background">
                  <span className="absolute inline-flex h-full w-full animate-ping rounded-full bg-primary opacity-20"></span>
                  <span className="relative inline-flex h-2.5 w-2.5 rounded-full bg-primary"></span>
                </span>
                <div className="flex flex-col">
                  <span className="text-sm font-medium leading-none mb-1">{activity.Description}</span>
                  <div className="flex items-center text-[10px] text-muted-foreground uppercase tracking-wider gap-2 mt-1">
                    <span>{activity.SourceModule}</span>
                    <span>•</span>
                    <span>{timeAgo}</span>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </CardContent>
    </Card>
  );
}
