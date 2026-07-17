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
import { Download, RefreshCw } from "lucide-react";
import { toast } from "sonner";
import { useAuth } from "@/contexts/auth-context";

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
    <div className="flex flex-col gap-6 pb-8">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight text-foreground">
            Analytics Dashboard
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Track performance metrics and scan intelligence.
          </p>
        </div>
        
        <div className="flex flex-wrap items-center gap-3">
          <DateRangePicker />
          <Button variant="outline" size="icon" onClick={handleRefresh} title="Refresh">
            <RefreshCw className="h-4 w-4" />
          </Button>
          <Button onClick={handleExport} className="gap-2">
            <Download className="h-4 w-4" /> Export CSV
          </Button>
        </div>
      </div>

      {/* KPI Cards */}
      <KPICards overview={overview} isLoading={overviewLoading} />

      {/* Main Charts Area */}
      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <div className="col-span-full lg:col-span-2">
          <ScanTrendChart data={timeline} isLoading={timelineLoading} />
        </div>
        <div className="col-span-1">
          <DeviceDistributionChart data={devices} isLoading={devicesLoading} />
        </div>
      </div>

      {/* Secondary Charts Area */}
      <div className="grid gap-6 md:grid-cols-2">
        <BrowserUsageChart data={browsers} isLoading={browsersLoading} />
        <LocationChart data={countries} isLoading={countriesLoading} />
      </div>

      {/* Data Table */}
      <ScanHistoryTable 
        data={history} 
        isLoading={historyLoading}
        search={search}
        onSearchChange={setSearch}
        page={page}
        onPageChange={setPage}
      />
    </div>
  );
}
