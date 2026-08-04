"use client";

import React, { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { ProductPerformanceItem } from "@/lib/business-performance-service";
import { useOptimizeStockOrPricing } from "@/hooks/use-business-performance";
import { Package, TrendingUp, AlertOctagon, CheckCircle2, Zap, ArrowRight, DollarSign } from "lucide-react";
import { cn } from "@/lib/utils";

interface MarginRankingTableProps {
  products: ProductPerformanceItem[];
  title?: string;
  description?: string;
}

export function MarginRankingTable({
  products,
  title = "Product Profitability & Loss Center Elimination",
  description = "Ranked by Gross Margin (%) with automated inventory reordering and negative operating loss detection.",
}: MarginRankingTableProps) {
  const { mutate: optimizeProduct, isPending } = useOptimizeStockOrPricing();
  const [optimizedIds, setOptimizedIds] = useState<Record<string, boolean>>({});

  const handleAction = (prod: ProductPerformanceItem) => {
    optimizeProduct({
      productId: prod.id,
      productName: prod.name,
      actionName: prod.aiRecommendedAction,
      impact: prod.estimatedImpact,
    });
    setOptimizedIds((prev) => ({ ...prev, [prod.id]: true }));
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Loss Center":
        return <Badge className="bg-rose-500/20 text-rose-600 border-rose-500/30 font-bold animate-pulse">Loss Center (-20% Margin)</Badge>;
      case "High Performing":
        return <Badge className="bg-emerald-500/15 text-emerald-600 border-emerald-500/30 font-bold">Top Quartile (93% Margin)</Badge>;
      case "Slow Moving":
        return <Badge className="bg-amber-500/15 text-amber-600 border-amber-500/30">Slow Moving Stock</Badge>;
      default:
        return <Badge variant="secondary" className="font-medium">Steady Core</Badge>;
    }
  };

  return (
    <Card className="border border-border bg-card shadow-sm">
      <CardHeader className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between border-b border-border/50 pb-4">
        <div>
          <CardTitle className="text-lg font-bold tracking-tight flex items-center gap-2">
            <Package className="h-5 w-5 text-primary" />
            {title}
          </CardTitle>
          <CardDescription className="text-xs text-muted-foreground mt-1">
            {description}
          </CardDescription>
        </div>
        <div className="flex items-center gap-2 text-xs">
          <span className="px-3 py-1 rounded-full bg-primary/10 text-primary font-semibold border border-primary/20">
            SaaS Target Gross Margin: &gt;80%
          </span>
        </div>
      </CardHeader>

      <CardContent className="p-0 overflow-x-auto">
        <table className="w-full text-left text-sm">
          <thead className="bg-muted/50 text-xs uppercase tracking-wider text-muted-foreground border-b border-border/60">
            <tr>
              <th className="py-3.5 px-4 font-semibold">Offering & Category</th>
              <th className="py-3.5 px-4 font-semibold">Revenue Yield</th>
              <th className="py-3.5 px-4 font-semibold">Unit Price vs Cost</th>
              <th className="py-3.5 px-4 font-semibold">Gross Margin %</th>
              <th className="py-3.5 px-4 font-semibold">Status</th>
              <th className="py-3.5 px-4 font-semibold text-right">AI Action Strategy</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-border/40">
            {products.map((prod) => {
              const isLoss = prod.grossMarginPercentage < 0;
              const isOptimized = optimizedIds[prod.id];

              return (
                <tr key={prod.id} className={cn("hover:bg-muted/30 transition-colors group", isLoss ? "bg-rose-500/5 hover:bg-rose-500/10" : "")}>
                  <td className="py-4 px-4">
                    <div className="font-bold text-foreground group-hover:text-primary transition-colors">
                      {prod.name}
                    </div>
                    <div className="text-xs text-muted-foreground font-mono flex items-center gap-1.5 mt-0.5">
                      <span>SKU: {prod.sku}</span>
                      <span>•</span>
                      <span className="text-primary font-semibold">{prod.category}</span>
                    </div>
                  </td>

                  <td className="py-4 px-4 font-mono">
                    <div className="font-bold text-foreground">${prod.revenue.toLocaleString()}</div>
                    <div className="text-xs text-muted-foreground">{prod.unitsSold} active subscriptions</div>
                  </td>

                  <td className="py-4 px-4">
                    <div className="font-semibold text-foreground">${prod.unitPrice.toLocaleString()} / unit</div>
                    <div className="text-xs text-muted-foreground">Cost: ${prod.unitCost.toLocaleString()}</div>
                  </td>

                  <td className="py-4 px-4">
                    <div className="flex flex-col gap-1 w-28">
                      <span className={cn("font-bold text-sm", isLoss ? "text-rose-600 dark:text-rose-400" : "text-emerald-600 dark:text-emerald-400")}>
                        {prod.grossMarginPercentage}%
                      </span>
                      <Progress value={Math.max(0, Math.min(100, prod.grossMarginPercentage))} className="h-1.5" />
                    </div>
                  </td>

                  <td className="py-4 px-4">
                    {getStatusBadge(prod.status)}
                  </td>

                  <td className="py-4 px-4 text-right">
                    <div className="flex flex-col items-end gap-1.5 max-w-xs ml-auto">
                      <p className="text-xs text-muted-foreground leading-snug">
                        {prod.aiRecommendedAction}{" "}
                        <span className="text-emerald-600 dark:text-emerald-400 font-bold">
                          ({isLoss ? `+$${prod.estimatedImpact.toLocaleString()} saved` : `+$${prod.estimatedImpact.toLocaleString()}/yr`})
                        </span>
                      </p>
                      {isOptimized ? (
                        <Button size="sm" variant="outline" disabled className="h-7 text-xs bg-emerald-500/10 text-emerald-600 border-emerald-500/30">
                          <CheckCircle2 className="mr-1 h-3.5 w-3.5" />
                          Optimization Applied
                        </Button>
                      ) : (
                        <Button
                          size="sm"
                          onClick={() => handleAction(prod)}
                          disabled={isPending}
                          className={cn(
                            "h-7 text-xs font-semibold transition-transform active:scale-95 shadow-sm",
                            isLoss
                              ? "bg-rose-600 text-white hover:bg-rose-700"
                              : "bg-primary text-primary-foreground hover:bg-primary/90"
                          )}
                        >
                          {isLoss ? "Sunset & Save $19.2k" : "Apply Pricing Strategy"}
                          <ArrowRight className="ml-1 h-3.5 w-3.5" />
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
