"use client";

import React from "react";
import { ArrowLeft, Save, Sparkles, Play, CheckCircle2, RotateCcw, RotateCw, ZoomIn, ZoomOut } from "lucide-react";
import { Button } from "@/components/ui/button";
import Link from "next/link";

interface WorkflowToolbarProps {
  name: string;
  setName: (name: string) => void;
  status: "Draft" | "Active" | "Paused" | "Archived";
  setStatus: (status: "Draft" | "Active" | "Paused" | "Archived") => void;
  onSave: () => void;
  onTestRun: () => void;
  onOpenAiAssistant: () => void;
  isSaving?: boolean;
}

export function WorkflowToolbar({
  name,
  setName,
  status,
  setStatus,
  onSave,
  onTestRun,
  onOpenAiAssistant,
  isSaving = false,
}: WorkflowToolbarProps) {
  return (
    <header className="flex h-16 shrink-0 items-center justify-between border-b border-border bg-background/95 px-6 backdrop-blur-xl">
      <div className="flex items-center gap-4">
        <Link
          href="/dashboard/workflows"
          className="flex h-9 w-9 items-center justify-center rounded-xl border border-border/50 text-muted-foreground transition-colors hover:bg-muted hover:text-foreground"
        >
          <ArrowLeft className="h-4 w-4" />
        </Link>
        <div className="flex flex-col">
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Workflow Name..."
            className="bg-transparent text-base font-bold text-foreground focus:outline-none focus:ring-1 focus:ring-primary/20 rounded px-1 -ml-1"
          />
          <span className="text-xs text-muted-foreground">Visual Workflow Automation Builder</span>
        </div>
      </div>

      <div className="flex items-center gap-3">
        <Button
          variant="outline"
          size="sm"
          onClick={onOpenAiAssistant}
          className="gap-2 border-primary/30 bg-primary/5 text-primary hover:bg-primary/10 shadow-sm"
        >
          <Sparkles className="h-4 w-4" />
          <span>AI Assistant</span>
        </Button>

        <Button variant="outline" size="sm" onClick={onTestRun} className="gap-2">
          <Play className="h-3.5 w-3.5 text-emerald-500" />
          <span>Test Run</span>
        </Button>

        <div className="h-6 w-px bg-border" />

        <select
          value={status}
          onChange={(e) => setStatus(e.target.value as any)}
          className="rounded-lg border border-border bg-card px-3 py-1.5 text-xs font-semibold text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
        >
          <option value="Draft">Draft</option>
          <option value="Active">Active</option>
          <option value="Paused">Paused</option>
          <option value="Archived">Archived</option>
        </select>

        <Button size="sm" onClick={onSave} disabled={isSaving} className="gap-2 shadow-md shadow-primary/20">
          <Save className="h-4 w-4" />
          <span>{isSaving ? "Saving..." : "Save Workflow"}</span>
        </Button>
      </div>
    </header>
  );
}
