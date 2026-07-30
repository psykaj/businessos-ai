"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { ResponsiveContainer, ComposedChart, Line, Area, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from "recharts";

interface ForecastChartProps {
  title: string;
  description?: string;
  data: any[];
  xKey: string;
  actualKey: string;
  forecastKey: string;
  lowerBoundKey: string;
  upperBoundKey: string;
}

export function ForecastChart({ 
  title, 
  description, 
  data, 
  xKey, 
  actualKey, 
  forecastKey,
  lowerBoundKey,
  upperBoundKey
}: ForecastChartProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{title}</CardTitle>
        {description && <CardDescription>{description}</CardDescription>}
      </CardHeader>
      <CardContent>
        <div className="h-[350px] w-full">
          <ResponsiveContainer width="100%" height="100%">
            <ComposedChart data={data} margin={{ top: 20, right: 20, bottom: 20, left: 20 }}>
              <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="hsl(var(--muted-foreground)/0.2)" />
              <XAxis dataKey={xKey} tick={{ fontSize: 12, fill: "hsl(var(--muted-foreground))" }} dy={10} />
              <YAxis tick={{ fontSize: 12, fill: "hsl(var(--muted-foreground))" }} dx={-10} />
              <Tooltip 
                contentStyle={{ borderRadius: '8px', border: '1px solid hsl(var(--border))', backgroundColor: 'hsl(var(--background))' }} 
              />
              <Legend wrapperStyle={{ paddingTop: '20px' }} />
              
              {/* Confidence Interval (Area) */}
              <Area 
                type="monotone" 
                dataKey={upperBoundKey} 
                stroke="none" 
                fill="#8b5cf6" 
                fillOpacity={0.1} 
                name="Confidence Range"
              />
              <Area 
                type="monotone" 
                dataKey={lowerBoundKey} 
                stroke="none" 
                fill="hsl(var(--background))" 
                fillOpacity={1} 
                name=""
                legendType="none"
              />
              
              {/* Actual Line */}
              <Line 
                type="monotone" 
                dataKey={actualKey} 
                stroke="#3b82f6" 
                strokeWidth={3} 
                dot={{ r: 4 }} 
                activeDot={{ r: 6 }} 
                name="Actual" 
              />
              
              {/* Forecast Line */}
              <Line 
                type="monotone" 
                dataKey={forecastKey} 
                stroke="#8b5cf6" 
                strokeWidth={3} 
                strokeDasharray="5 5" 
                dot={{ r: 4 }} 
                activeDot={{ r: 6 }} 
                name="Forecast" 
              />
            </ComposedChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  );
}
