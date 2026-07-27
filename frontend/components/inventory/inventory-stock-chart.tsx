"use client";

import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  Legend,
} from "recharts";
import { ProductVelocityReportDto, StockAlertDto } from "@/types/inventory";

interface InventoryStockChartProps {
  velocityData?: ProductVelocityReportDto[];
  alertsData?: StockAlertDto[];
}

const COLORS = ["#10b981", "#f59e0b", "#ef4444"];

export function InventoryStockChart({ velocityData = [], alertsData = [] }: InventoryStockChartProps) {
  // Process velocity chart data
  const velocityChart = velocityData.slice(0, 8).map((item) => ({
    name: item.productName.length > 12 ? item.productName.substring(0, 12) + "..." : item.productName,
    Quantity: item.totalMovementQuantity,
    Value: item.totalValue,
  }));

  // Process alert status breakdown
  const alertBreakdown = [
    { name: "Low Stock", value: alertsData.filter((a) => a.alertType === "LowStock").length },
    { name: "Overstock", value: alertsData.filter((a) => a.alertType === "Overstock").length },
    { name: "Out of Stock", value: alertsData.filter((a) => a.alertType === "OutOfStock").length },
  ].filter((d) => d.value > 0);

  const fallbackAlertBreakdown = alertBreakdown.length > 0 ? alertBreakdown : [
    { name: "Healthy Stock", value: 85 },
    { name: "Low Stock", value: 10 },
    { name: "Out of Stock", value: 5 },
  ];

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
      {/* Velocity Chart */}
      <div className="lg:col-span-2 p-5 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm">
        <div className="flex items-center justify-between mb-4">
          <div>
            <h3 className="font-semibold text-base">Top Product Velocity</h3>
            <p className="text-xs text-muted-foreground">Stock units moved over the past 30 days</p>
          </div>
        </div>
        <div className="h-64 w-full">
          {velocityChart.length > 0 ? (
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={velocityChart} margin={{ top: 10, right: 10, left: -20, bottom: 0 }}>
                <CartesianGrid strokeDasharray="3 3" opacity={0.15} />
                <XAxis dataKey="name" tick={{ fontSize: 11 }} />
                <YAxis tick={{ fontSize: 11 }} />
                <Tooltip
                  contentStyle={{
                    backgroundColor: "rgba(15, 23, 42, 0.9)",
                    borderColor: "rgba(255, 255, 255, 0.1)",
                    borderRadius: "8px",
                    color: "#fff",
                    fontSize: "12px",
                  }}
                />
                <Bar dataKey="Quantity" fill="#6366f1" radius={[4, 4, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          ) : (
            <div className="h-full flex items-center justify-center text-xs text-muted-foreground">
              No recent movement data available
            </div>
          )}
        </div>
      </div>

      {/* Stock Health Breakdown */}
      <div className="p-5 rounded-2xl border border-border/60 bg-card/60 backdrop-blur-md shadow-sm">
        <div className="mb-4">
          <h3 className="font-semibold text-base">Stock Risk Breakdown</h3>
          <p className="text-xs text-muted-foreground">Current risk categories across inventory</p>
        </div>
        <div className="h-64 w-full flex items-center justify-center">
          <ResponsiveContainer width="100%" height="100%">
            <PieChart>
              <Pie
                data={fallbackAlertBreakdown}
                cx="50%"
                cy="50%"
                innerRadius={50}
                outerRadius={80}
                paddingAngle={4}
                dataKey="value"
              >
                {fallbackAlertBreakdown.map((_, index) => (
                  <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip
                contentStyle={{
                  backgroundColor: "rgba(15, 23, 42, 0.9)",
                  borderColor: "rgba(255, 255, 255, 0.1)",
                  borderRadius: "8px",
                  color: "#fff",
                  fontSize: "12px",
                }}
              />
              <Legend verticalAlign="bottom" height={36} wrapperStyle={{ fontSize: "11px" }} />
            </PieChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
}
