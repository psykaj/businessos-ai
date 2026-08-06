"use client";

import React, { useState } from "react";
import { CustomerInsightDto, ChurnRiskCustomerDto, VipCustomerDto } from "@/lib/business-intelligence-service";
import { 
  Users, 
  UserCheck, 
  UserMinus, 
  AlertTriangle, 
  Mail, 
  Sparkles, 
  Check, 
  Loader2, 
  Smile, 
  TrendingUp, 
  Crown,
  ChevronRight
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

interface CustomerIntelligenceViewProps {
  customers: CustomerInsightDto;
}

export const CustomerIntelligenceView: React.FC<CustomerIntelligenceViewProps> = ({ customers }) => {
  const [activeTab, setActiveTab] = useState<"churn" | "vip">("churn");
  const [processingId, setProcessingId] = useState<string | null>(null);
  const [retainedIds, setRetainedIds] = useState<Record<string, boolean>>({});

  const handleTriggerRetention = async (cust: ChurnRiskCustomerDto) => {
    setProcessingId(cust.customerId);
    await new Promise((res) => setTimeout(res, 750));
    setRetainedIds((prev) => ({ ...prev, [cust.customerId]: true }));
    setProcessingId(null);
    toast.success("Retention Action Initiated", {
      description: `Dispatched automated re-engagement SMS & email sequence to ${cust.customerName} (${cust.email}).`,
    });
  };

  const handleVipReward = (vip: VipCustomerDto) => {
    toast.success("VIP Perks Activated", {
      description: `Applied Diamond Tier-2 loyalty discount bundle to ${vip.name} billing profile.`,
    });
  };

  return (
    <div className="rounded-3xl bg-white dark:bg-slate-900 border border-slate-200/80 dark:border-slate-800 shadow-xl p-6 space-y-6">
      
      {/* Section Title & Summary */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 pb-4 border-b border-slate-100 dark:border-slate-800">
        <div className="flex items-center gap-3">
          <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-blue-500/10 text-blue-600 dark:text-blue-400">
            <Users className="h-6 w-6" />
          </div>
          <div>
            <h3 className="text-xl font-bold text-slate-900 dark:text-white flex items-center gap-2">
              Customer Intelligence & Churn AI
              <span className="text-xs bg-red-500/10 text-red-500 border border-red-500/20 px-2.5 py-0.5 rounded-full font-extrabold animate-pulse">
                {customers.atRiskCustomerCount} At Risk
              </span>
            </h3>
            <p className="text-xs text-slate-500 dark:text-slate-400">
              Predictive churn warning triggers and customer lifetime value (LTV) cohorts
            </p>
          </div>
        </div>

        {/* Tab switcher */}
        <div className="flex items-center gap-1.5 p-1 rounded-2xl bg-slate-100 dark:bg-slate-800/60 border border-slate-200 dark:border-slate-700">
          <button
            onClick={() => setActiveTab("churn")}
            className={cn(
              "px-4 py-2 rounded-xl text-xs font-bold transition-all flex items-center gap-1.5",
              activeTab === "churn"
                ? "bg-white dark:bg-red-600 text-slate-900 dark:text-white shadow-md shadow-red-500/10"
                : "text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-white"
            )}
          >
            <AlertTriangle className="h-3.5 w-3.5" />
            <span>At-Risk Churn Warnings ({customers.topChurnRisks?.length || 0})</span>
          </button>
          <button
            onClick={() => setActiveTab("vip")}
            className={cn(
              "px-4 py-2 rounded-xl text-xs font-bold transition-all flex items-center gap-1.5",
              activeTab === "vip"
                ? "bg-white dark:bg-amber-500 text-slate-900 dark:text-slate-950 shadow-md shadow-amber-500/20 font-black"
                : "text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-white"
            )}
          >
            <Crown className="h-3.5 w-3.5 text-amber-500" />
            <span>VIP Top Spenders</span>
          </button>
        </div>
      </div>

      {/* 4 KPI Cards Grid */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="p-4 rounded-2xl bg-slate-50 dark:bg-slate-800/50 border border-slate-200/60 dark:border-slate-800 flex items-center gap-4">
          <div className="p-3 rounded-xl bg-blue-500/10 text-blue-500">
            <UserCheck className="h-6 w-6" />
          </div>
          <div>
            <span className="text-2xl font-black font-mono text-slate-900 dark:text-white">{customers.totalCustomers}</span>
            <span className="text-xs font-bold text-emerald-500 block">+{customers.growthRate}% M-o-M</span>
            <span className="text-[11px] text-slate-500 dark:text-slate-400 font-medium">Active Accounts</span>
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-slate-50 dark:bg-slate-800/50 border border-slate-200/60 dark:border-slate-800 flex items-center gap-4">
          <div className="p-3 rounded-xl bg-purple-500/10 text-purple-500">
            <TrendingUp className="h-6 w-6" />
          </div>
          <div>
            <span className="text-2xl font-black font-mono text-slate-900 dark:text-white">{customers.repeatCustomerRate}%</span>
            <span className="text-[11px] text-purple-600 dark:text-purple-400 font-bold block">Strong Loyalty</span>
            <span className="text-[11px] text-slate-500 dark:text-slate-400 font-medium">Repeat Order Rate</span>
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-slate-50 dark:bg-slate-800/50 border border-slate-200/60 dark:border-slate-800 flex items-center gap-4">
          <div className="p-3 rounded-xl bg-emerald-500/10 text-emerald-500">
            <Smile className="h-6 w-6" />
          </div>
          <div>
            <span className="text-2xl font-black font-mono text-slate-900 dark:text-white">{customers.averageCsatScore}%</span>
            <span className="text-[11px] text-emerald-600 dark:text-emerald-400 font-bold block">Excellent Rating</span>
            <span className="text-[11px] text-slate-500 dark:text-slate-400 font-medium">Average CSAT Score</span>
          </div>
        </div>

        <div className="p-4 rounded-2xl bg-red-500/5 dark:bg-red-950/20 border border-red-500/20 flex items-center gap-4">
          <div className="p-3 rounded-xl bg-red-500/10 text-red-500">
            <UserMinus className="h-6 w-6" />
          </div>
          <div>
            <span className="text-2xl font-black font-mono text-red-600 dark:text-red-400">{customers.atRiskCustomerCount}</span>
            <span className="text-xs text-red-500 font-bold block">Immediate Focus</span>
            <span className="text-[11px] text-slate-500 dark:text-slate-400 font-medium">Elevated Churn Risk</span>
          </div>
        </div>
      </div>

      {/* Tab 1: Churn Risk Customers Table */}
      {activeTab === "churn" && (
        <div className="space-y-4">
          <div className="overflow-x-auto rounded-2xl border border-slate-200 dark:border-slate-800">
            <table className="w-full text-left border-collapse">
              <thead>
                <tr className="bg-slate-100 dark:bg-slate-800/80 text-[11px] font-extrabold uppercase tracking-wider text-slate-600 dark:text-slate-400 border-b border-slate-200 dark:border-slate-700">
                  <th className="p-4">Customer Account</th>
                  <th className="p-4">Risk Level</th>
                  <th className="p-4">AI Diagnosed Risk Reason</th>
                  <th className="p-4">Recommended Retention Plan</th>
                  <th className="p-4 text-right">Action</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100 dark:divide-slate-800 text-xs">
                {(customers.topChurnRisks || []).map((risk) => {
                  const isProcessing = processingId === risk.customerId;
                  const isRetained = retainedIds[risk.customerId];

                  return (
                    <tr key={risk.customerId} className="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors">
                      <td className="p-4 font-bold text-slate-900 dark:text-white">
                        <div className="flex items-center gap-2">
                          <div className="h-8 w-8 rounded-full bg-slate-200 dark:bg-slate-700 flex items-center justify-center font-bold text-xs uppercase">
                            {risk.customerName.charAt(0)}
                          </div>
                          <div>
                            <span className="block">{risk.customerName}</span>
                            <span className="text-[10px] text-slate-400 font-normal">{risk.email}</span>
                          </div>
                        </div>
                      </td>
                      <td className="p-4">
                        <span className={cn(
                          "px-2 py-0.5 rounded-md text-[10px] font-black uppercase tracking-wide border",
                          risk.riskLevel === "Critical"
                            ? "bg-red-500 text-white border-red-600 shadow-sm shadow-red-500/30 animate-pulse"
                            : "bg-amber-500/20 text-amber-600 dark:text-amber-400 border-amber-500/30"
                        )}>
                          {risk.riskLevel}
                        </span>
                      </td>
                      <td className="p-4 text-slate-600 dark:text-slate-300 max-w-xs leading-snug">
                        {risk.riskReason}
                      </td>
                      <td className="p-4 text-indigo-600 dark:text-indigo-400 font-semibold max-w-xs leading-snug">
                        {risk.retentionAction}
                      </td>
                      <td className="p-4 text-right">
                        <Button
                          size="sm"
                          disabled={isProcessing || isRetained}
                          onClick={() => handleTriggerRetention(risk)}
                          className={cn(
                            "text-xs font-bold rounded-xl px-3 py-2 transition-all shadow-sm",
                            isRetained
                              ? "bg-emerald-600 hover:bg-emerald-600 text-white cursor-default"
                              : "bg-red-600 hover:bg-red-500 text-white shadow-red-500/20"
                          )}
                        >
                          {isProcessing ? (
                            <Loader2 className="h-3.5 w-3.5 animate-spin" />
                          ) : isRetained ? (
                            <span className="flex items-center gap-1"><Check className="h-3 w-3" /> Outreach Sent</span>
                          ) : (
                            <span className="flex items-center gap-1"><Mail className="h-3 w-3" /> Trigger Retention</span>
                          )}
                        </Button>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Tab 2: VIP Top Customers Table */}
      {activeTab === "vip" && (
        <div className="space-y-4">
          <div className="overflow-x-auto rounded-2xl border border-slate-200 dark:border-slate-800">
            <table className="w-full text-left border-collapse">
              <thead>
                <tr className="bg-slate-100 dark:bg-slate-800/80 text-[11px] font-extrabold uppercase tracking-wider text-slate-600 dark:text-slate-400 border-b border-slate-200 dark:border-slate-700">
                  <th className="p-4">VIP Client Name</th>
                  <th className="p-4">Loyalty Tier</th>
                  <th className="p-4">Lifetime Spent ($)</th>
                  <th className="p-4">Repeat Order Velocity</th>
                  <th className="p-4">Status</th>
                  <th className="p-4 text-right">Action</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100 dark:divide-slate-800 text-xs">
                {(customers.vipCustomers || []).map((vip) => (
                  <tr key={vip.id} className="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors">
                    <td className="p-4 font-bold text-slate-900 dark:text-white">
                      <div className="flex items-center gap-2.5">
                        <div className="h-8 w-8 rounded-full bg-amber-500/20 text-amber-500 flex items-center justify-center font-black text-xs">
                          <Crown className="h-4 w-4" />
                        </div>
                        <div>
                          <span className="block">{vip.name}</span>
                          <span className="text-[10px] text-slate-400 font-normal">{vip.email}</span>
                        </div>
                      </div>
                    </td>
                    <td className="p-4">
                      <span className="px-2 py-0.5 rounded-md text-[10px] font-black uppercase tracking-wider bg-amber-500/20 text-amber-600 dark:text-amber-300 border border-amber-500/30">
                        {vip.tier}
                      </span>
                    </td>
                    <td className="p-4 font-extrabold font-mono text-emerald-600 dark:text-emerald-400 text-sm">
                      ${vip.totalSpent.toLocaleString()}
                    </td>
                    <td className="p-4 font-bold text-slate-700 dark:text-slate-300">
                      {vip.repeatOrdersCount} Repeat Orders
                    </td>
                    <td className="p-4">
                      <span className="px-2 py-0.5 rounded-full text-[10px] font-bold bg-emerald-500/10 text-emerald-500 border border-emerald-500/20">
                        {vip.status}
                      </span>
                    </td>
                    <td className="p-4 text-right">
                      <Button
                        size="sm"
                        onClick={() => handleVipReward(vip)}
                        className="text-xs font-bold bg-indigo-600 hover:bg-indigo-500 text-white rounded-xl shadow-sm px-3"
                      >
                        Apply Perks
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* AI Summary Footer */}
      <div className="p-3.5 rounded-2xl bg-blue-500/5 dark:bg-blue-950/20 border border-blue-500/20 text-xs flex items-center justify-between">
        <span className="text-slate-700 dark:text-slate-300 flex items-center gap-2">
          <Sparkles className="h-4 w-4 text-blue-500 shrink-0" />
          <strong className="text-blue-600 dark:text-blue-400 uppercase tracking-wider">AI Customer Summary:</strong> {customers.summary}
        </span>
        <ChevronRight className="h-4 w-4 text-slate-400 hidden sm:block" />
      </div>
    </div>
  );
};
