"use client";

import { useMemory } from "@/hooks/use-memory";
import { Card, CardHeader, CardTitle, CardContent, CardDescription } from "@/components/ui/card";
import { BrainCircuit, Loader2, Sparkles, Target, Zap, Clock, TrendingUp } from "lucide-react";
import { Badge } from "@/components/ui/badge";

interface CustomerAiContextProps {
  customerId: string;
}

export function CustomerAiContext({ customerId }: CustomerAiContextProps) {
  const { useCustomerMemories } = useMemory();
  const { data: memories, isLoading } = useCustomerMemories(customerId);

  if (isLoading) {
    return (
      <div className="flex h-48 items-center justify-center rounded-xl border border-border bg-card">
        <Loader2 className="h-6 w-6 animate-spin text-muted-foreground" />
      </div>
    );
  }

  const activeMemories = memories?.filter(m => m.isActive) || [];

  if (activeMemories.length === 0) {
    return (
      <div className="flex h-64 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card px-4 text-center">
        <div className="flex h-12 w-12 items-center justify-center rounded-full bg-indigo-500/10 text-indigo-500 mb-4">
          <BrainCircuit className="h-6 w-6" />
        </div>
        <h3 className="text-base font-semibold text-foreground mb-1">No AI Context Available</h3>
        <p className="text-sm text-muted-foreground max-w-sm">
          BusinessOS AI hasn't learned any specific patterns or context for this customer yet.
        </p>
      </div>
    );
  }

  // Categorize for better UX
  const purchasePatterns = activeMemories.filter(m => m.memoryType === "BusinessPattern" || m.memoryType === "CustomerContext" && m.title.toLowerCase().includes("purchase"));
  const preferences = activeMemories.filter(m => m.memoryType === "CustomerContext" && !m.title.toLowerCase().includes("purchase"));
  const successfulActions = activeMemories.filter(m => m.memoryType === "ActionOutcome" && m.content.toLowerCase().includes("success"));
  const otherContext = activeMemories.filter(m => !purchasePatterns.includes(m) && !preferences.includes(m) && !successfulActions.includes(m));

  return (
    <div className="grid gap-6 md:grid-cols-2">
      <Card className="border-indigo-500/20 shadow-indigo-500/5">
        <CardHeader className="pb-3">
          <CardTitle className="text-base flex items-center gap-2 text-indigo-600 dark:text-indigo-400">
            <TrendingUp className="h-4 w-4" /> Purchase Patterns
          </CardTitle>
          <CardDescription>What AI has learned about buying habits</CardDescription>
        </CardHeader>
        <CardContent className="space-y-3">
          {purchasePatterns.length > 0 ? purchasePatterns.map(m => (
            <div key={m.id} className="rounded-lg bg-indigo-500/5 p-3 border border-indigo-500/10 text-sm">
              <span className="font-medium text-foreground block mb-0.5">{m.title}</span>
              <span className="text-muted-foreground">{m.content}</span>
            </div>
          )) : <p className="text-sm text-muted-foreground italic">No known patterns.</p>}
        </CardContent>
      </Card>

      <Card className="border-emerald-500/20 shadow-emerald-500/5">
        <CardHeader className="pb-3">
          <CardTitle className="text-base flex items-center gap-2 text-emerald-600 dark:text-emerald-400">
            <Target className="h-4 w-4" /> Customer Preferences
          </CardTitle>
          <CardDescription>Known preferences and requirements</CardDescription>
        </CardHeader>
        <CardContent className="space-y-3">
          {preferences.length > 0 ? preferences.map(m => (
            <div key={m.id} className="rounded-lg bg-emerald-500/5 p-3 border border-emerald-500/10 text-sm">
              <span className="font-medium text-foreground block mb-0.5">{m.title}</span>
              <span className="text-muted-foreground">{m.content}</span>
            </div>
          )) : <p className="text-sm text-muted-foreground italic">No known preferences.</p>}
        </CardContent>
      </Card>

      <Card className="border-amber-500/20 shadow-amber-500/5 md:col-span-2">
        <CardHeader className="pb-3">
          <CardTitle className="text-base flex items-center gap-2 text-amber-600 dark:text-amber-400">
            <Zap className="h-4 w-4" /> Successful Interactions
          </CardTitle>
          <CardDescription>Previous actions that resulted in positive outcomes</CardDescription>
        </CardHeader>
        <CardContent className="space-y-3">
          {successfulActions.length > 0 ? successfulActions.map(m => (
            <div key={m.id} className="rounded-lg bg-amber-500/5 p-3 border border-amber-500/10 text-sm flex justify-between items-center">
              <div>
                <span className="font-medium text-foreground block mb-0.5">{m.title}</span>
                <span className="text-muted-foreground">{m.content}</span>
              </div>
              <Badge variant="outline" className="bg-background text-amber-600 border-amber-500/30">Action Memory</Badge>
            </div>
          )) : <p className="text-sm text-muted-foreground italic">No historical successful actions logged.</p>}
        </CardContent>
      </Card>
      
      {otherContext.length > 0 && (
        <Card className="md:col-span-2 border-slate-200 dark:border-slate-800">
          <CardHeader className="pb-3">
            <CardTitle className="text-base flex items-center gap-2 text-slate-700 dark:text-slate-300">
              <Clock className="h-4 w-4" /> General Context
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            {otherContext.map(m => (
              <div key={m.id} className="rounded-lg bg-slate-50 dark:bg-slate-900 p-3 border border-slate-200 dark:border-slate-800 text-sm">
                <span className="font-medium text-foreground block mb-0.5">{m.title}</span>
                <span className="text-muted-foreground">{m.content}</span>
              </div>
            ))}
          </CardContent>
        </Card>
      )}
    </div>
  );
}
