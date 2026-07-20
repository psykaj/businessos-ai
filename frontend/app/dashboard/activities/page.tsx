"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Activity, Phone, Mail, Calendar, FileText, Settings } from "lucide-react";
import { formatDate } from "@/lib/utils";
import { ActivityType } from "@/types/crm";

const getActivityIcon = (type: ActivityType) => {
  switch (type) {
    case ActivityType.Call: return <Phone className="h-4 w-4 text-blue-500" />;
    case ActivityType.Email: return <Mail className="h-4 w-4 text-yellow-500" />;
    case ActivityType.Meeting: return <Calendar className="h-4 w-4 text-purple-500" />;
    case ActivityType.Note: return <FileText className="h-4 w-4 text-green-500" />;
    case ActivityType.SystemEvent: return <Settings className="h-4 w-4 text-slate-500" />;
    default: return <Activity className="h-4 w-4 text-slate-500" />;
  }
};

export default function ActivitiesPage() {
  const { data: activities, isLoading } = useQuery({
    queryKey: ['crm-activities'],
    queryFn: () => crmService.getActivities()
  });

  return (
    <div className="p-8 space-y-6 max-w-4xl mx-auto">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Activity Timeline</h1>
        <p className="text-muted-foreground">A chronological history of all CRM interactions and system events.</p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Recent Activities</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-6">
              {[1, 2, 3, 4, 5].map(i => (
                <div key={i} className="flex gap-4">
                  <Skeleton className="h-10 w-10 rounded-full shrink-0" />
                  <div className="space-y-2 flex-1">
                    <Skeleton className="h-4 w-full" />
                    <Skeleton className="h-3 w-1/3" />
                  </div>
                </div>
              ))}
            </div>
          ) : activities && activities.length > 0 ? (
            <div className="relative space-y-8 before:absolute before:inset-0 before:ml-5 before:-translate-x-px md:before:mx-auto md:before:translate-x-0 before:h-full before:w-0.5 before:bg-gradient-to-b before:from-transparent before:via-border before:to-transparent">
              {activities.map((activity) => (
                <div key={activity.id} className="relative flex items-start justify-between md:justify-normal md:odd:flex-row-reverse group is-active">
                  
                  {/* Icon */}
                  <div className="flex items-center justify-center w-10 h-10 rounded-full border bg-background shrink-0 md:order-1 md:group-odd:-translate-x-1/2 md:group-even:translate-x-1/2 shadow-sm z-10">
                    {getActivityIcon(activity.type)}
                  </div>
                  
                  {/* Content */}
                  <div className="w-[calc(100%-4rem)] md:w-[calc(50%-2.5rem)] p-4 rounded-xl border bg-card shadow-sm">
                    <div className="flex items-center justify-between mb-1">
                      <span className="font-semibold text-sm">{activity.relatedEntity}</span>
                      <time className="text-xs text-muted-foreground">{formatDate(activity.activityDate)}</time>
                    </div>
                    <p className="text-sm text-muted-foreground">{activity.description}</p>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <div className="text-center py-10 text-muted-foreground">
              <Activity className="h-10 w-10 mx-auto text-muted-foreground/50 mb-3" />
              <p>No activities recorded yet.</p>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
