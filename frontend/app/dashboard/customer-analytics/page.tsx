"use client";

import React from "react";
import { useCustomerAnalytics, useBusinessHealthSummary } from "@/hooks/use-business-performance";
import { KpiActionCard } from "@/components/business-performance/kpi-action-card";
import { LtvCacMatrix } from "@/components/business-performance/ltv-cac-matrix";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";
import { Users, Target, HeartHandshake, Sparkles, UserCheck, PhoneCall } from "lucide-react";
import { toast } from "sonner";

export default function CustomerAnalyticsPage() {
  const { data: customers, isLoading } = useCustomerAnalytics();
  const { data: health } = useBusinessHealthSummary();

  const handleWinBackAll = () => {
    toast.success("Automated Win-Back Sequences Dispatched!", {
      description: "AI Executive Assistant initiated priority intervention for 2 at-risk accounts ($3,050 combined MRR protected).",
      duration: 5000,
    });
  };

  if (isLoading || !customers || !health) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-12 w-1/3" />
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {[1, 2, 3].map((i) => (
            <Skeleton key={i} className="h-36 w-full rounded-xl" />
          ))}
        </div>
        <Skeleton className="h-96 w-full rounded-xl" />
      </div>
    );
  }

  const avgLtvCac = (
    customers.reduce((sum, c) => sum + c.ltvCacRatio, 0) / (customers.length || 1)
  ).toFixed(1);

  return (
    <div className="space-y-8 pb-12 animate-in fade-in-50 duration-300">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between border-b border-border/60 pb-6">
        <div>
          <h1 className="text-2xl font-black tracking-tight text-foreground sm:text-3xl flex items-center gap-2.5">
            <Users className="h-8 w-8 text-primary" />
            Customer & LTV/CAC Analytics Engine
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Monitor Customer Lifetime Value (LTV), repeat retention rates, and trigger proactive win-back interventions to protect enterprise ARR.
          </p>
        </div>
        <Button size="lg" onClick={handleWinBackAll} className="font-bold bg-rose-600 text-white hover:bg-rose-700 shadow-md">
          <PhoneCall className="mr-2 h-4 w-4 animate-bounce" />
          Trigger All VIP Win-Back Calls
        </Button>
      </div>

      {/* KPI Highlights */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-5">
        <KpiActionCard
          title="Avg LTV : CAC Ratio"
          value={`${avgLtvCac}x`}
          subtitle="Top cohort leader: VentureScale AI (14.7x)"
          businessImpact="Target Range: 3.0x - 6.0x+"
          variant="ai"
          icon={<Target className="h-5 w-5" />}
          actionLabel="View Matrix"
          onActionClick={() => {
            const el = document.getElementById("ltv-cac-table");
            el?.scrollIntoView({ behavior: "smooth" });
          }}
        />

        <KpiActionCard
          title="Repeat Purchase & Retention"
          value="95.2%"
          subtitle="Annual customer churn: only 1.4%"
          trendPercentage={8.4}
          trendLabel="YoY repeat rate lift"
          variant="success"
          icon={<UserCheck className="h-5 w-5" />}
          actionLabel="Win-Back VIPs"
          onActionClick={handleWinBackAll}
        />

        <KpiActionCard
          title="Total Open Upsell Vault"
          value="$78,600 / yr"
          subtitle="5 AI-detected tier expansion targets"
          businessImpact="+$18k ARR via Tool Registry add-on"
          variant="default"
          icon={<Sparkles className="h-5 w-5" />}
          actionLabel="Send Upgrade Offers"
          onActionClick={() => {
            toast.success("Upgrade offers dispatched!", { description: "Stripe one-click upgrade links sent to all qualified VIP accounts." });
          }}
        />
      </div>

      {/* LTV/CAC Table & Action Matrix */}
      <div id="ltv-cac-table">
        <LtvCacMatrix
          customers={customers}
          title="Top Customers & Churn Risk Action Matrix"
          description="Evaluate unit economics by account and execute automated expansion or win-back sequences."
        />
      </div>
    </div>
  );
}
