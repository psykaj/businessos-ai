import {
  QrCode,
  Users,
  DollarSign,
  Star,
  Megaphone,
  Eye,
} from "lucide-react";
import { MetricCard } from "@/components/dashboard/metric-card";
import type { MetricCardData } from "@/types/navigation";

const metrics: MetricCardData[] = [
  {
    title: "Total QR Codes",
    value: "1,247",
    change: "+12.5%",
    trend: "up",
    icon: QrCode,
  },
  {
    title: "Customers",
    value: "3,842",
    change: "+8.2%",
    trend: "up",
    icon: Users,
  },
  {
    title: "Revenue",
    value: "$48,295",
    change: "+23.1%",
    trend: "up",
    icon: DollarSign,
  },
  {
    title: "Reviews",
    value: "956",
    change: "+15.3%",
    trend: "up",
    icon: Star,
  },
  {
    title: "Campaigns",
    value: "24",
    change: "+4.5%",
    trend: "up",
    icon: Megaphone,
  },
  {
    title: "Monthly Visits",
    value: "28,403",
    change: "+18.7%",
    trend: "up",
    icon: Eye,
  },
];

export function MetricsGrid() {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      {metrics.map((metric) => (
        <MetricCard key={metric.title} data={metric} />
      ))}
    </div>
  );
}
