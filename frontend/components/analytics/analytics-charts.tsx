"use client";

import {
  Area,
  AreaChart,
  Bar,
  BarChart,
  CartesianGrid,
  Cell,
  Pie,
  PieChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import {
  BrowserAnalytics,
  CountryAnalytics,
  DeviceAnalytics,
  ScanTimeline,
} from "@/types/analytics";
import { format, parseISO } from "date-fns";

const COLORS = ["#0ea5e9", "#22c55e", "#f59e0b", "#ec4899", "#8b5cf6", "#64748b"];

interface ChartProps {
  isLoading?: boolean;
}

export function ScanTrendChart({ data, isLoading }: { data?: ScanTimeline[] } & ChartProps) {
  if (isLoading) {
    return <Skeleton className="h-[300px] w-full rounded-xl" />;
  }

  const formattedData = data?.map((d) => ({
    ...d,
    formattedDate: format(parseISO(d.date), "MMM dd"),
  })) || [];

  return (
    <Card className="col-span-full">
      <CardHeader>
        <CardTitle>Scan Trend</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[300px] w-full">
          {formattedData.length > 0 ? (
            <ResponsiveContainer width="100%" height="100%">
              <AreaChart data={formattedData} margin={{ top: 10, right: 10, left: -20, bottom: 0 }}>
                <defs>
                  <linearGradient id="colorScans" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#0ea5e9" stopOpacity={0.3} />
                    <stop offset="95%" stopColor="#0ea5e9" stopOpacity={0} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#e5e7eb" />
                <XAxis dataKey="formattedDate" tickLine={false} axisLine={false} tick={{ fontSize: 12, fill: '#6b7280' }} />
                <YAxis tickLine={false} axisLine={false} tick={{ fontSize: 12, fill: '#6b7280' }} />
                <Tooltip
                  contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}
                  labelStyle={{ fontWeight: 'bold', color: '#111827', marginBottom: '4px' }}
                />
                <Area type="monotone" dataKey="scanCount" name="Scans" stroke="#0ea5e9" strokeWidth={3} fillOpacity={1} fill="url(#colorScans)" />
              </AreaChart>
            </ResponsiveContainer>
          ) : (
            <div className="flex h-full items-center justify-center text-sm text-muted-foreground">
              No data available for this period.
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
}

export function DeviceDistributionChart({ data, isLoading }: { data?: DeviceAnalytics[] } & ChartProps) {
  if (isLoading) return <Skeleton className="h-[300px] w-full rounded-xl" />;

  return (
    <Card>
      <CardHeader>
        <CardTitle>Devices</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[250px] w-full">
          {data && data.length > 0 ? (
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={data}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={80}
                  paddingAngle={5}
                  dataKey="count"
                  nameKey="deviceType"
                >
                  {data.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip
                  // eslint-disable-next-line @typescript-eslint/no-explicit-any
                  formatter={(value: any, name: any, props: any) => [
                    `${value} (${props.payload.percentage.toFixed(1)}%)`,
                    name
                  ]}
                  contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}
                />
              </PieChart>
            </ResponsiveContainer>
          ) : (
            <div className="flex h-full items-center justify-center text-sm text-muted-foreground">
              No data available.
            </div>
          )}
        </div>
        {data && data.length > 0 && (
          <div className="mt-4 flex flex-wrap justify-center gap-4 text-sm">
            {data.map((entry, idx) => (
              <div key={entry.deviceType} className="flex items-center gap-1.5">
                <div className="h-3 w-3 rounded-full" style={{ backgroundColor: COLORS[idx % COLORS.length] }} />
                <span className="text-muted-foreground">{entry.deviceType}</span>
              </div>
            ))}
          </div>
        )}
      </CardContent>
    </Card>
  );
}

export function BrowserUsageChart({ data, isLoading }: { data?: BrowserAnalytics[] } & ChartProps) {
  if (isLoading) return <Skeleton className="h-[300px] w-full rounded-xl" />;

  const chartData = data?.slice(0, 5) || []; // Top 5

  return (
    <Card>
      <CardHeader>
        <CardTitle>Top Browsers</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[250px] w-full">
          {chartData.length > 0 ? (
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={chartData} layout="vertical" margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>
                <CartesianGrid strokeDasharray="3 3" horizontal={false} stroke="#e5e7eb" />
                <XAxis type="number" hide />
                <YAxis dataKey="browser" type="category" axisLine={false} tickLine={false} tick={{ fontSize: 12, fill: '#4b5563' }} width={80} />
                <Tooltip
                  cursor={{ fill: '#f3f4f6' }}
                  contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}
                />
                <Bar dataKey="count" name="Scans" fill="#8b5cf6" radius={[0, 4, 4, 0]} barSize={20} />
              </BarChart>
            </ResponsiveContainer>
          ) : (
            <div className="flex h-full items-center justify-center text-sm text-muted-foreground">
              No data available.
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
}

export function LocationChart({ data, isLoading }: { data?: CountryAnalytics[] } & ChartProps) {
  if (isLoading) return <Skeleton className="h-[300px] w-full rounded-xl" />;

  const chartData = data?.slice(0, 5) || []; // Top 5

  return (
    <Card>
      <CardHeader>
        <CardTitle>Top Countries</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[250px] w-full">
          {chartData.length > 0 ? (
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={chartData} layout="vertical" margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>
                <CartesianGrid strokeDasharray="3 3" horizontal={false} stroke="#e5e7eb" />
                <XAxis type="number" hide />
                <YAxis dataKey="country" type="category" axisLine={false} tickLine={false} tick={{ fontSize: 12, fill: '#4b5563' }} width={80} />
                <Tooltip
                  cursor={{ fill: '#f3f4f6' }}
                  contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}
                />
                <Bar dataKey="count" name="Scans" fill="#f59e0b" radius={[0, 4, 4, 0]} barSize={20} />
              </BarChart>
            </ResponsiveContainer>
          ) : (
            <div className="flex h-full items-center justify-center text-sm text-muted-foreground">
              No data available.
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
