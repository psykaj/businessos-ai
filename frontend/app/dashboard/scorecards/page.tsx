"use client";

import { useScorecards } from "@/hooks/use-executive";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Users, TrendingUp, AlertTriangle } from "lucide-react";

export default function ScorecardsPage() {
  const { data: scorecards, isLoading } = useScorecards();

  const getScoreColor = (score: number) => {
    if (score >= 90) return "text-emerald-500 bg-emerald-500/10";
    if (score >= 75) return "text-blue-500 bg-blue-500/10";
    if (score >= 60) return "text-amber-500 bg-amber-500/10";
    return "text-red-500 bg-red-500/10";
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Department Scorecards</h1>
        <p className="text-muted-foreground">Evaluate performance across departments and teams.</p>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        {isLoading ? (
          Array.from({ length: 4 }).map((_, i) => (
            <Skeleton key={i} className="h-64" />
          ))
        ) : (
          scorecards?.map((scorecard) => (
            <Card key={scorecard.id}>
              <CardHeader className="flex flex-row justify-between items-start pb-2">
                <div>
                  <CardTitle className="text-xl flex items-center gap-2">
                    <Users className="h-5 w-5 text-muted-foreground" />
                    {scorecard.title}
                  </CardTitle>
                  <CardDescription className="mt-1">
                    Period: {scorecard.evaluationPeriod}
                  </CardDescription>
                </div>
                <div className={`flex items-center justify-center h-16 w-16 rounded-full font-bold text-2xl ${getScoreColor(scorecard.score)}`}>
                  {scorecard.score}
                </div>
              </CardHeader>
              <CardContent className="space-y-4 pt-4">
                <div className="space-y-2">
                  <h4 className="text-sm font-semibold flex items-center gap-2">
                    <TrendingUp className="h-4 w-4 text-emerald-500" /> Strengths
                  </h4>
                  <p className="text-sm text-muted-foreground bg-emerald-500/5 p-3 rounded-md border border-emerald-500/10">
                    {scorecard.strengths}
                  </p>
                </div>
                <div className="space-y-2">
                  <h4 className="text-sm font-semibold flex items-center gap-2">
                    <AlertTriangle className="h-4 w-4 text-amber-500" /> Areas for Improvement
                  </h4>
                  <p className="text-sm text-muted-foreground bg-amber-500/5 p-3 rounded-md border border-amber-500/10">
                    {scorecard.areasForImprovement}
                  </p>
                </div>
              </CardContent>
            </Card>
          ))
        )}
        {(!scorecards || scorecards.length === 0) && !isLoading && (
          <div className="col-span-full text-center py-12 text-muted-foreground">
            No scorecards found for this period.
          </div>
        )}
      </div>
    </div>
  );
}
