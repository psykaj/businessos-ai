"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Users, Briefcase, TrendingUp, DollarSign, Activity } from "lucide-react";
import { formatCurrency } from "@/lib/utils";
import { 
  LineChart, 
  Line, 
  XAxis, 
  YAxis, 
  CartesianGrid, 
  Tooltip, 
  ResponsiveContainer 
} from "recharts";
import { Skeleton } from "@/components/ui/skeleton";

export default function CrmDashboardPage() {
  const { data: overview, isLoading: overviewLoading } = useQuery({
    queryKey: ['crm-overview'],
    queryFn: () => crmService.getOverview()
  });

  const { data: performance, isLoading: performanceLoading } = useQuery({
    queryKey: ['crm-performance'],
    queryFn: () => crmService.getSalesPerformance()
  });

  const { data: activities, isLoading: activitiesLoading } = useQuery({
    queryKey: ['crm-recent-activities'],
    queryFn: () => crmService.getActivities()
  });

  if (overviewLoading) {
    return (
      <div className="p-8 space-y-8">
        <h1 className="text-3xl font-bold">CRM Dashboard</h1>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {[1, 2, 3, 4].map(i => <Skeleton key={i} className="h-32 w-full rounded-xl" />)}
        </div>
      </div>
    );
  }

  return (
    <div className="p-8 space-y-8">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold tracking-tight">CRM Dashboard</h1>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Leads</CardTitle>
            <Users className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{overview?.totalLeads || 0}</div>
            <p className="text-xs text-muted-foreground">Active in pipeline</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Deals</CardTitle>
            <Briefcase className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{overview?.totalDeals || 0}</div>
            <p className="text-xs text-muted-foreground">Opportunities created</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Pipeline Value</CardTitle>
            <DollarSign className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{formatCurrency(overview?.pipelineValue || 0)}</div>
            <p className="text-xs text-muted-foreground">Forecasted: {formatCurrency(overview?.revenueForecast || 0)}</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Win Rate</CardTitle>
            <TrendingUp className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{overview?.conversionRate?.toFixed(1) || 0}%</div>
            <p className="text-xs text-muted-foreground">Deal conversion rate</p>
          </CardContent>
        </Card>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle>Revenue Forecast (6 Months)</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="h-[300px] w-full">
              {performanceLoading ? (
                <Skeleton className="h-full w-full" />
              ) : performance && performance.length > 0 ? (
                <ResponsiveContainer width="100%" height="100%">
                  <LineChart data={performance}>
                    <CartesianGrid strokeDasharray="3 3" vertical={false} />
                    <XAxis 
                      dataKey="date" 
                      tickFormatter={(val) => {
                        const date = new Date(val);
                        return `${date.toLocaleString('default', { month: 'short' })} ${date.getFullYear()}`;
                      }} 
                    />
                    <YAxis tickFormatter={(val) => `$${val}`} />
                    <Tooltip 
                      formatter={(value: any) => [formatCurrency(Number(value) || 0), "Revenue"]}
                      labelFormatter={(label) => {
                        const date = new Date(label);
                        return `${date.toLocaleString('default', { month: 'long' })} ${date.getFullYear()}`;
                      }}
                    />
                    <Line type="monotone" dataKey="revenue" stroke="#2563eb" strokeWidth={3} dot={{ r: 4 }} activeDot={{ r: 8 }} />
                  </LineChart>
                </ResponsiveContainer>
              ) : (
                <div className="flex h-full items-center justify-center text-muted-foreground">
                  No performance data available
                </div>
              )}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Recent Activity</CardTitle>
          </CardHeader>
          <CardContent>
            {activitiesLoading ? (
              <div className="space-y-4">
                {[1, 2, 3].map(i => <Skeleton key={i} className="h-12 w-full" />)}
              </div>
            ) : activities && activities.length > 0 ? (
              <div className="space-y-6">
                {activities.slice(0, 5).map((activity) => (
                  <div key={activity.id} className="flex items-start gap-4">
                    <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-primary/10">
                      <Activity className="h-4 w-4 text-primary" />
                    </div>
                    <div className="space-y-1">
                      <p className="text-sm font-medium leading-none">{activity.description}</p>
                      <p className="text-xs text-muted-foreground">
                        {activity.relatedEntity} • {new Date(activity.activityDate).toLocaleDateString()}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-sm text-muted-foreground">No recent activities.</div>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
