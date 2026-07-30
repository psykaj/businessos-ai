"use client";

import { useBusinessGoals } from "@/hooks/use-executive";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Target, Calendar, BarChart2 } from "lucide-react";
import { format } from "date-fns";

export default function BusinessGoalsPage() {
  const { data: goals, isLoading } = useBusinessGoals();

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case "on track": return "bg-emerald-500/10 text-emerald-500 border-emerald-500/20";
      case "at risk": return "bg-amber-500/10 text-amber-500 border-amber-500/20";
      case "behind": return "bg-red-500/10 text-red-500 border-red-500/20";
      case "completed": return "bg-blue-500/10 text-blue-500 border-blue-500/20";
      default: return "bg-slate-500/10 text-slate-500 border-slate-500/20";
    }
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Business Goals</h1>
        <p className="text-muted-foreground">Track top-level company objectives and key results.</p>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        {isLoading ? (
          Array.from({ length: 3 }).map((_, i) => (
            <Skeleton key={i} className="h-48" />
          ))
        ) : (
          goals?.map((goal) => {
            const progress = (goal.currentValue / goal.targetValue) * 100;
            return (
              <Card key={goal.id} className="flex flex-col">
                <CardHeader>
                  <div className="flex justify-between items-start">
                    <div className="space-y-1">
                      <CardTitle className="text-lg flex items-center gap-2">
                        <Target className="h-5 w-5 text-primary" />
                        {goal.title}
                      </CardTitle>
                      <CardDescription>{goal.category}</CardDescription>
                    </div>
                    <Badge variant="outline" className={getStatusColor(goal.status)}>
                      {goal.status}
                    </Badge>
                  </div>
                </CardHeader>
                <CardContent className="flex-1 flex flex-col justify-between">
                  <div className="space-y-4">
                    <p className="text-sm text-muted-foreground line-clamp-2">
                      {goal.description}
                    </p>
                    <div className="space-y-2">
                      <div className="flex justify-between text-sm">
                        <span className="font-medium text-foreground">
                          {goal.currentValue.toLocaleString()} / {goal.targetValue.toLocaleString()} {goal.unit}
                        </span>
                        <span className="text-muted-foreground">{progress.toFixed(1)}%</span>
                      </div>
                      <div className="h-2 w-full overflow-hidden rounded-full bg-secondary">
                        <div 
                          className="h-full bg-primary transition-all" 
                          style={{ width: `${Math.min(100, Math.max(0, progress))}%` }} 
                        />
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center gap-2 text-xs text-muted-foreground mt-6 pt-4 border-t">
                    <Calendar className="h-4 w-4" />
                    Deadline: {format(new Date(goal.deadline), "MMM dd, yyyy")}
                  </div>
                </CardContent>
              </Card>
            );
          })
        )}
        {(!goals || goals.length === 0) && !isLoading && (
          <div className="col-span-full text-center py-12 text-muted-foreground">
            No business goals found. Add goals to track your strategy.
          </div>
        )}
      </div>
    </div>
  );
}
