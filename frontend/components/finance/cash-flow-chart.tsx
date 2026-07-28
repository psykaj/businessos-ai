"use client";

import { MonthlyCashFlowPointDto } from "@/types/finance";
import {
  ResponsiveContainer,
  AreaChart,
  Area,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  Legend,
} from "recharts";

interface CashFlowChartProps {
  data?: MonthlyCashFlowPointDto[];
  isLoading?: boolean;
}

export function CashFlowChart({ data, isLoading }: CashFlowChartProps) {
  if (isLoading) {
    return <div className="h-72 bg-card/50 animate-pulse rounded-xl border border-border" />;
  }

  const chartData = data && data.length > 0 ? data : [
    { month: "Jan", cashIn: 45000, cashOut: 32000, netFlow: 13000 },
    { month: "Feb", cashIn: 52000, cashOut: 38000, netFlow: 14000 },
    { month: "Mar", cashIn: 61000, cashOut: 42000, netFlow: 19000 },
    { month: "Apr", cashIn: 58000, cashOut: 41000, netFlow: 17000 },
    { month: "May", cashIn: 67000, cashOut: 45000, netFlow: 22000 },
    { month: "Jun", cashIn: 74000, cashOut: 49000, netFlow: 25000 },
  ];

  return (
    <div className="bg-card border border-border rounded-xl p-5 shadow-sm">
      <div className="flex items-center justify-between mb-4">
        <div>
          <h3 className="text-base font-semibold text-foreground">Cash Inflow vs Outflow</h3>
          <p className="text-xs text-muted-foreground">Historical cash movement breakdown over time</p>
        </div>
      </div>
      <div className="h-72 w-full">
        <ResponsiveContainer width="100%" height="100%">
          <AreaChart data={chartData} margin={{ top: 10, right: 10, left: 0, bottom: 0 }}>
            <defs>
              <linearGradient id="colorCashIn" x1="0" y1="0" x2="0" y2="1">
                <stop offset="5%" stopColor="#10B981" stopOpacity={0.4} />
                <stop offset="95%" stopColor="#10B981" stopOpacity={0.0} />
              </linearGradient>
              <linearGradient id="colorCashOut" x1="0" y1="0" x2="0" y2="1">
                <stop offset="5%" stopColor="#EF4444" stopOpacity={0.4} />
                <stop offset="95%" stopColor="#EF4444" stopOpacity={0.0} />
              </linearGradient>
            </defs>
            <CartesianGrid strokeDasharray="3 3" stroke="#374151" opacity={0.3} />
            <XAxis dataKey="month" stroke="#9CA3AF" fontSize={12} />
            <YAxis stroke="#9CA3AF" fontSize={12} tickFormatter={(val) => `$${val / 1000}k`} />
            <Tooltip
              contentStyle={{ backgroundColor: "#1F2937", borderColor: "#374151", color: "#F9FAFB", borderRadius: "8px" }}
              formatter={(val: any) => [`$${Number(val).toLocaleString()}`, ""]}
            />
            <Legend wrapperStyle={{ paddingTop: "10px" }} />
            <Area type="monotone" dataKey="cashIn" name="Cash In (Collections)" stroke="#10B981" fillOpacity={1} fill="url(#colorCashIn)" strokeWidth={2} />
            <Area type="monotone" dataKey="cashOut" name="Cash Out (Expenses/Bills)" stroke="#EF4444" fillOpacity={1} fill="url(#colorCashOut)" strokeWidth={2} />
          </AreaChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}
