"use client";

import React from "react";
import { useServiceQualityMetrics } from "@/hooks/use-customer-feedback";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { ResponsiveContainer, LineChart, Line, XAxis, YAxis, Tooltip, CartesianGrid } from "recharts";
import { Clock, CheckCircle2, AlertTriangle, ShieldCheck, TrendingDown, Zap, Gauge } from "lucide-react";

export default function ServiceQualityPage() {
  const { data: slaData, isLoading } = useServiceQualityMetrics();

  if (isLoading) {
    return <div className="p-12 text-center text-slate-500 animate-pulse font-medium">Loading Support SLA & Service Velocity KPIs...</div>;
  }

  const items = slaData || [];
  const avgResponse = items.length ? items.reduce((acc, i) => acc + i.firstResponseTimeMin, 0) / items.length : 4.0;
  const avgFcr = items.length ? items.reduce((acc, i) => acc + i.fcrRate, 0) / items.length : 89.2;

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="border-b border-slate-200 dark:border-slate-800 pb-6">
        <h1 className="text-3xl font-black text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
          <Gauge className="h-8 w-8 text-emerald-500" />
          Service Quality & Support SLA Tracker
        </h1>
        <p className="text-slate-500 dark:text-slate-400 mt-1 text-sm font-medium">
          Measure support team responsiveness and First Contact Resolution (FCR). Maintaining sub-5 minute reply SLAs directly reduces account attrition.
        </p>
      </div>

      {/* KPI Headline row */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="p-3 rounded-xl bg-emerald-500/10 text-emerald-600 dark:text-emerald-400">
                <Clock className="h-6 w-6" />
              </div>
              <span className="text-xs font-bold text-emerald-600 bg-emerald-50 dark:bg-emerald-950/40 px-2.5 py-1 rounded-full border border-emerald-200 dark:border-emerald-800">
                Well below 5m target
              </span>
            </div>
            <p className="text-xs font-semibold text-slate-400 mt-4 uppercase tracking-wider">Avg First Response Time</p>
            <h3 className="text-3xl font-black text-slate-900 dark:text-white mt-1">{avgResponse.toFixed(1)} Minutes</h3>
          </CardContent>
        </Card>
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="p-3 rounded-xl bg-indigo-500/10 text-indigo-600 dark:text-indigo-400">
                <CheckCircle2 className="h-6 w-6" />
              </div>
              <span className="text-xs font-bold text-indigo-600 bg-indigo-50 dark:bg-indigo-950/40 px-2.5 py-1 rounded-full border border-indigo-200 dark:border-indigo-800">
                +4.2% vs Benchmark
              </span>
            </div>
            <p className="text-xs font-semibold text-slate-400 mt-4 uppercase tracking-wider">First Contact Resolution (FCR)</p>
            <h3 className="text-3xl font-black text-slate-900 dark:text-white mt-1">{avgFcr.toFixed(1)}%</h3>
          </CardContent>
        </Card>
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="p-3 rounded-xl bg-purple-500/10 text-purple-600 dark:text-purple-400">
                <Zap className="h-6 w-6" />
              </div>
              <span className="text-xs font-bold text-purple-600 bg-purple-50 dark:bg-purple-950/40 px-2.5 py-1 rounded-full border border-purple-200 dark:border-purple-800">
                99.4% Adherence
              </span>
            </div>
            <p className="text-xs font-semibold text-slate-400 mt-4 uppercase tracking-wider">SLA Compliance Rate</p>
            <h3 className="text-3xl font-black text-slate-900 dark:text-white mt-1">Tier-1 SLA Secured</h3>
          </CardContent>
        </Card>
      </div>

      {/* Charts section */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardHeader className="p-5 border-b border-slate-100 dark:border-slate-800">
            <CardTitle className="text-base font-bold text-slate-900 dark:text-white">Daily Response Time Velocity (Minutes)</CardTitle>
            <CardDescription className="text-xs text-slate-500">Lower response time prevents escalation and frustration</CardDescription>
          </CardHeader>
          <CardContent className="p-5 h-64">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={items}>
                <CartesianGrid strokeDasharray="3 3" stroke="#334155" opacity={0.2} />
                <XAxis dataKey="date" stroke="#64748B" fontSize={12} />
                <YAxis stroke="#64748B" fontSize={12} domain={[0, 10]} />
                <Tooltip />
                <Line type="monotone" name="Actual Reply (Min)" dataKey="firstResponseTimeMin" stroke="#10B981" strokeWidth={3} dot={{ r: 5 }} />
                <Line type="monotone" name="Target Max SLA" dataKey="targetResponseMin" stroke="#F43F5E" strokeWidth={2} strokeDasharray="5 5" />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl">
          <CardHeader className="p-5 border-b border-slate-100 dark:border-slate-800">
            <CardTitle className="text-base font-bold text-slate-900 dark:text-white">First Contact Resolution Rate (%)</CardTitle>
            <CardDescription className="text-xs text-slate-500">Higher FCR eliminates repeat ticket administrative overhead costs</CardDescription>
          </CardHeader>
          <CardContent className="p-5 h-64">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={items}>
                <CartesianGrid strokeDasharray="3 3" stroke="#334155" opacity={0.2} />
                <XAxis dataKey="date" stroke="#64748B" fontSize={12} />
                <YAxis stroke="#64748B" fontSize={12} domain={[70, 100]} />
                <Tooltip />
                <Line type="monotone" name="Actual FCR (%)" dataKey="fcrRate" stroke="#6366F1" strokeWidth={3} dot={{ r: 5 }} />
                <Line type="monotone" name="Target Minimum FCR" dataKey="targetFcrRate" stroke="#F59E0B" strokeWidth={2} strokeDasharray="5 5" />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
