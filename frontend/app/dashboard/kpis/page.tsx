"use client";

import { useKpis, useBenchmarks } from "@/hooks/use-executive";
import { MetricCard } from "@/components/executive/metric-card";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { TrendingUp, TrendingDown, Minus } from "lucide-react";

export default function KpiWorkspacePage() {
  const { data: kpis, isLoading: kpisLoading } = useKpis();
  const { data: benchmarks, isLoading: benchmarksLoading } = useBenchmarks();

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">KPI Workspace</h1>
        <p className="text-muted-foreground">Monitor and analyze your Key Performance Indicators.</p>
      </div>

      <div className="grid gap-4 md:grid-cols-4">
        {kpisLoading ? (
          Array.from({ length: 4 }).map((_, i) => (
            <Skeleton key={i} className="h-32" />
          ))
        ) : (
          kpis?.map((kpi) => (
            <MetricCard 
              key={kpi.id}
              title={kpi.name}
              value={kpi.unit === "Percentage" ? `${kpi.currentValue.toFixed(1)}%` : kpi.currentValue.toLocaleString()}
              trend={kpi.currentValue > kpi.targetValue ? 5.0 : -2.0} // Mock trend calculation for UI demo
              description={`Target: ${kpi.targetValue.toLocaleString()}`}
            />
          ))
        )}
      </div>

      <div className="grid gap-6 md:grid-cols-1">
        <Card>
          <CardHeader>
            <CardTitle>Industry Benchmarks</CardTitle>
            <CardDescription>Compare your performance against industry standards.</CardDescription>
          </CardHeader>
          <CardContent>
            {benchmarksLoading ? (
              <div className="space-y-4">
                <Skeleton className="h-10 w-full" />
                <Skeleton className="h-10 w-full" />
                <Skeleton className="h-10 w-full" />
              </div>
            ) : (
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Metric</TableHead>
                    <TableHead>Your Value</TableHead>
                    <TableHead>Industry Average</TableHead>
                    <TableHead>Top Quartile</TableHead>
                    <TableHead>Status</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {benchmarks?.map((benchmark) => {
                    const myKpi = kpis?.find(k => k.name.toLowerCase() === benchmark.metricName.toLowerCase());
                    const myValue = myKpi?.currentValue || 0;
                    const isAboveAvg = myValue >= benchmark.industryAverage;
                    const isTopQuartile = myValue >= benchmark.topQuartile;

                    return (
                      <TableRow key={benchmark.id}>
                        <TableCell className="font-medium">{benchmark.metricName}</TableCell>
                        <TableCell>{myValue.toFixed(1)}</TableCell>
                        <TableCell>{benchmark.industryAverage.toFixed(1)}</TableCell>
                        <TableCell>{benchmark.topQuartile.toFixed(1)}</TableCell>
                        <TableCell>
                          {isTopQuartile ? (
                            <Badge className="bg-emerald-500">Top Quartile</Badge>
                          ) : isAboveAvg ? (
                            <Badge variant="outline" className="text-blue-500 border-blue-500">Above Average</Badge>
                          ) : (
                            <Badge variant="destructive">Below Average</Badge>
                          )}
                        </TableCell>
                      </TableRow>
                    );
                  })}
                  {(!benchmarks || benchmarks.length === 0) && (
                    <TableRow>
                      <TableCell colSpan={5} className="text-center text-muted-foreground">
                        No benchmarks available.
                      </TableCell>
                    </TableRow>
                  )}
                </TableBody>
              </Table>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
