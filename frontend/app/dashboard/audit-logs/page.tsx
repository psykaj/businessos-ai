"use client";

import { useState } from "react";
import { useAuth } from "@/contexts/auth-context";
import { useAuditLogs } from "@/hooks/use-auditlogs";
import { AuditLog } from "@/types/auditlogs";
import { format } from "date-fns";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { DataTable } from "@/components/ui/data-table";
import { Input } from "@/components/ui/input";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Badge } from "@/components/ui/badge";
import { Search } from "lucide-react";

export default function AuditLogsPage() {
  const { user } = useAuth();
  const orgId = user?.organizationId;
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [module, setModule] = useState("All");

  const { data, isLoading } = useAuditLogs(orgId, {
    page,
    pageSize: 15,
    search: search || undefined,
    module: module !== "All" ? module : undefined,
  });

  const columns = [
    {
      header: "Action",
      cell: (item: AuditLog) => (
        <div className="flex flex-col gap-1">
          <span className="font-medium">{item.action}</span>
          <span className="text-xs text-muted-foreground">{item.description}</span>
        </div>
      )
    },
    {
      header: "Module",
      cell: (item: AuditLog) => <Badge variant="secondary">{item.module}</Badge>
    },
    {
      header: "User",
      cell: (item: AuditLog) => (
        <div className="flex flex-col">
          <span className="text-sm font-medium">{item.userEmail}</span>
          {item.ipAddress && <span className="text-xs text-muted-foreground font-mono">{item.ipAddress}</span>}
        </div>
      )
    },
    {
      header: "Date & Time",
      cell: (item: AuditLog) => (
        <span className="text-sm whitespace-nowrap">
          {format(new Date(item.timestamp), "MMM d, yyyy HH:mm:ss")}
        </span>
      )
    }
  ];

  return (
    <div className="mx-auto max-w-6xl space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight text-foreground">Audit Logs</h1>
        <p className="mt-1 text-sm text-muted-foreground">Comprehensive record of all significant actions in your organization.</p>
      </div>

      <Card>
        <CardHeader className="pb-4">
          <CardTitle>Activity History</CardTitle>
          <CardDescription>Track what happened, who did it, and when.</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center">
            <div className="relative flex-1">
              <Search className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
              <Input
                placeholder="Search by user email, action, or description..."
                className="pl-9"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
              />
            </div>
            <div className="w-full sm:w-[200px]">
              <Select value={module} onValueChange={(val) => setModule(val || "All")}>
                <SelectTrigger>
                  <SelectValue placeholder="Filter by Module" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="All">All Modules</SelectItem>
                  <SelectItem value="Authentication">Authentication</SelectItem>
                  <SelectItem value="Team">Team</SelectItem>
                  <SelectItem value="API Keys">API Keys</SelectItem>
                  <SelectItem value="Billing">Billing</SelectItem>
                  <SelectItem value="QR Codes">QR Codes</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          <DataTable
            data={data?.items || []}
            columns={columns}
            page={data?.page}
            totalPages={data?.totalPages}
            onPageChange={setPage}
            isLoading={isLoading}
          />
        </CardContent>
      </Card>
    </div>
  );
}
