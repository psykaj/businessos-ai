"use client";

import { useQuery } from "@tanstack/react-query";
import { analyticsService } from "@/lib/analytics-service";
import { useSearchParams } from "next/navigation";
import { useState } from "react";
import { DateRangePicker } from "@/components/analytics/date-range-picker";
import { KPICards } from "@/components/analytics/kpi-cards";
import {
  ScanTrendChart,
  DeviceDistributionChart,
  BrowserUsageChart,
  LocationChart,
} from "@/components/analytics/analytics-charts";
import { ScanHistoryTable } from "@/components/analytics/scan-history-table";
import { Button } from "@/components/ui/button";
import { Download, RefreshCw, BarChart3 } from "lucide-react";
import { toast } from "sonner";
import { useAuth } from "@/contexts/auth-context";
import { cn } from "@/lib/utils";

export default function AnalyticsPage() {
  const searchParams = useSearchParams();
  const startDate = searchParams.get("startDate") || undefined;
  const endDate = searchParams.get("endDate") || undefined;
  
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const { user } = useAuth();

  // Queries
  const { data: overview, isLoading: overviewLoading, refetch: refetchOverview } = useQuery({
    queryKey: ["analytics-overview", startDate, endDate],
    queryFn: () => analyticsService.getOverview(startDate, endDate),
    enabled: !!user,
  });

  const { data: timeline, isLoading: timelineLoading, refetch: refetchTimeline } = useQuery({
    queryKey: ["analytics-timeline", startDate, endDate],
    queryFn: () => analyticsService.getTimeline(startDate, endDate),
    enabled: !!user,
  });

  const { data: devices, isLoading: devicesLoading, refetch: refetchDevices } = useQuery({
    queryKey: ["analytics-devices", startDate, endDate],
    queryFn: () => analyticsService.getDevices(startDate, endDate),
    enabled: !!user,
  });

  const { data: browsers, isLoading: browsersLoading, refetch: refetchBrowsers } = useQuery({
    queryKey: ["analytics-browsers", startDate, endDate],
    queryFn: () => analyticsService.getBrowsers(startDate, endDate),
    enabled: !!user,
  });

  const { data: countries, isLoading: countriesLoading, refetch: refetchCountries } = useQuery({
    queryKey: ["analytics-countries", startDate, endDate],
    queryFn: () => analyticsService.getCountries(startDate, endDate),
    enabled: !!user,
  });

  const { data: history, isLoading: historyLoading, refetch: refetchHistory } = useQuery({
    queryKey: ["analytics-history", search, page, startDate, endDate],
    queryFn: () => analyticsService.getHistory(undefined, search, page, 10, startDate, endDate),
    enabled: !!user,
  });

  const handleRefresh = () => {
    refetchOverview();
    refetchTimeline();
    refetchDevices();
    refetchBrowsers();
    refetchCountries();
    refetchHistory();
    toast.success("Dashboard refreshed");
  };

  const handleExport = async () => {
    try {
      const response = await analyticsService.exportCSV(startDate, endDate);
      const url = window.URL.createObjectURL(new Blob([response as any]));
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", `analytics_export_${new Date().toISOString().split("T")[0]}.csv`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      toast.success("Export downloaded successfully");
    } catch (error) {
      toast.error("Failed to export analytics");
    }
  };

  return (
    <div className="relative min-h-full p-6 sm:p-10 max-w-[1400px] mx-auto space-y-10 overflow-hidden">
      {/* Background Decor */}
      <div className="absolute top-0 right-10 w-[600px] h-[600px] bg-primary/10 rounded-full blur-[120px] -z-10" />
      <div className="absolute bottom-0 left-10 w-[400px] h-[400px] bg-blue-500/5 rounded-full blur-[100px] -z-10" />

      {/* Header */}
      <div className="flex flex-col gap-6 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <h1 className="text-4xl font-extrabold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-foreground to-foreground/70 flex items-center gap-3">
            <div className="p-3 bg-primary/10 rounded-xl border border-primary/20">
              <BarChart3 className="w-8 h-8 text-primary" />
            </div>
            Analytics Intelligence
          </h1>
          <p className="mt-4 text-lg text-muted-foreground max-w-2xl">
            Track performance metrics, scan distributions, and global engagement in real-time.
          </p>
        </div>
        
        <div className="flex flex-wrap items-center gap-3 bg-background/50 backdrop-blur-xl p-2 rounded-2xl border border-border/50 shadow-sm">
          <DateRangePicker />
          <Button variant="outline" size="icon" onClick={handleRefresh} title="Refresh" className="rounded-xl border-border/50 hover:bg-muted/80 h-10 w-10">
            <RefreshCw className="h-4 w-4 text-muted-foreground" />
          </Button>
          <Button onClick={handleExport} className="gap-2 rounded-xl h-10 px-5 shadow-lg shadow-primary/20 hover:-translate-y-0.5 transition-transform">
            <Download className="h-4 w-4" /> Export CSV
          </Button>
        </div>
      </div>

      {/* KPI Cards Section */}
      <div className="relative z-10">
        <KPICards overview={overview} isLoading={overviewLoading} />
      </div>

      {/* Main Charts Area */}
      <div className="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
        <div className="col-span-full lg:col-span-2 rounded-3xl border border-border/50 bg-background/60 backdrop-blur-xl shadow-xl overflow-hidden p-2 transition-all hover:shadow-primary/5">
          <ScanTrendChart data={timeline} isLoading={timelineLoading} />
        </div>
        <div className="col-span-1 rounded-3xl border border-border/50 bg-background/60 backdrop-blur-xl shadow-xl overflow-hidden p-2 transition-all hover:shadow-primary/5">
          <DeviceDistributionChart data={devices} isLoading={devicesLoading} />
        </div>
      </div>

      {/* Secondary Charts Area */}
      <div className="grid gap-8 md:grid-cols-2">
        <div className="rounded-3xl border border-border/50 bg-background/60 backdrop-blur-xl shadow-xl overflow-hidden p-2 transition-all hover:shadow-primary/5">
          <BrowserUsageChart data={browsers} isLoading={browsersLoading} />
        </div>
        <div className="rounded-3xl border border-border/50 bg-background/60 backdrop-blur-xl shadow-xl overflow-hidden p-2 transition-all hover:shadow-primary/5">
          <LocationChart data={countries} isLoading={countriesLoading} />
        </div>
      </div>

      {/* Data Table */}
      <div className="rounded-3xl border border-border/50 bg-background/60 backdrop-blur-xl shadow-xl overflow-hidden p-6">
        <h3 className="text-xl font-bold mb-6 flex items-center gap-2">
          Scan History
        </h3>
        <ScanHistoryTable 
          data={history} 
          isLoading={historyLoading}
          search={search}
          onSearchChange={setSearch}
          page={page}
          onPageChange={setPage}
        />
      </div>
    </div>
  );
}
