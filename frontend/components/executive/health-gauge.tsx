"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { ResponsiveContainer, PieChart, Pie, Cell, Tooltip } from "recharts";

interface HealthGaugeProps {
  score: number;
  title?: string;
  description?: string;
}

export function HealthGauge({ score, title = "Business Health", description }: HealthGaugeProps) {
  const remaining = 100 - score;
  const data = [
    { name: "Score", value: score },
    { name: "Remaining", value: remaining },
  ];

  const getColor = (s: number) => {
    if (s >= 85) return "#10b981"; // emerald-500
    if (s >= 70) return "#3b82f6"; // blue-500
    if (s >= 50) return "#f59e0b"; // amber-500
    return "#ef4444"; // red-500
  };

  const color = getColor(score);
  
  return (
    <Card className="flex flex-col items-center justify-center">
      <CardHeader className="text-center pb-0">
        <CardTitle>{title}</CardTitle>
        {description && <CardDescription>{description}</CardDescription>}
      </CardHeader>
      <CardContent className="relative flex items-center justify-center pt-6">
        <div className="h-[200px] w-[200px]">
          <ResponsiveContainer width="100%" height="100%">
            <PieChart>
              <Pie
                data={data}
                cx="50%"
                cy="50%"
                startAngle={180}
                endAngle={0}
                innerRadius={70}
                outerRadius={90}
                paddingAngle={0}
                dataKey="value"
                stroke="none"
              >
                <Cell key="cell-0" fill={color} />
                <Cell key="cell-1" fill="hsl(var(--muted))" />
              </Pie>
            </PieChart>
          </ResponsiveContainer>
        </div>
        <div className="absolute top-[55%] left-1/2 -translate-x-1/2 -translate-y-1/2 flex flex-col items-center">
          <span className="text-4xl font-bold" style={{ color }}>{score.toFixed(0)}</span>
          <span className="text-xs text-muted-foreground mt-1">/ 100</span>
        </div>
      </CardContent>
    </Card>
  );
}
