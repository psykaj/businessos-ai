"use client";

import { useState } from "react";
import { useKPIs, useRecalculateKPIs } from "@/hooks/use-bi";
import { 
  BarChart3, 
  TrendingUp, 
  TrendingDown, 
  RefreshCw, 
  DollarSign, 
  Users, 
  Target, 
  Zap, 
  Sparkles, 
  Filter, 
  Clock, 
  CheckCircle2 
} from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";

export default function KPICenterPage() {
  const [selectedCategory, setSelectedCategory] = useState<string | undefined>(undefined);
  const [timeframe, setTimeframe] = useState("Monthly");

  const { data: kpis = [], isLoading, refetch } = useKPIs(selectedCategory);
  const { mutate: recalculate, isPending: isRecalculating } = useRecalculateKPIs();

  const categories = ["All", "Finance", "Sales", "Customers", "Marketing", "Operations", "AI"];

  const handleCategorySelect = (cat: string) => {
    setSelectedCategory(cat === "All" ? undefined : cat);
  };

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-[1600px] mx-auto min-h-screen">
      {/* Header Banner */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between border-b border-border/40 pb-6">
        <div>
          <div className="flex items-center gap-2.5">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary ring-1 ring-primary/20 shadow-inner">
              <BarChart3 className="h-5 w-5" />
            </div>
            <div>
              <h1 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
                KPI Performance Center
              </h1>
              <p className="text-sm text-muted-foreground mt-0.5">
                Real-time tracking for revenue growth, unit economics & operational metrics
              </p>
            </div>
          </div>
        </div>

        <div className="flex flex-wrap items-center gap-3">
          {/* Timeframe selector */}
          <div className="flex items-center rounded-xl border border-border/60 bg-muted/40 p-1">
            {["Daily", "Weekly", "Monthly", "Quarterly", "Yearly"].map((tf) => (
              <button
                key={tf}
                onClick={() => setTimeframe(tf)}
                className={`rounded-lg px-3 py-1.5 text-xs font-semibold transition-all ${
                  timeframe === tf
                    ? "bg-background text-foreground shadow-sm"
                    : "text-muted-foreground hover:text-foreground"
                }`}
              >
                {tf}
              </button>
            ))}
          </div>

          <Button
            onClick={() => recalculate()}
            disabled={isRecalculating}
            className="gap-2 rounded-xl text-xs font-semibold shadow-md"
          >
            <RefreshCw className={`h-3.5 w-3.5 ${isRecalculating ? "animate-spin" : ""}`} />
            {isRecalculating ? "Recalculating..." : "Recalculate KPIs"}
          </Button>
        </div>
      </div>

      {/* Category Pills */}
      <div className="flex flex-wrap items-center gap-2">
        {categories.map((cat) => {
          const isSelected = (cat === "All" && !selectedCategory) || selectedCategory === cat;
          return (
            <button
              key={cat}
              onClick={() => handleCategorySelect(cat)}
              className={`rounded-xl px-4 py-2 text-xs font-semibold transition-all border ${
                isSelected
                  ? "bg-primary text-primary-foreground border-primary shadow-sm"
                  : "bg-card/60 text-muted-foreground border-border/50 hover:bg-muted hover:text-foreground"
              }`}
            >
              {cat}
            </button>
          );
        })}
      </div>

      {/* KPI Grid */}
      {isLoading ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3, 4, 5, 6].map((n) => (
            <div key={n} className="h-44 rounded-2xl border border-border/40 bg-card/30 animate-pulse" />
          ))}
        </div>
      ) : (
        <div className="grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
          {kpis.map((kpi) => {
            const isUp = kpi.trend.toLowerCase().includes("up");
            const isDown = kpi.trend.toLowerCase().includes("down");

            return (
              <Card key={kpi.id} className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:shadow-md hover:border-border">
                <CardHeader className="flex flex-row items-center justify-between pb-2">
                  <Badge variant="outline" className="rounded-lg text-[10px] uppercase font-semibold text-muted-foreground border-border/60">
                    {kpi.category}
                  </Badge>
                  <span className="text-[11px] text-muted-foreground flex items-center gap-1 font-mono">
                    <Clock className="h-3 w-3" />
                    {timeframe}
                  </span>
                </CardHeader>
                <CardContent className="space-y-3">
                  <div>
                    <h3 className="text-sm font-semibold text-muted-foreground">{kpi.name}</h3>
                    <div className="text-2xl font-extrabold tracking-tight text-foreground mt-1">
                      {kpi.unit === "$" ? `$${kpi.currentValue.toLocaleString()}` : `${kpi.currentValue.toLocaleString()} ${kpi.unit !== "count" ? kpi.unit : ""}`}
                    </div>
                  </div>

                  <div className="flex items-center justify-between pt-2 border-t border-border/40 text-xs">
                    <div className="flex items-center gap-1.5 font-semibold">
                      {isUp ? (
                        <div className="flex items-center text-emerald-500 gap-1 bg-emerald-500/10 px-2 py-0.5 rounded-lg">
                          <TrendingUp className="h-3.5 w-3.5" />
                          <span>{kpi.trend}</span>
                        </div>
                      ) : isDown ? (
                        <div className="flex items-center text-rose-500 gap-1 bg-rose-500/10 px-2 py-0.5 rounded-lg">
                          <TrendingDown className="h-3.5 w-3.5" />
                          <span>{kpi.trend}</span>
                        </div>
                      ) : (
                        <div className="flex items-center text-blue-500 gap-1 bg-blue-500/10 px-2 py-0.5 rounded-lg">
                          <span>{kpi.trend}</span>
                        </div>
                      )}
                    </div>

                    {kpi.targetValue && (
                      <span className="text-muted-foreground">
                        Target: <span className="font-semibold text-foreground">{kpi.unit === "$" ? `$${kpi.targetValue.toLocaleString()}` : kpi.targetValue}</span>
                      </span>
                    )}
                  </div>
                </CardContent>
              </Card>
            );
          })}
        </div>
      )}
    </div>
  );
}
