"use client";

import React, { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { CustomerLtvCacItem } from "@/lib/business-performance-service";
import { useExecuteGrowthAction } from "@/hooks/use-business-performance";
import { Users, AlertTriangle, ArrowUpRight, CheckCircle2, ShieldAlert, Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface LtvCacMatrixProps {
  customers: CustomerLtvCacItem[];
  title?: string;
  description?: string;
}

export function LtvCacMatrix({
  customers,
  title = "Customer Lifetime Value (LTV) & CAC Optimization",
  description = "Audit high-margin expansion opportunities and trigger automated win-back intervention for at-risk accounts.",
}: LtvCacMatrixProps) {
  const { mutate: executeAction, isPending } = useExecuteGrowthAction();
  const [actionedIds, setActionedIds] = useState<Record<string, boolean>>({});

  const handleTriggerAction = (cust: CustomerLtvCacItem) => {
    executeAction({
      id: cust.customerId,
      actionType: cust.upsellOpportunity,
      estimatedImpact: cust.estimatedUpsellValue,
    });
    setActionedIds((prev) => ({ ...prev, [cust.customerId]: true }));
  };

  const getRiskBadge = (risk: string) => {
    switch (risk) {
      case "Critical":
        return <Badge className="bg-rose-500/20 text-rose-600 border-rose-500/30 font-bold animate-pulse">Critical Risk</Badge>;
      case "High":
        return <Badge className="bg-orange-500/15 text-orange-600 border-orange-500/30 font-semibold">High Churn Risk</Badge>;
      case "Medium":
        return <Badge className="bg-amber-500/15 text-amber-600 border-amber-500/30 font-medium">Moderate</Badge>;
      default:
        return <Badge variant="outline" className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 font-semibold">Low Risk (98% Repeat)</Badge>;
    }
  };

  return (
    <Card className="border border-border bg-card shadow-sm">
      <CardHeader className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between border-b border-border/50 pb-4">
        <div>
          <CardTitle className="text-lg font-bold tracking-tight flex items-center gap-2">
            <Users className="h-5 w-5 text-primary" />
            {title}
          </CardTitle>
          <CardDescription className="text-xs text-muted-foreground mt-1">
            {description}
          </CardDescription>
        </div>
        <div className="flex items-center gap-2 text-xs font-semibold">
          <span className="px-2.5 py-1 rounded-md bg-emerald-500/10 text-emerald-600 border border-emerald-500/20">
            Target LTV:CAC Range: 3.0x - 6.0x+
          </span>
        </div>
      </CardHeader>

      <CardContent className="p-0 overflow-x-auto">
        <table className="w-full text-left text-sm">
          <thead className="bg-muted/50 text-xs uppercase tracking-wider text-muted-foreground border-b border-border/60">
            <tr>
              <th className="py-3.5 px-4 font-semibold">Account & Tier</th>
              <th className="py-3.5 px-4 font-semibold">MRR / LTV</th>
              <th className="py-3.5 px-4 font-semibold">CAC / Ratio</th>
              <th className="py-3.5 px-4 font-semibold">Repeat Rate</th>
              <th className="py-3.5 px-4 font-semibold">Churn Status</th>
              <th className="py-3.5 px-4 font-semibold text-right">AI Recommended Action</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-border/40">
            {customers.map((cust) => {
              const isHealthy = cust.ltvCacRatio >= 3.0;
              const isActioned = actionedIds[cust.customerId];

              return (
                <tr key={cust.customerId} className="hover:bg-muted/30 transition-colors group">
                  <td className="py-4 px-4">
                    <div className="font-bold text-foreground group-hover:text-primary transition-colors">
                      {cust.companyName}
                    </div>
                    <div className="text-xs text-muted-foreground flex items-center gap-1 mt-0.5">
                      <span>{cust.customerName}</span>
                      <span>•</span>
                      <span className="text-primary font-medium">{cust.tier}</span>
                    </div>
                  </td>

                  <td className="py-4 px-4 font-mono">
                    <div className="font-semibold text-foreground">${cust.mrr.toLocaleString()}/mo</div>
                    <div className="text-xs text-muted-foreground">LTV: ${cust.ltv.toLocaleString()}</div>
                  </td>

                  <td className="py-4 px-4">
                    <div className={cn("font-bold text-sm inline-flex items-center gap-1", isHealthy ? "text-emerald-600 dark:text-emerald-400" : "text-rose-600 dark:text-rose-400")}>
                      {cust.ltvCacRatio}x Ratio
                      {isHealthy && <Sparkles className="h-3 w-3 fill-emerald-500" />}
                    </div>
                    <div className="text-xs text-muted-foreground">CAC: ${cust.cac.toLocaleString()}</div>
                  </td>

                  <td className="py-4 px-4">
                    <div className="font-semibold text-foreground">{cust.repeatPurchaseRate}%</div>
                    <div className="text-xs text-muted-foreground">Active {cust.lastActiveDaysAgo === 0 ? "today" : `${cust.lastActiveDaysAgo}d ago`}</div>
                  </td>

                  <td className="py-4 px-4">
                    {getRiskBadge(cust.churnRiskScore)}
                  </td>

                  <td className="py-4 px-4 text-right">
                    <div className="flex flex-col items-end gap-1.5 max-w-xs ml-auto">
                      <p className="text-xs text-muted-foreground leading-snug">
                        {cust.upsellOpportunity} <span className="text-emerald-600 dark:text-emerald-400 font-bold">(+${cust.estimatedUpsellValue.toLocaleString()}/yr)</span>
                      </p>
                      {isActioned ? (
                        <Button size="sm" variant="outline" disabled className="h-7 text-xs bg-emerald-500/10 text-emerald-600 border-emerald-500/30">
                          <CheckCircle2 className="mr-1 h-3.5 w-3.5" />
                          Workflow Triggered
                        </Button>
                      ) : (
                        <Button
                          size="sm"
                          onClick={() => handleTriggerAction(cust)}
                          disabled={isPending}
                          className={cn(
                            "h-7 text-xs font-semibold transition-transform active:scale-95 shadow-sm",
                            cust.churnRiskScore === "Critical" || cust.churnRiskScore === "High"
                              ? "bg-rose-600 text-white hover:bg-rose-700"
                              : "bg-primary text-primary-foreground hover:bg-primary/90"
                          )}
                        >
                          {cust.churnRiskScore === "Critical" || cust.churnRiskScore === "High" ? "Trigger Win-Back Call" : "Send Upgrade Offer"}
                          <ArrowUpRight className="ml-1 h-3.5 w-3.5" />
                        </Button>
                      )}
                    </div>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </CardContent>
    </Card>
  );
}
