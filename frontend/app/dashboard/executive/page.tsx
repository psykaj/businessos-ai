"use client";

import { useKpis, useBusinessGoals } from "@/hooks/use-executive";
import { MetricCard } from "@/components/executive/metric-card";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Target, ArrowRight } from "lucide-react";
import Link from "next/link";
import { Skeleton } from "@/components/ui/skeleton";
import { BusinessBriefing } from "@/components/executive/business-briefing";

export default function ExecutiveDashboardPage() {
  const { data: kpis, isLoading: kpisLoading } = useKpis();
  const { data: goals, isLoading: goalsLoading } = useBusinessGoals();

  const revenueKpi = kpis?.find(k => k.name.toLowerCase().includes("revenue"));
  const leadKpi = kpis?.find(k => k.name.toLowerCase().includes("lead"));

  return (
    <div className="space-y-12">
      {/* AI Business Briefing Section */}
      <BusinessBriefing />

      {/* Legacy KPIs and Strategic Goals Section */}
      <div className="pt-8 border-t space-y-6">
        <div className="flex flex-col gap-2 md:flex-row md:items-center md:justify-between">
          <div>
            <h2 className="text-2xl font-bold tracking-tight">Executive Operations</h2>
            <p className="text-muted-foreground">Strategic goals and key performance indicators.</p>
          </div>
          <div className="flex gap-2">
            <Link href="/dashboard/kpis">
              <Button variant="outline">KPI Workspace</Button>
            </Link>
          </div>
        </div>

        <div className="grid gap-4 md:grid-cols-3">
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

        <div className="grid gap-6 md:grid-cols-2">
          <Card className="md:col-span-2">
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
                <div className="grid md:grid-cols-2 gap-6">
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
    </div>
  );
}
