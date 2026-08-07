"use client";

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ActionCenterSummary } from "@/types/action-center";
import { ArrowDownToLine, ArrowUpToLine, CheckCircle2, Clock, DollarSign, Activity } from "lucide-react";
import { motion } from "framer-motion";

interface BusinessImpactDashboardProps {
  summary: ActionCenterSummary;
  isLoading?: boolean;
}

export function BusinessImpactDashboard({ summary, isLoading = false }: BusinessImpactDashboardProps) {
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
      maximumFractionDigits: 0,
    }).format(value);
  };

  const metrics = [
    {
      title: "Revenue Saved / Generated",
      value: formatCurrency(summary.revenueSaved),
      icon: DollarSign,
      color: "text-emerald-500",
      bgColor: "bg-emerald-500/10",
    },
    {
      title: "Costs Reduced",
      value: formatCurrency(summary.costReduced),
      icon: ArrowDownToLine,
      color: "text-blue-500",
      bgColor: "bg-blue-500/10",
    },
    {
      title: "Time Saved (hrs)",
      value: summary.timeSaved.toString(),
      icon: Clock,
      color: "text-amber-500",
      bgColor: "bg-amber-500/10",
    },
    {
      title: "Actions Executed",
      value: summary.actionsExecuted.toString(),
      icon: CheckCircle2,
      color: "text-indigo-500",
      bgColor: "bg-indigo-500/10",
    },
    {
      title: "Pending Actions",
      value: summary.pendingActions.toString(),
      icon: Activity,
      color: "text-rose-500",
      bgColor: "bg-rose-500/10",
    },
    {
      title: "Success Rate",
      value: `${summary.successRate}%`,
      icon: ArrowUpToLine,
      color: "text-teal-500",
      bgColor: "bg-teal-500/10",
    },
  ];

  const container = {
    hidden: { opacity: 0 },
    show: {
      opacity: 1,
      transition: {
        staggerChildren: 0.1,
      },
    },
  };

  const item = {
    hidden: { opacity: 0, y: 20 },
    show: { opacity: 1, y: 0 },
  };

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold tracking-tight">Business Impact</h2>
      <motion.div
        variants={container}
        initial="hidden"
        animate="show"
        className="grid gap-4 md:grid-cols-2 lg:grid-cols-3"
      >
        {metrics.map((metric, i) => (
          <motion.div key={i} variants={item}>
            <Card>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">
                  {metric.title}
                </CardTitle>
                <div className={`p-2 rounded-full ${metric.bgColor}`}>
                  <metric.icon className={`h-4 w-4 ${metric.color}`} />
                </div>
              </CardHeader>
              <CardContent>
                {isLoading ? (
                  <div className="h-8 w-24 bg-muted animate-pulse rounded-md" />
                ) : (
                  <div className="text-2xl font-bold">{metric.value}</div>
                )}
              </CardContent>
            </Card>
          </motion.div>
        ))}
      </motion.div>
    </div>
  );
}
