"use client";

import { Workflow } from "@/types/automation";
import { ArrowDown, BrainCircuit, CheckCircle2, AlertTriangle, Zap, Workflow as WorkflowIcon } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

interface WorkflowPreviewProps {
  workflow: Partial<Workflow>;
}

export function WorkflowPreview({ workflow }: WorkflowPreviewProps) {
  const steps = workflow.steps || [];
  
  // Sort steps by order
  const sortedSteps = [...steps].sort((a, b) => (a.stepOrder || 0) - (b.stepOrder || 0));

  return (
    <div className="flex flex-col items-center justify-start w-full py-6 space-y-2">
      {/* TRIGGER */}
      <div className="w-full max-w-sm">
        <Card className="border-indigo-200 dark:border-indigo-900 bg-indigo-50/50 dark:bg-indigo-900/10 shadow-sm relative">
          <div className="absolute -left-3 -top-3 p-1.5 bg-indigo-100 dark:bg-indigo-900 text-indigo-600 dark:text-indigo-300 rounded-full border border-indigo-200 dark:border-indigo-800">
            <Zap className="h-4 w-4" />
          </div>
          <CardContent className="p-4 pt-5 text-center">
            <p className="text-xs font-semibold text-indigo-600 dark:text-indigo-400 tracking-wider mb-1 uppercase">Trigger</p>
            <p className="font-medium">
              {workflow.triggerType ? workflow.triggerType.replace(/([A-Z])/g, ' $1').trim() : "Select a trigger"}
            </p>
          </CardContent>
        </Card>
      </div>

      {sortedSteps.map((step, index) => {
        let Icon = WorkflowIcon;
        let colorClass = "text-slate-500 border-slate-200 bg-slate-50";
        let titleClass = "text-slate-600";
        
        if (step.stepType === 'Condition') {
          colorClass = "border-amber-200 dark:border-amber-900 bg-amber-50/50 dark:bg-amber-900/10";
          titleClass = "text-amber-600 dark:text-amber-400";
        } else if (step.stepType === 'AIDecision') {
          Icon = BrainCircuit;
          colorClass = "border-purple-200 dark:border-purple-900 bg-purple-50/50 dark:bg-purple-900/10";
          titleClass = "text-purple-600 dark:text-purple-400";
        } else if (step.stepType === 'Action') {
          Icon = CheckCircle2;
          colorClass = "border-emerald-200 dark:border-emerald-900 bg-emerald-50/50 dark:bg-emerald-900/10";
          titleClass = "text-emerald-600 dark:text-emerald-400";
        } else if (step.stepType === 'Approval') {
          Icon = AlertTriangle;
          colorClass = "border-rose-200 dark:border-rose-900 bg-rose-50/50 dark:bg-rose-900/10";
          titleClass = "text-rose-600 dark:text-rose-400";
        }

        return (
          <div key={step.id || `step-${index}`} className="flex flex-col items-center w-full max-w-sm">
            <ArrowDown className="h-6 w-6 text-muted-foreground/30 my-1" />
            <Card className={`w-full shadow-sm relative ${colorClass}`}>
              <div className={`absolute -left-3 -top-3 p-1.5 rounded-full border bg-background ${titleClass}`}>
                <Icon className="h-4 w-4" />
              </div>
              <CardContent className="p-4 pt-5 text-center">
                <p className={`text-xs font-semibold tracking-wider mb-1 uppercase ${titleClass}`}>
                  {step.stepType === 'AIDecision' ? 'AI Decision' : step.stepType}
                </p>
                <p className="font-medium">{step.name || "Configure step"}</p>
              </CardContent>
            </Card>
          </div>
        );
      })}

      {sortedSteps.length === 0 && (
        <div className="flex flex-col items-center w-full max-w-sm">
           <ArrowDown className="h-6 w-6 text-muted-foreground/30 my-1" />
           <div className="w-full p-4 border border-dashed rounded-xl text-center text-muted-foreground text-sm">
             Add actions to complete workflow
           </div>
        </div>
      )}
    </div>
  );
}
