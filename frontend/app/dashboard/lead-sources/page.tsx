"use client";

import { useMarketingDashboardMetrics } from "@/hooks/use-marketing-analytics";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Search, Filter, Download } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

const MOCK_SOURCES = [
  { source: "Google Ads", leads: 1245, customers: 145, conversion: "11.6%", revenue: 145000, cost: 12000, roi: "1,108%" },
  { source: "Organic Search", leads: 980, customers: 85, conversion: "8.6%", revenue: 85000, cost: 2000, roi: "4,150%" },
  { source: "Direct Traffic", leads: 450, customers: 60, conversion: "13.3%", revenue: 60000, cost: 0, roi: "∞" },
  { source: "Facebook Ads", leads: 320, customers: 12, conversion: "3.7%", revenue: 12000, cost: 4500, roi: "166%" },
  { source: "Referral", leads: 150, customers: 45, conversion: "30.0%", revenue: 45000, cost: 500, roi: "8,900%" },
  { source: "WhatsApp", leads: 85, customers: 15, conversion: "17.6%", revenue: 15000, cost: 150, roi: "9,900%" },
];

export default function LeadSourcesPage() {
  const { data: metrics, isLoading } = useMarketingDashboardMetrics();

  return (
    <div className="flex flex-col gap-6 p-8 h-full">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Lead Source Analytics</h1>
          <p className="text-muted-foreground mt-1">Analyze which channels drive the most revenue and highest ROI.</p>
        </div>
        <Button variant="outline">
          <Download className="mr-2 h-4 w-4" />
          Export Report
        </Button>
      </div>

      <div className="flex items-center gap-2">
        <div className="relative flex-1 max-w-sm">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input placeholder="Search sources..." className="pl-9" />
        </div>
        <Button variant="outline" size="icon">
          <Filter className="h-4 w-4" />
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Performance by Source</CardTitle>
          <CardDescription>Year-to-date marketing attribution</CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Source Channel</TableHead>
                <TableHead className="text-right">Leads</TableHead>
                <TableHead className="text-right">Customers</TableHead>
                <TableHead className="text-right">Conv. Rate</TableHead>
                <TableHead className="text-right">Est. Revenue</TableHead>
                <TableHead className="text-right">Cost</TableHead>
                <TableHead className="text-right">ROI</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {MOCK_SOURCES.map((source) => (
                <TableRow key={source.source}>
                  <TableCell className="font-medium">{source.source}</TableCell>
                  <TableCell className="text-right">{source.leads.toLocaleString()}</TableCell>
                  <TableCell className="text-right">{source.customers.toLocaleString()}</TableCell>
                  <TableCell className="text-right">{source.conversion}</TableCell>
                  <TableCell className="text-right font-medium text-emerald-600">${source.revenue.toLocaleString()}</TableCell>
                  <TableCell className="text-right text-muted-foreground">${source.cost.toLocaleString()}</TableCell>
                  <TableCell className="text-right font-semibold">{source.roi}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
