"use client";

import React, { useState } from "react";
import {
  FileClock,
  Search,
  Filter,
  RefreshCw,
  CheckCircle2,
  XCircle,
  Clock,
  Play,
  RotateCcw,
  Eye,
  X
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { useWorkflowExecutions, useWorkflowExecutionLogs } from "@/hooks/use-workflows";

export default function WorkflowHistoryPage() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState<string>("");
  const [selectedExecutionId, setSelectedExecutionId] = useState<string | null>(null);

  const { data, isLoading, refetch } = useWorkflowExecutions(page, 20, statusFilter || undefined);
  const { data: logs } = useWorkflowExecutionLogs(selectedExecutionId || "");

  const executions = data?.items || [];
  const totalCount = data?.totalCount || 0;

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8">
      {/* Header */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <div className="flex items-center gap-2">
            <h1 className="text-2xl font-bold tracking-tight text-foreground lg:text-3xl">
              Execution Monitoring & Audit History
            </h1>
            <span className="inline-flex items-center gap-1 rounded-full bg-primary/10 px-2.5 py-0.5 text-xs font-semibold text-primary">
              <RefreshCw className="h-3.5 w-3.5" />
              Real-time Logs
            </span>
          </div>
          <p className="mt-1 text-sm text-muted-foreground">
            Track workflow runs, execution duration, error stack traces, and step-by-step logs.
          </p>
        </div>

        <div className="flex items-center gap-3">
          <Button variant="outline" size="sm" onClick={() => refetch()} className="gap-2">
            <RefreshCw className="h-4 w-4" />
            <span>Refresh Logs</span>
          </Button>
        </div>
      </div>

      {/* Filter bar */}
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-3">
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="rounded-xl border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 shadow-sm"
          >
            <option value="">All Statuses</option>
            <option value="Completed">Completed</option>
            <option value="Running">Running</option>
            <option value="Failed">Failed</option>
            <option value="Retrying">Retrying</option>
          </select>
        </div>
      </div>

      {/* Table */}
      <div className="overflow-hidden rounded-2xl border border-border/60 bg-card shadow-sm">
        {isLoading ? (
          <div className="p-12 text-center text-sm text-muted-foreground">Loading execution logs...</div>
        ) : executions.length === 0 ? (
          <div className="p-12 text-center text-sm text-muted-foreground">No execution history recorded yet.</div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full text-left text-sm text-foreground">
              <thead className="border-b border-border/50 bg-muted/40 text-xs uppercase font-semibold text-muted-foreground">
                <tr>
                  <th className="px-6 py-4">Execution ID</th>
                  <th className="px-6 py-4">Workflow ID</th>
                  <th className="px-6 py-4">Executed By</th>
                  <th className="px-6 py-4">Started At</th>
                  <th className="px-6 py-4">Duration</th>
                  <th className="px-6 py-4">Status</th>
                  <th className="px-6 py-4 text-right">Logs</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-border/40">
                {executions.map((ex) => (
                  <tr key={ex.id} className="hover:bg-muted/20 transition-colors">
                    <td className="px-6 py-4 font-mono text-xs font-bold text-primary">{ex.id.slice(0, 8)}...</td>
                    <td className="px-6 py-4 font-mono text-xs text-muted-foreground">{ex.workflowId.slice(0, 8)}...</td>
                    <td className="px-6 py-4 text-xs font-semibold">{ex.executedBy}</td>
                    <td className="px-6 py-4 text-xs text-muted-foreground">
                      {new Date(ex.startedAt).toLocaleString()}
                    </td>
                    <td className="px-6 py-4 text-xs font-mono">{ex.durationMs} ms</td>
                    <td className="px-6 py-4">
                      <span
                        className={`inline-flex items-center gap-1 rounded-full px-2.5 py-0.5 text-xs font-semibold ${
                          ex.status === "Completed"
                            ? "bg-emerald-500/10 text-emerald-500"
                            : ex.status === "Running"
                            ? "bg-amber-500/10 text-amber-500 animate-pulse"
                            : "bg-rose-500/10 text-rose-500"
                        }`}
                      >
                        {ex.status}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-right">
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => setSelectedExecutionId(ex.id)}
                        className="gap-1.5 text-xs"
                      >
                        <Eye className="h-4 w-4" />
                        <span>View Logs</span>
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* Log Details Modal / Drawer */}
      {selectedExecutionId && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4">
          <div className="w-full max-w-2xl rounded-3xl border border-border bg-background p-6 shadow-2xl flex flex-col gap-4 max-h-[85vh] overflow-hidden">
            <div className="flex items-center justify-between border-b border-border/50 pb-4">
              <div>
                <h3 className="text-base font-bold text-foreground">Step Execution Logs</h3>
                <p className="text-xs font-mono text-muted-foreground">ID: {selectedExecutionId}</p>
              </div>
              <button
                onClick={() => setSelectedExecutionId(null)}
                className="flex h-8 w-8 items-center justify-center rounded-lg text-muted-foreground hover:bg-muted hover:text-foreground"
              >
                <X className="h-4 w-4" />
              </button>
            </div>

            <div className="flex-1 overflow-y-auto flex flex-col gap-3 pr-2">
              {logs && logs.length > 0 ? (
                logs.map((log) => (
                  <div key={log.id} className="rounded-xl border border-border/60 bg-card p-4 flex flex-col gap-2">
                    <div className="flex items-center justify-between">
                      <span className="font-bold text-xs text-foreground">{log.stepName} ({log.stepType})</span>
                      <span
                        className={`rounded-full px-2 py-0.5 text-[10px] font-semibold ${
                          log.status === "Completed"
                            ? "bg-emerald-500/10 text-emerald-500"
                            : "bg-rose-500/10 text-rose-500"
                        }`}
                      >
                        {log.status}
                      </span>
                    </div>

                    <div className="text-[11px] font-mono text-muted-foreground bg-muted/40 p-2 rounded-lg">
                      <div>Input: {log.inputJson}</div>
                      <div>Output: {log.outputJson}</div>
                      {log.errorMessage && <div className="text-rose-500">Error: {log.errorMessage}</div>}
                    </div>
                  </div>
                ))
              ) : (
                <div className="p-8 text-center text-xs text-muted-foreground">
                  No step log records found for this execution.
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
