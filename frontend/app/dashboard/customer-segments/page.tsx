"use client";

import React, { useState } from "react";
import { useCustomerSegments, useCustomerSegmentMembers, useRecalculateSegments } from "@/hooks/use-customer-success";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { HealthScoreBadge } from "@/components/customer-success/health-score-badge";
import { Layers, RefreshCw, Users, Search, Download, Star, Crown, Zap, Clock, ShieldAlert } from "lucide-react";
import { toast } from "sonner";

export default function CustomerSegmentsPage() {
  const [selectedSegment, setSelectedSegment] = useState<string>("VIP Customers");
  const [search, setSearch] = useState<string>("");

  const { data: segments, isLoading: segmentsLoading } = useCustomerSegments();
  const { data: members, isLoading: membersLoading } = useCustomerSegmentMembers(selectedSegment);

  const recalculateMutation = useRecalculateSegments();

  const exportSegmentData = () => {
    toast.success(`Exporting ${selectedSegment} member list (CSV)...`);
  };

  const getSegmentIcon = (name: string) => {
    switch (name.toLowerCase()) {
      case "vip customers":
        return Crown;
      case "high spend customers":
        return Star;
      case "new customers":
        return Zap;
      case "repeat customers":
        return RefreshCw;
      case "inactive customers":
        return Clock;
      case "at-risk customers":
        return ShieldAlert;
      default:
        return Users;
    }
  };

  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      {/* Top Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <Layers className="h-8 w-8 text-purple-600" />
            Customer Segments
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Automated customer grouping into actionable cohorts (VIP, High Spend, New, Inactive, At-Risk).
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button variant="outline" size="sm" onClick={exportSegmentData} className="gap-2">
            <Download className="h-4 w-4" /> Export CSV
          </Button>
          <Button
            size="sm"
            onClick={() => recalculateMutation.mutate()}
            disabled={recalculateMutation.isPending}
            className="gap-2 bg-purple-600 hover:bg-purple-700 text-white"
          >
            <RefreshCw className={`h-4 w-4 ${recalculateMutation.isPending ? "animate-spin" : ""}`} />
            Recalculate Segments
          </Button>
        </div>
      </div>

      {/* Cohort Selector Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
        {segments?.map((seg) => {
          const Icon = getSegmentIcon(seg.name);
          const isSelected = selectedSegment.toLowerCase() === seg.name.toLowerCase();

          return (
            <Card
              key={seg.id}
              onClick={() => setSelectedSegment(seg.name)}
              className={`cursor-pointer transition-all border-slate-200 dark:border-slate-800 hover:border-purple-500/50 ${
                isSelected ? "ring-2 ring-purple-600 bg-purple-500/5" : ""
              }`}
            >
              <CardContent className="p-5 flex items-center justify-between">
                <div className="space-y-1">
                  <p className="text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">{seg.name}</p>
                  <p className="text-2xl font-bold text-slate-900 dark:text-white font-mono">{seg.customerCount}</p>
                </div>
                <div className={`h-10 w-10 rounded-lg flex items-center justify-center ${isSelected ? "bg-purple-600 text-white" : "bg-purple-500/10 text-purple-600"}`}>
                  <Icon className="h-5 w-5" />
                </div>
              </CardContent>
            </Card>
          );
        })}
      </div>

      {/* Selected Segment Member List */}
      <Card className="border-slate-200 dark:border-slate-800">
        <CardHeader className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <CardTitle className="text-base flex items-center gap-2">
              Segment Members: <span className="text-purple-600 dark:text-purple-400 font-bold">{selectedSegment}</span>
            </CardTitle>
            <CardDescription>Filtered customer accounts matching segment criteria</CardDescription>
          </div>
          <div className="relative w-full sm:w-64">
            <Search className="absolute left-3 top-2.5 h-4 w-4 text-slate-400" />
            <Input
              placeholder="Search segment members..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="pl-9 text-sm"
            />
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="border-slate-200 dark:border-slate-800">
                <TableHead>Customer Name</TableHead>
                <TableHead>Email</TableHead>
                <TableHead>Lifetime Value</TableHead>
                <TableHead>Health Score</TableHead>
                <TableHead>Risk Level</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {members?.length ? (
                members
                  .filter((m) => !search || m.customerName.toLowerCase().includes(search.toLowerCase()))
                  .map((m) => (
                    <TableRow key={m.customerId} className="border-slate-100 dark:border-slate-800/60">
                      <TableCell className="font-semibold text-slate-900 dark:text-white">{m.customerName}</TableCell>
                      <TableCell className="text-xs text-slate-500">{m.email}</TableCell>
                      <TableCell className="font-mono font-medium">${m.lifetimeValue.toLocaleString()}</TableCell>
                      <TableCell className="font-mono font-bold">{m.healthScore}/100</TableCell>
                      <TableCell>
                        <HealthScoreBadge score={m.healthScore} riskLevel={m.riskLevel} showScore={false} />
                      </TableCell>
                    </TableRow>
                  ))
              ) : (
                <TableRow>
                  <TableCell colSpan={5} className="text-center py-6 text-slate-500 text-sm">
                    No customers found in segment '{selectedSegment}'.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
