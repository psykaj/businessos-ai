"use client";

import { useBusinessHealth, useExecutiveInsights, useKpis, useLatestForecast, useBusinessGoals } from "@/hooks/use-executive";
import { MetricCard } from "@/components/executive/metric-card";
import { HealthGauge } from "@/components/executive/health-gauge";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { AlertCircle, Target, ArrowRight } from "lucide-react";
import Link from "next/link";
import { Skeleton } from "@/components/ui/skeleton";

export default function ExecutiveDashboardPage() {
  const { data: health, isLoading: healthLoading } = useBusinessHealth();
  const { data: insights, isLoading: insightsLoading } = useExecutiveInsights(3);
  const { data: kpis, isLoading: kpisLoading } = useKpis();
  const { data: goals, isLoading: goalsLoading } = useBusinessGoals();

  const revenueKpi = kpis?.find(k => k.name.toLowerCase().includes("revenue"));
  const leadKpi = kpis?.find(k => k.name.toLowerCase().includes("lead"));

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-2 md:flex-row md:items-center md:justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Executive Command Center</h1>
          <p className="text-muted-foreground">Your high-level business intelligence overview.</p>
        </div>
        <div className="flex gap-2">
          <Link href="/dashboard/recommendations">
            <Button variant="outline">View AI Recommendations</Button>
          </Link>
          <Link href="/dashboard/kpis">
            <Button>KPI Workspace</Button>
          </Link>
        </div>
      </div>

      <div className="grid gap-4 md:grid-cols-4">
        {healthLoading ? <Skeleton className="h-48 col-span-1" /> : (
          <div className="col-span-1">
            <HealthGauge 
              score={health?.overallScore || 0} 
              description={health?.status}
            />
          </div>
        )}
        
        <div className="col-span-1 md:col-span-3 grid gap-4 md:grid-cols-3">
          {kpisLoading ? (
            <>
              <Skeleton className="h-32" />
              <Skeleton className="h-32" />
              <Skeleton className="h-32" />
            </>
          ) : (
            <>
              <MetricCard 
                title={revenueKpi?.name || "Revenue Growth"}
                value={`${revenueKpi?.currentValue.toFixed(1) || 0}%`}
                trend={revenueKpi?.currentValue || 0}
                description="Month over month"
              />
              <MetricCard 
                title={leadKpi?.name || "Lead Conversion"}
                value={`${leadKpi?.currentValue.toFixed(1) || 0}%`}
                trend={leadKpi?.currentValue || 0}
                description="Total conversion rate"
              />
              <MetricCard 
                title="Active Goals"
                value={goals?.filter(g => g.status === "On Track").length || 0}
                trend={5.0}
                description="Goals currently on track"
              />
            </>
          )}
        </div>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>AI Executive Insights</CardTitle>
                <CardDescription>Automated insights generated from your business data</CardDescription>
              </div>
              <Link href="/dashboard/recommendations">
                <Button variant="ghost" size="sm">
                  View All <ArrowRight className="ml-1 h-4 w-4" />
                </Button>
              </Link>
            </div>
          </CardHeader>
          <CardContent>
            {insightsLoading ? (
              <div className="space-y-4">
                <Skeleton className="h-16 w-full" />
                <Skeleton className="h-16 w-full" />
              </div>
            ) : insights?.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">No critical insights at this time.</div>
            ) : (
              <div className="space-y-4">
                {insights?.map((insight) => (
                  <div key={insight.id} className="flex items-start gap-4 rounded-lg border p-4">
                    <div className="bg-primary/10 p-2 rounded-full">
                      <AlertCircle className="h-5 w-5 text-primary" />
                    </div>
                    <div className="flex-1">
                      <h4 className="text-sm font-semibold">{insight.title}</h4>
                      <p className="text-sm text-muted-foreground mt-1">{insight.description}</p>
                      <div className="mt-2 flex items-center gap-2 text-xs font-medium text-emerald-500">
                        <span>Impact: ${insight.businessImpact?.toLocaleString() ?? 0}</span>
                        <span>•</span>
                        <span>{insight.confidenceLevel}% Confidence</span>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Strategic Goals</CardTitle>
                <CardDescription>Progress on top-level business objectives</CardDescription>
              </div>
              <Link href="/dashboard/business-goals">
                <Button variant="ghost" size="sm">
                  Manage <ArrowRight className="ml-1 h-4 w-4" />
                </Button>
              </Link>
            </div>
          </CardHeader>
          <CardContent>
            {goalsLoading ? (
              <div className="space-y-4">
                <Skeleton className="h-12 w-full" />
                <Skeleton className="h-12 w-full" />
              </div>
            ) : (
              <div className="space-y-6">
                {goals?.slice(0, 4).map((goal) => {
                  const progress = (goal.currentValue / goal.targetValue) * 100;
                  return (
                    <div key={goal.id} className="space-y-2">
                      <div className="flex items-center justify-between text-sm">
                        <div className="flex items-center gap-2 font-medium">
                          <Target className="h-4 w-4 text-primary" />
                          {goal.title}
                        </div>
                        <span className="text-muted-foreground">
                          {goal.currentValue} / {goal.targetValue} {goal.unit}
                        </span>
                      </div>
                      <div className="h-2 w-full overflow-hidden rounded-full bg-secondary">
                        <div 
                          className="h-full bg-primary transition-all" 
                          style={{ width: `${Math.min(100, Math.max(0, progress))}%` }} 
                        />
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
