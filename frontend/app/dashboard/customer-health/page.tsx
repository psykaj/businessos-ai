"use client";

import React, { useState } from "react";
import Link from "next/link";
import { useCustomerHealthPaged, useCustomerHealthSummary, useRecalculateAllHealth } from "@/hooks/use-customer-success";
import { HealthScoreBadge } from "@/components/customer-success/health-score-badge";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { HeartPulse, Search, RefreshCw, Filter, ShieldAlert, CheckCircle2, AlertTriangle, Eye } from "lucide-react";

export default function CustomerHealthDashboardPage() {
  const [riskFilter, setRiskFilter] = useState<string>("");
  const [search, setSearch] = useState<string>("");
  const [page, setPage] = useState<number>(1);

  const { data: summary, isLoading: summaryLoading } = useCustomerHealthSummary();
  const { data: healthData, isLoading: listLoading } = useCustomerHealthPaged({
    riskLevel: riskFilter || undefined,
    search: search || undefined,
    page,
    pageSize: 15,
  });

  const recalculateMutation = useRecalculateAllHealth();

  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      {/* Top Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <HeartPulse className="h-8 w-8 text-purple-600" />
            Customer Health Dashboard
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Automated churn risk calculation based on purchase frequency, support tickets, LTV, and CSAT.
          </p>
        </div>
        <Button
          onClick={() => recalculateMutation.mutate()}
          disabled={recalculateMutation.isPending}
          className="gap-2 bg-purple-600 hover:bg-purple-700 text-white"
        >
          <RefreshCw className={`h-4 w-4 ${recalculateMutation.isPending ? "animate-spin" : ""}`} />
          Recalculate All Scores
        </Button>
      </div>

      {/* Summary KPI Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <Card
          className={`cursor-pointer transition-all border-slate-200 dark:border-slate-800 ${riskFilter === "Healthy" ? "ring-2 ring-emerald-500" : ""}`}
          onClick={() => setRiskFilter(riskFilter === "Healthy" ? "" : "Healthy")}
        >
          <CardContent className="p-5 flex items-center justify-between">
            <div>
              <p className="text-xs font-semibold text-emerald-600 dark:text-emerald-400 uppercase tracking-wider">Healthy Accounts</p>
              <p className="text-2xl font-bold text-slate-900 dark:text-white font-mono mt-1">{summary?.healthyCount ?? 0}</p>
            </div>
            <div className="h-10 w-10 rounded-full bg-emerald-500/10 text-emerald-600 flex items-center justify-center">
              <CheckCircle2 className="h-5 w-5" />
            </div>
          </CardContent>
        </Card>

        <Card
          className={`cursor-pointer transition-all border-slate-200 dark:border-slate-800 ${riskFilter === "Stable" ? "ring-2 ring-blue-500" : ""}`}
          onClick={() => setRiskFilter(riskFilter === "Stable" ? "" : "Stable")}
        >
          <CardContent className="p-5 flex items-center justify-between">
            <div>
              <p className="text-xs font-semibold text-blue-600 dark:text-blue-400 uppercase tracking-wider">Stable Accounts</p>
              <p className="text-2xl font-bold text-slate-900 dark:text-white font-mono mt-1">{summary?.stableCount ?? 0}</p>
            </div>
            <div className="h-10 w-10 rounded-full bg-blue-500/10 text-blue-600 flex items-center justify-center">
              <HeartPulse className="h-5 w-5" />
            </div>
          </CardContent>
        </Card>

        <Card
          className={`cursor-pointer transition-all border-slate-200 dark:border-slate-800 ${riskFilter === "Needs Attention" ? "ring-2 ring-amber-500" : ""}`}
          onClick={() => setRiskFilter(riskFilter === "Needs Attention" ? "" : "Needs Attention")}
        >
          <CardContent className="p-5 flex items-center justify-between">
            <div>
              <p className="text-xs font-semibold text-amber-600 dark:text-amber-400 uppercase tracking-wider">Needs Attention</p>
              <p className="text-2xl font-bold text-slate-900 dark:text-white font-mono mt-1">{summary?.needsAttentionCount ?? 0}</p>
            </div>
            <div className="h-10 w-10 rounded-full bg-amber-500/10 text-amber-600 flex items-center justify-center">
              <AlertTriangle className="h-5 w-5" />
            </div>
          </CardContent>
        </Card>

        <Card
          className={`cursor-pointer transition-all border-slate-200 dark:border-slate-800 ${riskFilter === "High Risk" ? "ring-2 ring-rose-500" : ""}`}
          onClick={() => setRiskFilter(riskFilter === "High Risk" ? "" : "High Risk")}
        >
          <CardContent className="p-5 flex items-center justify-between">
            <div>
              <p className="text-xs font-semibold text-rose-600 dark:text-rose-400 uppercase tracking-wider">High Risk (Churn Risk)</p>
              <p className="text-2xl font-bold text-slate-900 dark:text-white font-mono mt-1">{summary?.highRiskCount ?? 0}</p>
            </div>
            <div className="h-10 w-10 rounded-full bg-rose-500/10 text-rose-600 flex items-center justify-center">
              <ShieldAlert className="h-5 w-5" />
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Filter & Search Toolbar */}
      <Card className="border-slate-200 dark:border-slate-800">
        <CardContent className="p-4 flex flex-col sm:flex-row items-center justify-between gap-4">
          <div className="relative w-full sm:w-80">
            <Search className="absolute left-3 top-2.5 h-4 w-4 text-slate-400" />
            <Input
              placeholder="Search customer health..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="pl-9 text-sm"
            />
          </div>

          <div className="flex items-center gap-2 w-full sm:w-auto">
            <Button
              variant={riskFilter === "" ? "default" : "outline"}
              size="sm"
              onClick={() => setRiskFilter("")}
              className="text-xs"
            >
              All Accounts
            </Button>
            {["Healthy", "Stable", "Needs Attention", "High Risk"].map((rf) => (
              <Button
                key={rf}
                variant={riskFilter === rf ? "default" : "outline"}
                size="sm"
                onClick={() => setRiskFilter(rf)}
                className="text-xs hidden md:inline-flex"
              >
                {rf}
              </Button>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Health Score Table */}
      <Card className="border-slate-200 dark:border-slate-800">
        <CardHeader className="pb-3">
          <CardTitle className="text-base">Customer Account Health Scores</CardTitle>
          <CardDescription>Live health index, interaction recency, and risk classification</CardDescription>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="border-slate-200 dark:border-slate-800">
                <TableHead>Customer</TableHead>
                <TableHead>Health Score</TableHead>
                <TableHead>Risk Level</TableHead>
                <TableHead>Lifetime Value</TableHead>
                <TableHead>Tickets</TableHead>
                <TableHead>CSAT Rating</TableHead>
                <TableHead>Last Interaction</TableHead>
                <TableHead className="text-right">Action</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {healthData?.items.length ? (
                healthData.items.map((h) => (
                  <TableRow key={h.id} className="border-slate-100 dark:border-slate-800/60 hover:bg-slate-50 dark:hover:bg-slate-900/50">
                    <TableCell className="font-semibold text-slate-900 dark:text-white">
                      {h.customerName || "Customer Account"}
                    </TableCell>
                    <TableCell>
                      <span className="font-bold font-mono text-sm">{h.healthScore}/100</span>
                    </TableCell>
                    <TableCell>
                      <HealthScoreBadge score={h.healthScore} riskLevel={h.riskLevel} showScore={false} />
                    </TableCell>
                    <TableCell className="font-mono font-medium">${h.lifetimeValue.toLocaleString()}</TableCell>
                    <TableCell>
                      <span className={h.supportTicketCount > 2 ? "text-rose-500 font-bold" : "text-slate-600 dark:text-slate-400"}>
                        {h.supportTicketCount}
                      </span>
                    </TableCell>
                    <TableCell className="font-medium text-amber-500">
                      {h.satisfactionRating ? `${h.satisfactionRating} / 5` : "N/A"}
                    </TableCell>
                    <TableCell className="text-xs text-slate-500">
                      {h.lastInteractionDate ? new Date(h.lastInteractionDate).toLocaleDateString() : "Never"}
                    </TableCell>
                    <TableCell className="text-right">
                      <Link href={`/dashboard/customers/${h.customerId}`}>
                        <Button variant="ghost" size="sm" className="gap-1 text-xs text-purple-600 hover:text-purple-700">
                          <Eye className="h-3.5 w-3.5" /> 360° Profile
                        </Button>
                      </Link>
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={8} className="text-center py-8 text-slate-500 text-sm">
                    No customer health records found matching filters.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
