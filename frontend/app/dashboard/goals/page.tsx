"use client";

import { useState } from "react";
import { useGoals, useCreateGoal, useUpdateGoal, useDeleteGoal, useSyncGoals } from "@/hooks/use-bi";
import { 
  Target, 
  Plus, 
  RefreshCw, 
  CheckCircle2, 
  AlertTriangle, 
  Clock, 
  Trash2, 
  TrendingUp, 
  Calendar 
} from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription, DialogFooter } from "@/components/ui/dialog";

export default function GoalsManagementPage() {
  const [selectedStatus, setSelectedStatus] = useState<string | undefined>(undefined);
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [name, setName] = useState("");
  const [targetValue, setTargetValue] = useState("");
  const [initialValue, setInitialValue] = useState("");

  const { data: goals = [], isLoading } = useGoals(selectedStatus);
  const { mutate: createGoal, isPending: isCreating } = useCreateGoal();
  const { mutate: deleteGoal } = useDeleteGoal();
  const { mutate: syncGoals, isPending: isSyncing } = useSyncGoals();

  const handleCreate = () => {
    if (!name || !targetValue) return;
    createGoal(
      {
        name,
        targetValue: parseFloat(targetValue),
        initialValue: initialValue ? parseFloat(initialValue) : 0
      },
      {
        onSuccess: () => {
          setIsCreateOpen(false);
          setName("");
          setTargetValue("");
          setInitialValue("");
        }
      }
    );
  };

  const statuses = ["All", "InProgress", "Achieved", "Behind", "Failed"];

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-[1600px] mx-auto min-h-screen">
      {/* Header Banner */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between border-b border-border/40 pb-6">
        <div>
          <div className="flex items-center gap-2.5">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary ring-1 ring-primary/20 shadow-inner">
              <Target className="h-5 w-5" />
            </div>
            <div>
              <h1 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
                Goal Management & Progress Center
              </h1>
              <p className="text-sm text-muted-foreground mt-0.5">
                Set quantitative targets, track milestone completion & sync with live business KPIs
              </p>
            </div>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <Button
            variant="outline"
            onClick={() => syncGoals()}
            disabled={isSyncing}
            className="gap-2 rounded-xl text-xs font-medium"
          >
            <RefreshCw className={`h-3.5 w-3.5 ${isSyncing ? "animate-spin" : ""}`} />
            Sync with KPIs
          </Button>

          <Button
            onClick={() => setIsCreateOpen(true)}
            className="gap-2 rounded-xl text-xs font-semibold shadow-md"
          >
            <Plus className="h-3.5 w-3.5" />
            Create Business Goal
          </Button>
        </div>
      </div>

      {/* Filter Row */}
      <div className="flex items-center gap-2 flex-wrap">
        {statuses.map((st) => {
          const isSelected = (st === "All" && !selectedStatus) || selectedStatus === st;
          return (
            <button
              key={st}
              onClick={() => setSelectedStatus(st === "All" ? undefined : st)}
              className={`rounded-xl px-4 py-2 text-xs font-semibold transition-all border ${
                isSelected
                  ? "bg-primary text-primary-foreground border-primary shadow-sm"
                  : "bg-card/60 text-muted-foreground border-border/50 hover:bg-muted hover:text-foreground"
              }`}
            >
              {st}
            </button>
          );
        })}
      </div>

      {/* Goals Grid */}
      {isLoading ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map((n) => (
            <div key={n} className="h-44 rounded-2xl border border-border/40 bg-card/30 animate-pulse" />
          ))}
        </div>
      ) : goals.length === 0 ? (
        <Card className="rounded-2xl border-border/40 bg-card/40 p-12 text-center">
          <Target className="h-12 w-12 text-muted-foreground/50 mx-auto mb-3" />
          <h3 className="text-lg font-bold text-foreground">No Goals Defined</h3>
          <p className="text-sm text-muted-foreground max-w-md mx-auto mt-1 mb-4">
            Establish clear quarterly revenue targets, lead goals, or retention benchmarks to track growth automatically.
          </p>
          <Button onClick={() => setIsCreateOpen(true)} size="sm" className="rounded-xl">
            Create First Goal
          </Button>
        </Card>
      ) : (
        <div className="grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
          {goals.map((goal) => {
            const isAchieved = goal.status === "Achieved" || goal.progressPercentage >= 100;
            const isFailed = goal.status === "Failed";

            return (
              <Card key={goal.id} className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:shadow-md hover:border-border flex flex-col justify-between">
                <CardHeader className="space-y-2">
                  <div className="flex items-center justify-between">
                    <Badge className={`rounded-lg text-[10px] font-bold ${
                      isAchieved 
                        ? "bg-emerald-500/20 text-emerald-400 border-emerald-500/30" 
                        : isFailed 
                        ? "bg-red-500/20 text-red-400 border-red-500/30" 
                        : "bg-blue-500/20 text-blue-400 border-blue-500/30"
                    }`}>
                      {isAchieved ? "Achieved" : goal.status}
                    </Badge>

                    <button
                      onClick={() => deleteGoal(goal.id)}
                      className="text-muted-foreground hover:text-red-400 transition-colors p-1"
                      title="Delete Goal"
                    >
                      <Trash2 className="h-3.5 w-3.5" />
                    </button>
                  </div>

                  <CardTitle className="text-base font-bold text-foreground tracking-tight line-clamp-1">
                    {goal.name}
                  </CardTitle>
                </CardHeader>

                <CardContent className="space-y-4 pt-2">
                  <div>
                    <div className="flex items-center justify-between text-xs font-semibold mb-1.5">
                      <span className="text-muted-foreground">Progress</span>
                      <span className="text-foreground">{goal.progressPercentage}%</span>
                    </div>

                    {/* Progress Bar */}
                    <div className="h-2.5 w-full rounded-full bg-muted/60 overflow-hidden">
                      <div
                        className={`h-full rounded-full transition-all duration-500 ${
                          isAchieved ? "bg-emerald-500" : isFailed ? "bg-red-500" : "bg-primary"
                        }`}
                        style={{ width: `${Math.min(100, goal.progressPercentage)}%` }}
                      />
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-2 pt-2 border-t border-border/40 text-xs">
                    <div>
                      <span className="text-[10px] text-muted-foreground block">Current Value</span>
                      <span className="font-bold text-foreground">{goal.currentValue.toLocaleString()}</span>
                    </div>
                    <div className="text-right">
                      <span className="text-[10px] text-muted-foreground block">Target Value</span>
                      <span className="font-bold text-foreground">{goal.targetValue.toLocaleString()}</span>
                    </div>
                  </div>
                </CardContent>
              </Card>
            );
          })}
        </div>
      )}

      {/* Create Goal Modal */}
      <Dialog open={isCreateOpen} onOpenChange={setIsCreateOpen}>
        <DialogContent className="sm:max-w-[425px] rounded-2xl border-border bg-card">
          <DialogHeader>
            <DialogTitle className="text-lg font-bold">Create Organizational Goal</DialogTitle>
            <DialogDescription className="text-xs">
              Define a quantitative target to track against live business KPIs.
            </DialogDescription>
          </DialogHeader>

          <div className="space-y-4 py-2">
            <div className="space-y-1.5">
              <label className="text-xs font-semibold text-foreground">Goal Title</label>
              <input
                type="text"
                placeholder="e.g. Q3 Revenue Target"
                value={name}
                onChange={(e) => setName(e.target.value)}
                className="w-full rounded-xl border border-border bg-background px-3 py-2 text-xs text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
              />
            </div>

            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1.5">
                <label className="text-xs font-semibold text-foreground">Target Value</label>
                <input
                  type="number"
                  placeholder="50000"
                  value={targetValue}
                  onChange={(e) => setTargetValue(e.target.value)}
                  className="w-full rounded-xl border border-border bg-background px-3 py-2 text-xs text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
                />
              </div>
              <div className="space-y-1.5">
                <label className="text-xs font-semibold text-foreground">Initial Value</label>
                <input
                  type="number"
                  placeholder="0"
                  value={initialValue}
                  onChange={(e) => setInitialValue(e.target.value)}
                  className="w-full rounded-xl border border-border bg-background px-3 py-2 text-xs text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
                />
              </div>
            </div>
          </div>

          <DialogFooter className="gap-2 sm:gap-0">
            <Button variant="outline" onClick={() => setIsCreateOpen(false)} className="rounded-xl text-xs">
              Cancel
            </Button>
            <Button onClick={handleCreate} disabled={isCreating || !name || !targetValue} className="rounded-xl text-xs font-semibold">
              {isCreating ? "Creating..." : "Save Goal"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
