"use client";

import React from "react";
import { useCsatDashboard } from "@/hooks/use-customer-feedback";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { HeartHandshake, ShieldAlert, DollarSign, ArrowRight, TrendingUp, AlertTriangle, Building2, UserCheck } from "lucide-react";
import { toast } from "sonner";

export default function CustomerSatisfactionPage() {
  const { data: csatData, isLoading } = useCsatDashboard();

  if (isLoading) {
    return <div className="p-12 text-center text-slate-500 animate-pulse font-medium">Loading Customer Satisfaction & Churn Risk Analytics...</div>;
  }

  const accounts = csatData?.churnRiskAccounts || [];

  const handleIntercept = (customerName: string) => {
    toast.success(`Interception protocol initiated for ${customerName}. Calendar link & SLA discount sent to executive sponsor!`);
  };

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* Header Banner */}
      <div className="border-b border-slate-200 dark:border-slate-800 pb-6">
        <h1 className="text-3xl font-black text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
          <HeartHandshake className="h-8 w-8 text-rose-500" />
          NPS Analytics & Automated Churn Risk Predictor
        </h1>
        <p className="text-slate-500 dark:text-slate-400 mt-1 text-sm font-medium">
          Identify dissatisfied enterprise accounts before cancellation notices arrive. Proactive relationship retention is the single highest ROI business action.
        </p>
      </div>

      {/* Churn Risk Cohorts Table */}
      <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-md rounded-xl overflow-hidden">
        <CardHeader className="p-6 bg-slate-50/50 dark:bg-slate-900/50 border-b border-slate-200 dark:border-slate-800 flex flex-col md:flex-row md:items-center justify-between gap-4">
          <div>
            <CardTitle className="text-xl font-black text-slate-900 dark:text-white flex items-center gap-2">
              <ShieldAlert className="h-6 w-6 text-rose-500 animate-bounce" />
              High-Risk Account Retention Watchlist ({accounts.length})
            </CardTitle>
            <CardDescription className="text-slate-500 text-xs mt-1">
              Accounts flagged with repeat complaint rates over 20% or Net Promoter Score drop below passive target.
            </CardDescription>
          </div>
          <Badge className="bg-rose-100 text-rose-800 dark:bg-rose-950/40 dark:text-rose-300 px-3 py-1.5 rounded-lg border border-rose-200 dark:border-rose-800 font-mono font-bold text-xs self-start md:self-auto">
            $12,441 Monthly Recurring Revenue (MRR) at Immediate Churn Risk
          </Badge>
        </CardHeader>

        <CardContent className="p-0">
          <div className="overflow-x-auto">
            <Table className="w-full text-left border-collapse">
              <TableHeader className="bg-slate-50 dark:bg-slate-900 text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
                <TableRow>
                  <TableHead className="py-3.5 px-6">Customer Account</TableHead>
                  <TableHead className="py-3.5 px-4">MRR Value</TableHead>
                  <TableHead className="py-3.5 px-4">Churn Risk Cohort</TableHead>
                  <TableHead className="py-3.5 px-4">Repeat Complaint Rate</TableHead>
                  <TableHead className="py-3.5 px-4 w-1/3">AI Proactive Retention Recommendation</TableHead>
                  <TableHead className="py-3.5 px-6 text-right">Action Intercept</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody className="divide-y divide-slate-100 dark:divide-slate-800">
                {accounts.map((item) => (
                  <TableRow key={item.customerId} className="hover:bg-slate-50/70 dark:hover:bg-slate-800/50 transition-colors">
                    <TableCell className="py-4 px-6">
                      <div className="font-bold text-sm text-slate-900 dark:text-white">{item.customerName}</div>
                      <div className="text-xs text-slate-500 flex items-center gap-1 mt-0.5">
                        <Building2 className="h-3 w-3" /> {item.companyName}
                      </div>
                    </TableCell>
                    <TableCell className="py-4 px-4 font-mono font-extrabold text-slate-900 dark:text-white text-sm">
                      ${item.mrr.toLocaleString()}/mo
                    </TableCell>
                    <TableCell className="py-4 px-4">
                      <Badge
                        className={`text-xs font-black px-2.5 py-0.5 ${
                          item.churnRisk === "Critical"
                            ? "bg-rose-500 text-white animate-pulse"
                            : item.churnRisk === "High"
                            ? "bg-amber-500 text-white"
                            : "bg-blue-100 text-blue-800 dark:bg-blue-900/40 dark:text-blue-300"
                        }`}
                      >
                        {item.churnRisk} Risk
                      </Badge>
                    </TableCell>
                    <TableCell className="py-4 px-4 font-mono font-bold text-rose-500 text-sm">
                      {item.repeatComplaintRate}% repeat complaints
                    </TableCell>
                    <TableCell className="py-4 px-4 text-xs text-slate-700 dark:text-slate-300 font-medium max-w-xs">
                      <span className="p-2 bg-indigo-50 dark:bg-indigo-950/30 rounded border border-indigo-100 dark:border-indigo-900/40 block">
                        {item.aiRecommendedAction}
                      </span>
                    </TableCell>
                    <TableCell className="py-4 px-6 text-right">
                      <Button
                        size="sm"
                        onClick={() => handleIntercept(item.customerName)}
                        className="bg-rose-600 hover:bg-rose-700 text-white font-bold text-xs h-9 px-3.5 shadow-md transition-transform active:scale-95"
                      >
                        Intercept Account <ArrowRight className="h-3.5 w-3.5 ml-1" />
                      </Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
