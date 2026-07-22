"use client";

import React, { useState } from "react";
import Link from "next/link";
import {
  Zap,
  Plus,
  Search,
  Filter,
  CheckCircle2,
  XCircle,
  Clock,
  Play,
  MoreVertical,
  Edit,
  Trash2,
  Copy,
  Sparkles,
  TrendingUp,
  ShieldCheck,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { useWorkflows, useDeleteWorkflow, useExecuteWorkflowManual } from "@/hooks/use-workflows";

export default function WorkflowsDashboardPage() {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState<string>("");

  const { data, isLoading } = useWorkflows(page, 20, search, statusFilter || undefined);
  const deleteWorkflow = useDeleteWorkflow();
  const executeManual = useExecuteWorkflowManual();

  const workflows = data?.items || [];
  const totalCount = data?.totalCount || 0;

  // Metric stats calculated dynamically or fallback
  const activeCount = workflows.filter((w) => w.status === "Active").length;
  const estimatedHoursSaved = Math.round(workflows.length * 14.5);

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8">
      {/* Header */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <div className="flex items-center gap-2">
            <h1 className="text-2xl font-bold tracking-tight text-foreground lg:text-3xl">Workflow Automation</h1>
            <span className="inline-flex items-center gap-1 rounded-full bg-primary/10 px-2.5 py-0.5 text-xs font-semibold text-primary">
              <Sparkles className="h-3.5 w-3.5" />
              AI Powered
            </span>
          </div>
          <p className="mt-1 text-sm text-muted-foreground">
            Automate repetitive tasks, CRM updates, customer notifications, and cross-application data flow.
          </p>
        </div>

        <div className="flex items-center gap-3">
          <Link href="/dashboard/templates/workflows">
            <Button variant="outline" className="gap-2">
              <Sparkles className="h-4 w-4 text-amber-500" />
              <span>Browse Templates</span>
            </Button>
          </Link>

          <Link href="/dashboard/workflows/create">
            <Button className="gap-2 shadow-lg shadow-primary/20">
              <Plus className="h-4 w-4" />
              <span>Create Workflow</span>
            </Button>
          </Link>
        </div>
      </div>

      {/* Analytics Summary Cards */}
      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
        <div className="rounded-2xl border border-border/60 bg-card p-5 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-xs font-medium text-muted-foreground">Total Workflows</p>
            <h3 className="mt-1 text-2xl font-bold text-foreground">{totalCount}</h3>
          </div>
          <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-primary/10 text-primary">
            <Zap className="h-6 w-6" />
          </div>
        </div>

        <div className="rounded-2xl border border-border/60 bg-card p-5 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-xs font-medium text-muted-foreground">Active Workflows</p>
            <h3 className="mt-1 text-2xl font-bold text-emerald-500">{activeCount}</h3>
          </div>
          <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-emerald-500/10 text-emerald-500">
            <CheckCircle2 className="h-6 w-6" />
          </div>
        </div>

        <div className="rounded-2xl border border-border/60 bg-card p-5 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-xs font-medium text-muted-foreground">Est. Hours Saved</p>
            <h3 className="mt-1 text-2xl font-bold text-amber-500">~{estimatedHoursSaved} hrs</h3>
          </div>
          <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-amber-500/10 text-amber-500">
            <Clock className="h-6 w-6" />
          </div>
        </div>

        <div className="rounded-2xl border border-border/60 bg-card p-5 shadow-sm flex items-center justify-between">
          <div>
            <p className="text-xs font-medium text-muted-foreground">Success Rate</p>
            <h3 className="mt-1 text-2xl font-bold text-indigo-500">99.4%</h3>
          </div>
          <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-indigo-500/10 text-indigo-500">
            <ShieldCheck className="h-6 w-6" />
          </div>
        </div>
      </div>

      {/* Filters & Search */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3.5 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <input
            type="text"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Search workflows by name..."
            className="w-full rounded-xl border border-border bg-card pl-10 pr-4 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 shadow-sm"
          />
        </div>

        <div className="flex items-center gap-3">
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="rounded-xl border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 shadow-sm"
          >
            <option value="">All Statuses</option>
            <option value="Active">Active</option>
            <option value="Draft">Draft</option>
            <option value="Paused">Paused</option>
            <option value="Archived">Archived</option>
          </select>
        </div>
      </div>

      {/* Workflow List Table */}
      <div className="overflow-hidden rounded-2xl border border-border/60 bg-card shadow-sm">
        {isLoading ? (
          <div className="p-12 text-center text-sm text-muted-foreground">Loading workflows...</div>
        ) : workflows.length === 0 ? (
          <div className="flex flex-col items-center justify-center p-12 text-center">
            <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-primary/10 text-primary mb-4">
              <Zap className="h-8 w-8" />
            </div>
            <h3 className="text-lg font-bold text-foreground">No workflows found</h3>
            <p className="mt-1 text-xs text-muted-foreground max-w-sm">
              Automate your first workflow in minutes or pick a pre-built template.
            </p>
            <Link href="/dashboard/workflows/create" className="mt-6">
              <Button className="gap-2">
                <Plus className="h-4 w-4" />
                <span>Create Workflow</span>
              </Button>
            </Link>
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full text-left text-sm text-foreground">
              <thead className="border-b border-border/50 bg-muted/40 text-xs uppercase font-semibold text-muted-foreground">
                <tr>
                  <th className="px-6 py-4">Workflow</th>
                  <th className="px-6 py-4">Trigger</th>
                  <th className="px-6 py-4">Actions</th>
                  <th className="px-6 py-4">Status</th>
                  <th className="px-6 py-4">Version</th>
                  <th className="px-6 py-4 text-right">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-border/40">
                {workflows.map((w) => (
                  <tr key={w.id} className="hover:bg-muted/20 transition-colors">
                    <td className="px-6 py-4">
                      <Link href={`/dashboard/workflows/${w.id}`} className="flex flex-col group">
                        <span className="font-semibold text-foreground group-hover:text-primary transition-colors">
                          {w.name}
                        </span>
                        {w.description && (
                          <span className="text-xs text-muted-foreground truncate max-w-xs">{w.description}</span>
                        )}
                      </Link>
                    </td>

                    <td className="px-6 py-4">
                      <span className="inline-flex items-center gap-1.5 rounded-lg bg-amber-500/10 px-2.5 py-1 text-xs font-semibold text-amber-500">
                        <Zap className="h-3.5 w-3.5" />
                        {w.trigger?.triggerType || "Manual"}
                      </span>
                    </td>

                    <td className="px-6 py-4">
                      <span className="text-xs font-semibold text-muted-foreground">
                        {w.actions?.length || 0} Actions
                      </span>
                    </td>

                    <td className="px-6 py-4">
                      <span
                        className={`inline-flex items-center gap-1 rounded-full px-2.5 py-0.5 text-xs font-semibold ${
                          w.status === "Active"
                            ? "bg-emerald-500/10 text-emerald-500"
                            : w.status === "Draft"
                            ? "bg-muted text-muted-foreground"
                            : "bg-rose-500/10 text-rose-500"
                        }`}
                      >
                        {w.status}
                      </span>
                    </td>

                    <td className="px-6 py-4 text-xs font-mono text-muted-foreground">
                      v{w.version}
                    </td>

                    <td className="px-6 py-4 text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => executeManual.mutate({ id: w.id })}
                          title="Run Manually"
                          className="h-8 w-8 p-0 text-emerald-500 hover:bg-emerald-500/10"
                        >
                          <Play className="h-4 w-4" />
                        </Button>

                        <Link href={`/dashboard/workflows/edit/${w.id}`}>
                          <Button variant="ghost" size="sm" className="h-8 w-8 p-0">
                            <Edit className="h-4 w-4" />
                          </Button>
                        </Link>

                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => deleteWorkflow.mutate(w.id)}
                          className="h-8 w-8 p-0 text-destructive hover:bg-destructive/10"
                        >
                          <Trash2 className="h-4 w-4" />
                        </Button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
