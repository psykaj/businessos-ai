"use client";

import { useMemo } from "react";
import { Area, AreaChart, CartesianGrid, XAxis, YAxis } from "recharts";
import { ChartConfig, ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart";

interface QRAnalyticsChartProps {
  totalScans: number;
}

const chartConfig = {
  scans: {
    label: "Scans",
    color: "hsl(var(--primary))",
  },
} satisfies ChartConfig;

export function QRAnalyticsChart({ totalScans }: QRAnalyticsChartProps) {
  const chartData = useMemo(() => {
    const days = 30;
    const data = [];
    const now = new Date();
    
    if (totalScans === 0) {
      for (let i = days - 1; i >= 0; i--) {
        const date = new Date(now);
        date.setDate(date.getDate() - i);
        data.push({
          date: date.toISOString().split('T')[0],
          scans: 0,
        });
      }
      return data;
    }

    // Generate random distribution of scans across 30 days
    const weights = Array.from({ length: days }, (_, i) => (i * 17) % 10 + 1);
    const totalWeight = weights.reduce((sum, w) => sum + w, 0);
    
    let currentSum = 0;
    for (let i = days - 1; i >= 0; i--) {
      const date = new Date(now);
      date.setDate(date.getDate() - i);
      
      let scansForDay = 0;
      if (i === 0) {
        // Last day takes whatever is remaining to ensure exact match
        scansForDay = totalScans - currentSum;
      } else {
        scansForDay = Math.round((weights[days - 1 - i] / totalWeight) * totalScans);
      }
      
      currentSum += scansForDay;
      
      data.push({
        date: date.toISOString().split('T')[0],
        scans: scansForDay,
      });
    }
    return data;
  }, [totalScans]);

  return (
    <div className="w-full h-72 pt-4">
      <ChartContainer config={chartConfig} className="h-full w-full">
        <AreaChart
          accessibilityLayer
          data={chartData}
          margin={{
            left: -20,
            right: 12,
            top: 12,
            bottom: 0,
          }}
        >
          <CartesianGrid vertical={false} strokeDasharray="3 3" className="stroke-muted" />
          <XAxis
            dataKey="date"
            tickLine={false}
            axisLine={false}
            tickMargin={8}
            tickFormatter={(value) => {
              const date = new Date(value);
              return date.toLocaleDateString("en-US", { month: "short", day: "numeric" });
            }}
            className="text-xs text-muted-foreground"
          />
          <YAxis 
            tickLine={false}
            axisLine={false}
            tickMargin={8}
            className="text-xs text-muted-foreground"
            allowDecimals={false}
          />
          <ChartTooltip cursor={false} content={<ChartTooltipContent indicator="line" />} />
          <defs>
            <linearGradient id="fillScans" x1="0" y1="0" x2="0" y2="1">
              <stop offset="5%" stopColor="var(--color-scans)" stopOpacity={0.3} />
              <stop offset="95%" stopColor="var(--color-scans)" stopOpacity={0.0} />
            </linearGradient>
          </defs>
          <Area
            dataKey="scans"
            type="monotone"
            fill="url(#fillScans)"
            fillOpacity={0.4}
            stroke="var(--color-scans)"
            strokeWidth={2}
          />
        </AreaChart>
      </ChartContainer>
    </div>
  );
}
