"use client";

import React from "react";
import Link from "next/link";
import {
  useCustomerHealthSummary,
  useRetentionOverview,
  useSatisfactionSummary,
  useSuccessTasksPaged,
  useRecalculateAllHealth,
} from "@/hooks/use-customer-success";
import { HealthScoreBadge } from "@/components/customer-success/health-score-badge";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Progress } from "@/components/ui/progress";
import {
  HeartPulse,
  Award,
  Users,
  MessageSquare,
  CheckSquare,
  AlertTriangle,
  ArrowRight,
  RefreshCw,
  TrendingUp,
  ShieldAlert,
  Star,
  Zap,
} from "lucide-react";

export default function CustomerSuccessCenterPage() {
  const { data: healthSummary, isLoading: healthLoading } = useCustomerHealthSummary();
  const { data: retention, isLoading: retentionLoading } = useRetentionOverview();
  const { data: csat } = useSatisfactionSummary();
  const { data: tasksData } = useSuccessTasksPaged({ status: "Pending", pageSize: 5 });

  const recalculateHealth = useRecalculateAllHealth();

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* Header Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <HeartPulse className="h-8 w-8 text-purple-600" />
            Customer Success Center
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Proactively monitor account health, retain high-value customers, and boost repeat purchases.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button
            variant="outline"
            size="sm"
            onClick={() => recalculateHealth.mutate()}
            disabled={recalculateHealth.isPending}
            className="gap-2"
          >
            <RefreshCw className={`h-4 w-4 ${recalculateHealth.isPending ? "animate-spin" : ""}`} />
            Recalculate Health Scores
          </Button>
          <Link href="/dashboard/customer-success/tasks">
            <Button size="sm" className="gap-2 bg-gradient-to-r from-purple-600 to-indigo-600 text-white">
              <CheckSquare className="h-4 w-4" /> Success Tasks
            </Button>
          </Link>
        </div>
      </div>

      {/* Primary Retention Metrics Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <Card className="border-slate-200 dark:border-slate-800 bg-gradient-to-br from-emerald-500/5 via-background to-transparent">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-emerald-600 dark:text-emerald-400 uppercase tracking-wider">Retention Rate</span>
              <TrendingUp className="h-5 w-5 text-emerald-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {retention?.retentionRatePercentage ?? 94.5}%
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Active customer retention status</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800 bg-gradient-to-br from-purple-500/5 via-background to-transparent">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-purple-600 dark:text-purple-400 uppercase tracking-wider">Avg Health Score</span>
              <HeartPulse className="h-5 w-5 text-purple-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {healthSummary?.averageHealthScore ?? 82}/100
              </span>
            </div>
            <Progress value={healthSummary?.averageHealthScore ?? 82} className="h-1.5 mt-2 bg-slate-100 dark:bg-slate-800" />
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800 bg-gradient-to-br from-rose-500/5 via-background to-transparent">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-rose-600 dark:text-rose-400 uppercase tracking-wider">At-Risk Accounts</span>
              <ShieldAlert className="h-5 w-5 text-rose-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {healthSummary?.highRiskCount ?? 0}
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">
              Revenue at risk: ${retention?.revenueAtRisk.toLocaleString() ?? "0"}
            </p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800 bg-gradient-to-br from-amber-500/5 via-background to-transparent">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-amber-600 dark:text-amber-400 uppercase tracking-wider">CSAT Score</span>
              <Star className="h-5 w-5 text-amber-500 fill-amber-500/20" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {csat?.averageRating ?? 4.8} / 5.0
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">From {csat?.totalSubmissions ?? 0} customer reviews</p>
          </CardContent>
        </Card>
      </div>

      {/* Feature Quick Workspaces */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Link href="/dashboard/customer-health">
          <Card className="h-full border-slate-200 dark:border-slate-800 hover:border-purple-500/50 transition-all hover:shadow-lg group">
            <CardHeader className="pb-2">
              <div className="h-10 w-10 rounded-lg bg-purple-500/10 text-purple-600 flex items-center justify-center mb-2 group-hover:scale-110 transition-transform">
                <HeartPulse className="h-5 w-5" />
              </div>
              <CardTitle className="text-lg flex items-center justify-between">
                Customer Health
                <ArrowRight className="h-4 w-4 text-slate-400 group-hover:translate-x-1 transition-transform" />
              </CardTitle>
              <CardDescription>Monitor account health scores & churn risks</CardDescription>
            </CardHeader>
          </Card>
        </Link>

        <Link href="/dashboard/loyalty">
          <Card className="h-full border-slate-200 dark:border-slate-800 hover:border-indigo-500/50 transition-all hover:shadow-lg group">
            <CardHeader className="pb-2">
              <div className="h-10 w-10 rounded-lg bg-indigo-500/10 text-indigo-600 flex items-center justify-center mb-2 group-hover:scale-110 transition-transform">
                <Award className="h-5 w-5" />
              </div>
              <CardTitle className="text-lg flex items-center justify-between">
                Loyalty & Rewards
                <ArrowRight className="h-4 w-4 text-slate-400 group-hover:translate-x-1 transition-transform" />
              </CardTitle>
              <CardDescription>Reward repeat customers & manage points</CardDescription>
            </CardHeader>
          </Card>
        </Link>

        <Link href="/dashboard/referrals">
          <Card className="h-full border-slate-200 dark:border-slate-800 hover:border-emerald-500/50 transition-all hover:shadow-lg group">
            <CardHeader className="pb-2">
              <div className="h-10 w-10 rounded-lg bg-emerald-500/10 text-emerald-600 flex items-center justify-center mb-2 group-hover:scale-110 transition-transform">
                <Users className="h-5 w-5" />
              </div>
              <CardTitle className="text-lg flex items-center justify-between">
                Referral Program
                <ArrowRight className="h-4 w-4 text-slate-400 group-hover:translate-x-1 transition-transform" />
              </CardTitle>
              <CardDescription>Track referral codes & acquisition funnels</CardDescription>
            </CardHeader>
          </Card>
        </Link>

        <Link href="/dashboard/customer-feedback">
          <Card className="h-full border-slate-200 dark:border-slate-800 hover:border-amber-500/50 transition-all hover:shadow-lg group">
            <CardHeader className="pb-2">
              <div className="h-10 w-10 rounded-lg bg-amber-500/10 text-amber-600 flex items-center justify-center mb-2 group-hover:scale-110 transition-transform">
                <MessageSquare className="h-5 w-5" />
              </div>
              <CardTitle className="text-lg flex items-center justify-between">
                Feedback & CSAT
                <ArrowRight className="h-4 w-4 text-slate-400 group-hover:translate-x-1 transition-transform" />
              </CardTitle>
              <CardDescription>Analyze customer satisfaction ratings</CardDescription>
            </CardHeader>
          </Card>
        </Link>
      </div>

      {/* Retention Recommendations & Pending Success Tasks */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <Card className="lg:col-span-2 border-slate-200 dark:border-slate-800">
          <CardHeader>
            <CardTitle className="text-lg flex items-center gap-2">
              <Zap className="h-5 w-5 text-amber-500" />
              Recommended Retention Actions
            </CardTitle>
            <CardDescription>Automated insights to increase customer lifetime value</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            {retention?.recommendedActions.length ? (
              retention.recommendedActions.map((action, idx) => (
                <div key={idx} className="p-4 rounded-lg bg-slate-50 dark:bg-slate-900 border border-slate-200 dark:border-slate-800 flex items-start justify-between gap-4">
                  <div className="space-y-1">
                    <p className="text-sm font-semibold text-slate-900 dark:text-white">{action.description}</p>
                    <p className="text-xs text-slate-500">Affects {action.affectedCustomerCount} customers</p>
                  </div>
                  <Button size="sm" variant="outline" className="shrink-0 text-xs">
                    Execute Action
                  </Button>
                </div>
              ))
            ) : (
              <div className="p-6 text-center text-slate-500 text-sm">
                All accounts are in good standing! No immediate retention risks detected.
              </div>
            )}
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardHeader>
            <CardTitle className="text-lg flex items-center gap-2">
              <CheckSquare className="h-5 w-5 text-purple-600" />
              Pending Success Tasks
            </CardTitle>
            <CardDescription>High priority follow-ups</CardDescription>
          </CardHeader>
          <CardContent className="space-y-3">
            {tasksData?.items.length ? (
              tasksData.items.map((task) => (
                <div key={task.id} className="p-3 rounded-lg border border-slate-200 dark:border-slate-800 text-sm space-y-1">
                  <div className="flex items-center justify-between">
                    <span className="font-semibold text-slate-900 dark:text-white truncate max-w-[180px]">{task.title}</span>
                    <span className={`text-[10px] px-2 py-0.5 rounded font-bold uppercase ${task.priority === "Critical" ? "bg-rose-500/10 text-rose-600" : "bg-purple-500/10 text-purple-600"}`}>
                      {task.priority}
                    </span>
                  </div>
                  <p className="text-xs text-slate-500 truncate">{task.customerName}</p>
                </div>
              ))
            ) : (
              <p className="text-xs text-slate-500 text-center py-4">No pending success tasks.</p>
            )}
            <Link href="/dashboard/customer-success/tasks" className="block pt-2">
              <Button variant="ghost" size="sm" className="w-full text-xs text-purple-600">
                View All Tasks &rarr;
              </Button>
            </Link>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
