"use client";

import React from "react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { ArrowUpRight, ArrowDownRight, Sparkles, Zap, HelpCircle } from "lucide-react";
import { cn } from "@/lib/utils";

interface KpiActionCardProps {
  title: string;
  value: string | number;
  subtitle?: string;
  trendPercentage?: number;
  trendLabel?: string;
  icon?: React.ReactNode;
  businessImpact?: string;
  actionLabel?: string;
  onActionClick?: () => void;
  variant?: "default" | "success" | "warning" | "ai";
  className?: string;
}

export function KpiActionCard({
  title,
  value,
  subtitle,
  trendPercentage,
  trendLabel = "vs prior period",
  icon,
  businessImpact,
  actionLabel,
  onActionClick,
  variant = "default",
  className,
}: KpiActionCardProps) {
  const isPositive = trendPercentage !== undefined && trendPercentage >= 0;

  const getBorderColor = () => {
    switch (variant) {
      case "success": return "hover:border-emerald-500/50 border-emerald-500/20";
      case "warning": return "hover:border-amber-500/50 border-amber-500/20";
      case "ai": return "hover:border-primary/60 border-primary/30 bg-gradient-to-br from-primary/5 via-transparent to-transparent shadow-sm shadow-primary/5";
      default: return "hover:border-primary/40 border-border";
    }
  };

  return (
    <Card
      className={cn(
        "group relative overflow-hidden transition-all duration-300 hover:shadow-md",
        getBorderColor(),
        className
      )}
    >
      {/* Top right decorative indicator for AI variant */}
      {variant === "ai" && (
        <div className="absolute -right-6 -top-6 h-16 w-16 rounded-full bg-primary/10 blur-xl group-hover:bg-primary/20 transition-all duration-500" />
      )}

      <CardContent className="p-5">
        <div className="flex items-start justify-between gap-4">
          <div className="space-y-1">
            <span className="text-xs font-medium uppercase tracking-wider text-muted-foreground">
              {title}
            </span>
            <div className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
              {value}
            </div>
          </div>
          {icon && (
            <div className={cn(
              "flex h-10 w-10 shrink-0 items-center justify-center rounded-xl transition-transform duration-300 group-hover:scale-110",
              variant === "ai" ? "bg-primary text-primary-foreground shadow-md shadow-primary/30" : "bg-muted text-muted-foreground"
            )}>
              {icon}
            </div>
          )}
        </div>

        {subtitle && (
          <p className="mt-1 text-xs text-muted-foreground">{subtitle}</p>
        )}

        {/* Trend Indicator & Business Value Badge */}
        <div className="mt-4 flex items-center justify-between gap-2 border-t border-border/50 pt-3">
          <div className="flex items-center gap-1.5 text-xs">
            {trendPercentage !== undefined ? (
              <>
                <span
                  className={cn(
                    "inline-flex items-center font-semibold rounded-md px-1.5 py-0.5",
                    isPositive
                      ? "bg-emerald-500/15 text-emerald-600 dark:text-emerald-400"
                      : "bg-rose-500/15 text-rose-600 dark:text-rose-400"
                  )}
                >
                  {isPositive ? <ArrowUpRight className="mr-0.5 h-3.5 w-3.5" /> : <ArrowDownRight className="mr-0.5 h-3.5 w-3.5" />}
                  {isPositive ? "+" : ""}
                  {trendPercentage}%
                </span>
                <span className="text-muted-foreground/80">{trendLabel}</span>
              </>
            ) : businessImpact ? (
              <span className="flex items-center gap-1 font-medium text-amber-600 dark:text-amber-400">
                <Zap className="h-3.5 w-3.5 text-amber-500 fill-amber-500 animate-pulse" />
                {businessImpact}
              </span>
            ) : (
              <span className="text-muted-foreground/60 text-xs italic">Real-time telemetry</span>
            )}
          </div>

          {/* Action button if present */}
          {actionLabel && onActionClick && (
            <Button
              size="sm"
              variant={variant === "ai" ? "default" : "outline"}
              onClick={(e) => {
                e.stopPropagation();
                onActionClick();
              }}
              className="h-7 text-xs font-semibold px-2.5 shadow-sm transition-transform active:scale-95 hover:bg-primary hover:text-primary-foreground"
            >
              <Sparkles className="mr-1 h-3 w-3" />
              {actionLabel}
            </Button>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
