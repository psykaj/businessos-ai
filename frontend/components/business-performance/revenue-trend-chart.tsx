"use client";

import React, { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  ResponsiveContainer,
  AreaChart,
  Area,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  PieChart,
  Pie,
  Cell,
} from "recharts";
import { RevenueTrendItem, RevenueByRegion } from "@/lib/business-performance-service";
import { TrendingUp, BarChart3, Globe, Sparkles, Layers } from "lucide-react";

interface RevenueTrendChartProps {
  trends: RevenueTrendItem[];
  regions: RevenueByRegion[];
  title?: string;
  description?: string;
}

const REGION_COLORS = ["#10b981", "#3b82f6", "#f59e0b", "#8b5cf6"];

export function RevenueTrendChart({
  trends,
  regions,
  title = "Revenue, Profit & MRR Velocity",
  description = "Multi-dimensional financial telemetry with expansion decomposition & regional foundations.",
}: RevenueTrendChartProps) {
  const [activeTab, setActiveTab] = useState<"trend" | "mrr" | "regions">("trend");

  return (
    <Card className="border border-border bg-card shadow-sm transition-all hover:border-primary/40">
      <CardHeader className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between pb-4 border-b border-border/50">
        <div>
          <CardTitle className="text-lg font-bold tracking-tight flex items-center gap-2">
            <TrendingUp className="h-5 w-5 text-primary" />
            {title}
          </CardTitle>
          <CardDescription className="text-xs text-muted-foreground mt-1">
            {description}
          </CardDescription>
        </div>

        {/* View Mode Toggle Buttons */}
        <div className="flex flex-wrap gap-1 bg-muted p-1 rounded-lg">
          <Button
            variant={activeTab === "trend" ? "default" : "ghost"}
            size="sm"
            onClick={() => setActiveTab("trend")}
            className="h-8 text-xs font-semibold px-3"
          >
            <BarChart3 className="mr-1.5 h-3.5 w-3.5" />
            Revenue & Profit
          </Button>
          <Button
            variant={activeTab === "mrr" ? "default" : "ghost"}
            size="sm"
            onClick={() => setActiveTab("mrr")}
            className="h-8 text-xs font-semibold px-3"
          >
            <Layers className="mr-1.5 h-3.5 w-3.5" />
            MRR Decomposition
          </Button>
          <Button
            variant={activeTab === "regions" ? "default" : "ghost"}
            size="sm"
            onClick={() => setActiveTab("regions")}
            className="h-8 text-xs font-semibold px-3"
          >
            <Globe className="mr-1.5 h-3.5 w-3.5" />
            Global Regions
          </Button>
        </div>
      </CardHeader>

      <CardContent className="p-6">
        {activeTab === "trend" && (
          <div className="space-y-4">
            <div className="flex flex-wrap items-center justify-between gap-4 text-xs">
              <div className="flex items-center gap-4">
                <span className="flex items-center gap-1.5 font-medium text-foreground">
                  <span className="h-3 w-3 rounded-full bg-emerald-500 inline-block" /> Total Revenue ($104.5k)
                </span>
                <span className="flex items-center gap-1.5 font-medium text-foreground">
                  <span className="h-3 w-3 rounded-full bg-primary inline-block" /> Net Profit ($85.1k)
                </span>
                <span className="flex items-center gap-1.5 font-medium text-muted-foreground">
                  <span className="h-3 w-3 rounded-full bg-amber-500/60 inline-block" /> Target ($104.0k)
                </span>
              </div>
              <Badge variant="outline" className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 font-semibold">
                +26.8% Half-Year Expansion
              </Badge>
            </div>

            <div className="h-[340px] w-full pt-2">
              <ResponsiveContainer width="100%" height="100%">
                <AreaChart data={trends} margin={{ top: 10, right: 30, left: 10, bottom: 0 }}>
                  <defs>
                    <linearGradient id="colorRev" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor="#10b981" stopOpacity={0.3} />
                      <stop offset="95%" stopColor="#10b981" stopOpacity={0.0} />
                    </linearGradient>
                    <linearGradient id="colorProfit" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor="hsl(var(--primary))" stopOpacity={0.3} />
                      <stop offset="95%" stopColor="hsl(var(--primary))" stopOpacity={0.0} />
                    </linearGradient>
                  </defs>
                  <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--muted-foreground)/0.15)" />
                  <XAxis dataKey="period" axisLine={false} tickLine={false} tick={{ fontSize: 12, fill: "hsl(var(--muted-foreground))" }} />
                  <YAxis axisLine={false} tickLine={false} tickFormatter={(v) => `$${v / 1000}k`} tick={{ fontSize: 12, fill: "hsl(var(--muted-foreground))" }} />
                  <Tooltip
                    contentStyle={{ backgroundColor: "hsl(var(--card))", borderColor: "hsl(var(--border))", borderRadius: "8px", color: "hsl(var(--foreground))" }}
                    formatter={(val: any) => [`$${Number(val ?? 0).toLocaleString()}`, "Amount"]}
                  />
                  <Legend verticalAlign="top" height={36} />
                  <Area type="monotone" name="Total Revenue" dataKey="revenue" stroke="#10b981" strokeWidth={3} fillOpacity={1} fill="url(#colorRev)" />
                  <Area type="monotone" name="Net Profit" dataKey="profit" stroke="hsl(var(--primary))" strokeWidth={2.5} fillOpacity={1} fill="url(#colorProfit)" />
                  <Area type="monotone" name="Target Revenue" dataKey="targetRevenue" stroke="#f59e0b" strokeDasharray="5 5" fill="transparent" strokeWidth={1.5} />
                </AreaChart>
              </ResponsiveContainer>
            </div>
          </div>
        )}

        {activeTab === "mrr" && (
          <div className="space-y-4">
            <div className="flex flex-wrap items-center justify-between gap-4 text-xs">
              <span className="text-muted-foreground">
                Decomposing net subscription momentum into <strong className="text-emerald-500">New + Expansion</strong> vs <strong className="text-rose-500">Contraction + Churn</strong>.
              </span>
              <Badge variant="outline" className="bg-primary/10 text-primary border-primary/30 font-semibold">
                Net Revenue Retention: 124.8%
              </Badge>
            </div>

            <div className="h-[340px] w-full pt-2">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={trends} margin={{ top: 10, right: 30, left: 10, bottom: 0 }}>
                  <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--muted-foreground)/0.15)" />
                  <XAxis dataKey="period" axisLine={false} tickLine={false} tick={{ fontSize: 12, fill: "hsl(var(--muted-foreground))" }} />
                  <YAxis axisLine={false} tickLine={false} tickFormatter={(v) => `$${v / 1000}k`} tick={{ fontSize: 12, fill: "hsl(var(--muted-foreground))" }} />
                  <Tooltip
                    contentStyle={{ backgroundColor: "hsl(var(--card))", borderColor: "hsl(var(--border))", borderRadius: "8px" }}
                    formatter={(val: any) => [`$${Number(val ?? 0).toLocaleString()}`, "MRR Component"]}
                  />
                  <Legend verticalAlign="top" height={36} />
                  <Bar dataKey="newMrr" name="New MRR" stackId="a" fill="#10b981" radius={[4, 4, 0, 0]} />
                  <Bar dataKey="expansionMrr" name="Expansion MRR" stackId="a" fill="hsl(var(--primary))" radius={[4, 4, 0, 0]} />
                  <Bar dataKey="contractionMrr" name="Contraction MRR" stackId="b" fill="#f97316" radius={[4, 4, 0, 0]} />
                  <Bar dataKey="churnedMrr" name="Churned MRR" stackId="b" fill="#ef4444" radius={[4, 4, 0, 0]} />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>
        )}

        {activeTab === "regions" && (
          <div className="grid grid-cols-1 md:grid-cols-12 gap-6 items-center">
            <div className="md:col-span-6 h-[320px]">
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={regions}
                    dataKey="revenue"
                    nameKey="region"
                    cx="50%"
                    cy="50%"
                    outerRadius={110}
                    innerRadius={65}
                    paddingAngle={3}
                    label={({ percent }: { percent?: number }) => `${((percent ?? 0) * 100).toFixed(0)}%`}
                  >
                    {regions.map((_, index) => (
                      <Cell key={`cell-${index}`} fill={REGION_COLORS[index % REGION_COLORS.length]} stroke="hsl(var(--card))" strokeWidth={2} />
                    ))}
                  </Pie>
                  <Tooltip
                    contentStyle={{ backgroundColor: "hsl(var(--card))", borderColor: "hsl(var(--border))", borderRadius: "8px" }}
                    formatter={(val: any) => [`$${Number(val ?? 0).toLocaleString()}`, "Regional ARR"]}
                  />
                </PieChart>
              </ResponsiveContainer>
            </div>

            <div className="md:col-span-6 space-y-3">
              <h4 className="text-sm font-semibold text-foreground border-b border-border pb-2 flex items-center justify-between">
                <span>Geographic ARR Foundation</span>
                <span className="text-xs text-muted-foreground font-normal">YoY Growth</span>
              </h4>
              {regions.map((reg, idx) => (
                <div key={reg.region} className="flex items-center justify-between p-2 rounded-lg bg-muted/40 border border-border/40 hover:bg-muted/80 transition-colors">
                  <div className="flex items-center gap-3">
                    <span className="h-3 w-3 rounded-full" style={{ backgroundColor: REGION_COLORS[idx % REGION_COLORS.length] }} />
                    <div>
                      <div className="font-medium text-sm text-foreground">{reg.region}</div>
                      <div className="text-xs text-muted-foreground">{reg.activeCustomers} active enterprise logos</div>
                    </div>
                  </div>
                  <div className="text-right">
                    <div className="font-bold text-sm text-foreground">${reg.revenue.toLocaleString()}</div>
                    <span className="inline-flex text-xs font-semibold text-emerald-600 dark:text-emerald-400">
                      +{reg.growthYoY}% YoY
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
