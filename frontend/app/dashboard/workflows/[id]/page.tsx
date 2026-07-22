"use client";

import React, { use } from "react";
import Link from "next/link";
import {
  Zap,
  Play,
  Edit,
  ArrowLeft,
  CheckCircle2,
  XCircle,
  Clock,
  Sparkles,
  FileClock,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { useWorkflow, useExecuteWorkflowManual, useWorkflowExecutions } from "@/hooks/use-workflows";

export default function WorkflowDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const { id } = use(params);
  const { data: workflow, isLoading } = useWorkflow(id);
  const { data: executionsData } = useWorkflowExecutions(1, 20);
  const executeManual = useExecuteWorkflowManual();

  if (isLoading) {
    return <div className="p-12 text-center text-sm text-muted-foreground">Loading workflow details...</div>;
  }

  if (!workflow) {
    return <div className="p-12 text-center text-sm text-muted-foreground">Workflow not found</div>;
  }

  const executions = executionsData?.items || [];

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8">
      {/* Back button & Header */}
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Link
            href="/dashboard/workflows"
            className="flex h-9 w-9 items-center justify-center rounded-xl border border-border/50 text-muted-foreground transition-colors hover:bg-muted hover:text-foreground"
          >
            <ArrowLeft className="h-4 w-4" />
          </Link>
          <div>
            <div className="flex items-center gap-2">
              <h1 className="text-2xl font-bold text-foreground">{workflow.name}</h1>
              <span
                className={`inline-flex items-center gap-1 rounded-full px-2.5 py-0.5 text-xs font-semibold ${
                  workflow.status === "Active"
                    ? "bg-emerald-500/10 text-emerald-500"
                    : "bg-muted text-muted-foreground"
                }`}
              >
                {workflow.status}
              </span>
            </div>
            {workflow.description && (
              <p className="mt-1 text-sm text-muted-foreground">{workflow.description}</p>
            )}
          </div>
        </div>

        <div className="flex items-center gap-3">
          <Button
            variant="outline"
            size="sm"
            onClick={() => executeManual.mutate({ id: workflow.id })}
            className="gap-2"
          >
            <Play className="h-3.5 w-3.5 text-emerald-500" />
            <span>Test Manual Trigger</span>
          </Button>

          <Link href={`/dashboard/workflows/edit/${workflow.id}`}>
            <Button size="sm" className="gap-2">
              <Edit className="h-4 w-4" />
              <span>Edit Builder</span>
            </Button>
          </Link>
        </div>
      </div>

      {/* Grid Specs */}
      <div className="grid gap-6 md:grid-cols-3">
        {/* Trigger Summary */}
        <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm">
          <div className="flex items-center gap-2 text-xs font-bold uppercase tracking-wider text-amber-500 mb-3">
            <Zap className="h-4 w-4" />
            <span>Trigger Configured</span>
          </div>
          <h3 className="text-base font-bold text-foreground">{workflow.trigger?.triggerType || "Manual"}</h3>
          <p className="mt-1 text-xs text-muted-foreground">
            Enabled: {workflow.trigger?.enabled ? "Yes" : "No"}
          </p>
        </div>

        {/* Action Summary */}
        <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm">
          <div className="flex items-center gap-2 text-xs font-bold uppercase tracking-wider text-primary mb-3">
            <CheckCircle2 className="h-4 w-4" />
            <span>Configured Actions</span>
          </div>
          <h3 className="text-base font-bold text-foreground">{workflow.actions?.length || 0} Sequential Steps</h3>
          <div className="mt-2 flex flex-wrap gap-1">
            {workflow.actions?.map((a, idx) => (
              <span key={a.id || idx} className="rounded bg-primary/10 px-2 py-0.5 text-xs font-semibold text-primary">
                {a.actionType}
              </span>
            ))}
          </div>
        </div>

        {/* Version & Date */}
        <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm">
          <div className="flex items-center gap-2 text-xs font-bold uppercase tracking-wider text-muted-foreground mb-3">
            <Clock className="h-4 w-4" />
            <span>Version & Metadata</span>
          </div>
          <h3 className="text-base font-bold text-foreground">Version v{workflow.version}</h3>
          <p className="mt-1 text-xs text-muted-foreground">
            Created: {new Date(workflow.createdAt).toLocaleDateString()}
          </p>
        </div>
      </div>

      {/* Execution Logs Table */}
      <div className="flex flex-col gap-4">
        <div className="flex items-center justify-between">
          <h2 className="text-lg font-bold text-foreground flex items-center gap-2">
            <FileClock className="h-5 w-5 text-primary" />
            Execution History & Logs
          </h2>
        </div>

        <div className="overflow-hidden rounded-2xl border border-border/60 bg-card shadow-sm">
          {executions.length === 0 ? (
            <div className="p-8 text-center text-sm text-muted-foreground">
              No executions logged yet. Click "Test Manual Trigger" to test run.
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left text-sm text-foreground">
                <thead className="border-b border-border/50 bg-muted/40 text-xs uppercase font-semibold text-muted-foreground">
                  <tr>
                    <th className="px-6 py-4">Execution ID</th>
                    <th className="px-6 py-4">Triggered By</th>
                    <th className="px-6 py-4">Started At</th>
                    <th className="px-6 py-4">Duration</th>
                    <th className="px-6 py-4">Status</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-border/40">
                  {executions.map((ex) => (
                    <tr key={ex.id} className="hover:bg-muted/20 transition-colors">
                      <td className="px-6 py-4 font-mono text-xs text-primary">{ex.id.slice(0, 8)}...</td>
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
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
