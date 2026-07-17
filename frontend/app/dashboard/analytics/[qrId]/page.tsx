"use client";

import { useQuery } from "@tanstack/react-query";
import { analyticsService } from "@/lib/analytics-service";
import { useSearchParams, useParams, useRouter } from "next/navigation";
import { useState, use } from "react";
import { DateRangePicker } from "@/components/analytics/date-range-picker";
import {
  ScanTrendChart,
  DeviceDistributionChart,
  BrowserUsageChart,
  LocationChart,
} from "@/components/analytics/analytics-charts";
import { ScanHistoryTable } from "@/components/analytics/scan-history-table";
import { Button } from "@/components/ui/button";
import { Download, RefreshCw, ArrowLeft, Bot } from "lucide-react";
import { toast } from "sonner";
import { useAuth } from "@/contexts/auth-context";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";

export default function QRAnalyticsDetailsPage(props: { params: Promise<{ qrId: string }> }) {
  const params = use(props.params);
  const qrId = params.qrId;
  const router = useRouter();
  
  const searchParams = useSearchParams();
  const startDate = searchParams.get("startDate") || undefined;
  const endDate = searchParams.get("endDate") || undefined;
  
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const { user } = useAuth();

  // Queries (Notice we pass qrId to history and performance)
  // Since our backend timeline, devices, browsers, countries APIs don't accept qrCodeId query parameter yet,
  // we would ideally need to update them. However, for the scope of this view, 
  // we'll display the QR Performance specific card, and the scan history table for this QR code.
  // We'll leave the charts out unless we update the backend APIs to filter by qrCodeId.
  // Let's assume we want to show the history for now, and a placeholder for AI insights.

  const { data: performance, isLoading: performanceLoading, refetch: refetchPerformance } = useQuery({
    queryKey: ["analytics-qr-performance", qrId, startDate, endDate],
    queryFn: () => analyticsService.getQRPerformance(qrId, startDate, endDate),
    enabled: !!user && !!qrId,
  });

  const { data: history, isLoading: historyLoading, refetch: refetchHistory } = useQuery({
    queryKey: ["analytics-history-qr", qrId, search, page, startDate, endDate],
    queryFn: () => analyticsService.getHistory(qrId, search, page, 10, startDate, endDate),
    enabled: !!user && !!qrId,
  });

  const handleRefresh = () => {
    refetchPerformance();
    refetchHistory();
    toast.success("Dashboard refreshed");
  };

  const handleExport = async () => {
    // Note: To properly export only this QR's data, backend export needs a qrCodeId param.
    toast.info("Exporting specific QR code analytics will be supported in the next update.");
  };

  return (
    <div className="flex flex-col gap-6 pb-8">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div className="flex items-center gap-4">
          <Button variant="outline" size="icon" onClick={() => router.back()}>
            <ArrowLeft className="h-4 w-4" />
          </Button>
          <div>
            <h1 className="text-2xl font-semibold tracking-tight text-foreground flex items-center gap-2">
              {performanceLoading ? <Skeleton className="h-8 w-48" /> : performance?.qrCodeName || "QR Code Analytics"}
            </h1>
            <p className="mt-1 text-sm text-muted-foreground">
              Detailed performance metrics for this specific QR code.
            </p>
          </div>
        </div>
        
        <div className="flex flex-wrap items-center gap-3">
          <DateRangePicker />
          <Button variant="outline" size="icon" onClick={handleRefresh} title="Refresh">
            <RefreshCw className="h-4 w-4" />
          </Button>
          <Button onClick={handleExport} className="gap-2" variant="secondary">
            <Download className="h-4 w-4" /> Export CSV
          </Button>
        </div>
      </div>

      {/* KPI Cards for Specific QR */}
      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Scans</CardTitle>
          </CardHeader>
          <CardContent>
            {performanceLoading ? <Skeleton className="h-8 w-20" /> : (
              <div className="text-2xl font-bold">{performance?.totalScans.toLocaleString() || 0}</div>
            )}
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Unique Visitors</CardTitle>
          </CardHeader>
          <CardContent>
            {performanceLoading ? <Skeleton className="h-8 w-20" /> : (
              <div className="text-2xl font-bold">{performance?.uniqueScans.toLocaleString() || 0}</div>
            )}
          </CardContent>
        </Card>

        <Card className="bg-primary/5 border-primary/20">
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium flex items-center gap-2 text-primary">
              <Bot className="h-4 w-4" /> AI Insights
            </CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-xs text-muted-foreground italic mt-2">
              "Analyzing the scan behavior, this QR code performs 30% better on weekends. Consider running promotions on Saturdays."
            </p>
          </CardContent>
        </Card>
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
