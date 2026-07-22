"use client";

import React from "react";
import { Sparkles, ArrowRight, Zap, CheckCircle2, Copy } from "lucide-react";
import { Button } from "@/components/ui/button";

export interface WorkflowTemplateInfo {
  id: string;
  name: string;
  category: string;
  description: string;
  triggerName: string;
  actionsCount: number;
  estimatedHoursSaved: number;
  popular?: boolean;
}

interface TemplateCardProps {
  template: WorkflowTemplateInfo;
  onInstall: (template: WorkflowTemplateInfo) => void;
}

export function TemplateCard({ template, onInstall }: TemplateCardProps) {
  return (
    <div className="group relative flex flex-col justify-between rounded-2xl border border-border/60 bg-card p-6 shadow-sm transition-all duration-200 hover:border-primary/40 hover:shadow-md">
      <div className="flex flex-col gap-3">
        <div className="flex items-center justify-between">
          <span className="inline-flex items-center gap-1 rounded-full bg-primary/10 px-2.5 py-0.5 text-[11px] font-semibold text-primary">
            {template.category}
          </span>
          {template.popular && (
            <span className="inline-flex items-center gap-1 rounded-full bg-amber-500/10 px-2.5 py-0.5 text-[11px] font-bold text-amber-500">
              <Sparkles className="h-3 w-3" />
              Popular
            </span>
          )}
        </div>

        <h3 className="text-base font-bold text-foreground group-hover:text-primary transition-colors">
          {template.name}
        </h3>

        <p className="text-xs text-muted-foreground leading-relaxed line-clamp-2">
          {template.description}
        </p>

        <div className="mt-2 flex flex-wrap items-center gap-3 text-xs text-muted-foreground">
          <div className="flex items-center gap-1">
            <Zap className="h-3.5 w-3.5 text-amber-500" />
            <span>{template.triggerName}</span>
          </div>
          <span>•</span>
          <div className="flex items-center gap-1">
            <CheckCircle2 className="h-3.5 w-3.5 text-emerald-500" />
            <span>{template.actionsCount} Actions</span>
          </div>
          <span>•</span>
          <div className="font-semibold text-primary">
            ~{template.estimatedHoursSaved} hrs/mo saved
          </div>
        </div>
      </div>

      <div className="mt-6 pt-4 border-t border-border/40">
        <Button onClick={() => onInstall(template)} className="w-full gap-2 text-xs font-semibold shadow-sm">
          <span>Use Template</span>
          <ArrowRight className="h-4 w-4" />
        </Button>
      </div>
    </div>
  );
}
