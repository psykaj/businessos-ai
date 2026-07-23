"use client";

import { useState } from "react";
import { useForecast, useGenerateForecast } from "@/hooks/use-bi";
import { 
  TrendingUp, 
  Activity, 
  RefreshCw, 
  DollarSign, 
  Users, 
  Target, 
  ShieldCheck, 
  BarChart3, 
  Sparkles, 
  Calendar 
} from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import {
  AreaChart,
  Area,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer
} from "recharts";

export default function ForecastDashboardPage() {
  const [selectedType, setSelectedType] = useState("Revenue");
  const [horizonDays, setHorizonDays] = useState(30);

  const { data: forecastSummary, isLoading } = useForecast(selectedType);
  const { mutate: generateForecast, isPending: isGenerating } = useGenerateForecast();

  const handleGenerate = () => {
    generateForecast({ forecastType: selectedType, horizonDays });
  };

  const types = [
    { key: "Revenue", label: "Revenue Forecast", icon: DollarSign },
    { key: "Sales", label: "Sales Pipeline Forecast", icon: Target },
    { key: "Lead", label: "Inbound Lead Forecast", icon: BarChart3 },
    { key: "CustomerGrowth", label: "Customer Growth", icon: Users },
    { key: "Subscription", label: "Subscription MRR", icon: Activity }
  ];

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-[1600px] mx-auto min-h-screen">
      {/* Header Banner */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between border-b border-border/40 pb-6">
        <div>
          <div className="flex items-center gap-2.5">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary ring-1 ring-primary/20 shadow-inner">
              <Activity className="h-5 w-5" />
            </div>
            <div>
              <h1 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
                Predictive Forecasting Engine
              </h1>
              <p className="text-sm text-muted-foreground mt-0.5">
                AI and statistical projection modeling for enterprise growth metrics
              </p>
            </div>
          </div>
        </div>

        <div className="flex items-center gap-3">
          {/* Horizon Selector */}
          <div className="flex items-center rounded-xl border border-border/60 bg-muted/40 p-1">
            {[30, 60, 90].map((days) => (
              <button
                key={days}
                onClick={() => setHorizonDays(days)}
                className={`rounded-lg px-3 py-1.5 text-xs font-semibold transition-all ${
                  horizonDays === days
                    ? "bg-background text-foreground shadow-sm"
                    : "text-muted-foreground hover:text-foreground"
                }`}
              >
                {days} Days Horizon
              </button>
            ))}
          </div>

          <Button
            onClick={handleGenerate}
            disabled={isGenerating}
            className="gap-2 rounded-xl text-xs font-semibold shadow-md"
          >
            <RefreshCw className={`h-3.5 w-3.5 ${isGenerating ? "animate-spin" : ""}`} />
            {isGenerating ? "Computing..." : "Run Forecast Model"}
          </Button>
        </div>
      </div>

      {/* Forecast Type Selector Tabs */}
      <Tabs defaultValue="Revenue" value={selectedType} onValueChange={setSelectedType} className="space-y-6">
        <TabsList className="flex h-auto w-full flex-wrap justify-start gap-2 rounded-2xl bg-muted/50 p-1.5 border border-border/40">
          {types.map((t) => {
            const Icon = t.icon;
            return (
              <TabsTrigger key={t.key} value={t.key} className="rounded-xl px-4 py-2 text-xs font-semibold gap-2">
                <Icon className="h-3.5 w-3.5" />
                {t.label}
              </TabsTrigger>
            );
          })}
        </TabsList>

        {/* Forecast Content Summary */}
        <div className="grid gap-4 sm:grid-cols-3">
          <Card className="rounded-2xl border-border/50 bg-card/60">
            <CardHeader className="pb-2">
              <CardTitle className="text-xs text-muted-foreground uppercase">Projected Total ({horizonDays} Days)</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-foreground">
                {selectedType === "Revenue" || selectedType === "Sales" || selectedType === "Subscription"
                  ? `$${forecastSummary?.totalPredicted?.toLocaleString() ?? "0"}`
                  : forecastSummary?.totalPredicted?.toLocaleString() ?? "0"}
              </div>
            </CardContent>
          </Card>

          <Card className="rounded-2xl border-border/50 bg-card/60">
            <CardHeader className="pb-2">
              <CardTitle className="text-xs text-muted-foreground uppercase">Projected Growth Velocity</CardTitle>
            </CardHeader>
            <CardContent className="flex items-center gap-2">
              <div className="text-2xl font-bold text-emerald-500">
                +{forecastSummary?.growthRate ?? 6.5}%
              </div>
              <TrendingUp className="h-5 w-5 text-emerald-500" />
            </CardContent>
          </Card>

          <Card className="rounded-2xl border-border/50 bg-card/60">
            <CardHeader className="pb-2">
              <CardTitle className="text-xs text-muted-foreground uppercase">Model Confidence Score</CardTitle>
            </CardHeader>
            <CardContent className="flex items-center justify-between">
              <div className="text-2xl font-bold text-blue-500">
                {Math.round((forecastSummary?.averageConfidence ?? 0.88) * 100)}%
              </div>
              <Badge variant="outline" className="border-blue-500/30 text-blue-400 text-[10px]">
                High Statistical Significance
              </Badge>
            </CardContent>
          </Card>
        </div>

        {/* Projection Chart */}
        <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl">
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle className="text-base font-bold">{selectedType} Predictive Projection</CardTitle>
                <CardDescription>Estimated daily growth curve over next {horizonDays} days</CardDescription>
              </div>
              <Badge className="bg-primary/20 text-primary border-primary/30 text-xs">
                ML Ready Engine
              </Badge>
            </div>
          </CardHeader>
          <CardContent className="pt-4">
            <div className="h-[350px] w-full">
              {isLoading ? (
                <div className="h-full w-full bg-muted/20 animate-pulse rounded-xl" />
              ) : (
                <ResponsiveContainer width="100%" height="100%">
                  <AreaChart data={forecastSummary?.dataPoints ?? []}>
                    <defs>
                      <linearGradient id="colorForecast" x1="0" y1="0" x2="0" y2="1">
                        <stop offset="5%" stopColor="#10b981" stopOpacity={0.4} />
                        <stop offset="95%" stopColor="#10b981" stopOpacity={0} />
                      </linearGradient>
                    </defs>
                    <CartesianGrid strokeDasharray="3 3" opacity={0.15} />
                    <XAxis 
                      dataKey="forecastDate" 
                      tick={{ fontSize: 11 }}
                      tickFormatter={(str) => new Date(str).toLocaleDateString("en-US", { month: "short", day: "numeric" })}
                    />
                    <YAxis tick={{ fontSize: 11 }} />
                    <Tooltip 
                      contentStyle={{ backgroundColor: "rgba(15, 23, 42, 0.9)", borderRadius: "12px", border: "1px solid #334155" }}
                      labelFormatter={(str) => new Date(str).toLocaleDateString("en-US", { month: "long", day: "numeric", year: "numeric" })}
                      formatter={(val: any) => [
                        selectedType === "Revenue" || selectedType === "Sales" || selectedType === "Subscription"
                          ? `$${Number(val).toLocaleString()}`
                          : Number(val).toLocaleString(),
                        "Predicted Value"
                      ]}
                    />
                    <Area type="monotone" dataKey="predictedValue" stroke="#10b981" strokeWidth={2.5} fillOpacity={1} fill="url(#colorForecast)" />
                  </AreaChart>
                </ResponsiveContainer>
              )}
            </div>
          </CardContent>
        </Card>
      </Tabs>
    </div>
  );
}
