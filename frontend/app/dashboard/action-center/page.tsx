"use client";

import { useState, useMemo } from "react";
import { AiActionDto } from "@/types/action-center";
import { BusinessImpactDashboard } from "@/components/action-center/BusinessImpactDashboard";
import { ActionFilters } from "@/components/action-center/ActionFilters";
import { AiActionList } from "@/components/action-center/AiActionList";
import { ExecutionTimeline } from "@/components/action-center/ExecutionTimeline";
import { ActionDetailsDrawer } from "@/components/action-center/ActionDetailsDrawer";
import { toast } from "sonner";
import {
  useGetActions,
  useApproveActionMutation,
  useExecuteActionMutation,
  useRejectActionMutation,
} from "@/hooks/useActionCenter";

export default function ActionCenterPage() {
  const { data: actions = [], isLoading, error } = useGetActions();
  const approveMutation = useApproveActionMutation();
  const executeMutation = useExecuteActionMutation();
  const rejectMutation = useRejectActionMutation();

  const [searchQuery, setSearchQuery] = useState("");
  const [priorityFilter, setPriorityFilter] = useState("all");
  const [statusFilter, setStatusFilter] = useState("all");

  const [selectedAction, setSelectedAction] = useState<AiActionDto | null>(null);
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [processingIds, setProcessingIds] = useState<Set<string>>(new Set());

  // Filter actions based on state
  const filteredActions = useMemo(() => {
    return actions.filter((action) => {
      const matchesSearch = action.title.toLowerCase().includes(searchQuery.toLowerCase()) || 
                            action.description.toLowerCase().includes(searchQuery.toLowerCase());
      const matchesPriority = priorityFilter === "all" || action.priority === priorityFilter;
      const matchesStatus = statusFilter === "all" || action.status === statusFilter;
      
      return matchesSearch && matchesPriority && matchesStatus;
    });
  }, [actions, searchQuery, priorityFilter, statusFilter]);

  // Calculate summary metrics
  const summary = useMemo(() => {
    let revenueSaved = 0;
    let costReduced = 0;
    let actionsExecuted = 0;
    let pendingActions = 0;
    let successfulActions = 0;

    actions.forEach(a => {
      if (a.status === "Executed") {
        actionsExecuted++;
        successfulActions++; // Simplified success rate calculation
        revenueSaved += a.estimatedRevenueIncrease;
        costReduced += a.estimatedCostSaving;
      } else if (a.status === "Pending" || a.status === "Approved") {
        pendingActions++;
      }
    });

    return {
      revenueSaved,
      costReduced,
      timeSaved: actionsExecuted * 2, // Arbitrary 2 hours per action saved
      actionsExecuted,
      pendingActions,
      successRate: actionsExecuted > 0 ? Math.round((successfulActions / actionsExecuted) * 100) : 0,
    };
  }, [actions]);

  const handleApprove = async (id: string) => {
    try {
      setProcessingIds(prev => new Set(prev).add(id));
      await approveMutation.mutateAsync(id);
      toast.success("Action approved successfully.");
    } catch (err: any) {
      toast.error(err.response?.data?.Error || "Failed to approve action.");
    } finally {
      setProcessingIds(prev => {
        const next = new Set(prev);
        next.delete(id);
        return next;
      });
    }
  };

  const handleReject = async (id: string) => {
    try {
      setProcessingIds(prev => new Set(prev).add(id));
      await rejectMutation.mutateAsync(id);
      toast.success("Action rejected.");
    } catch (err: any) {
      toast.error(err.response?.data?.Error || "Failed to reject action.");
    } finally {
      setProcessingIds(prev => {
        const next = new Set(prev);
        next.delete(id);
        return next;
      });
    }
  };

  const handleExecute = async (id: string) => {
    try {
      setProcessingIds(prev => new Set(prev).add(id));
      await executeMutation.mutateAsync(id);
      toast.success("Action executed successfully.");
    } catch (err: any) {
      toast.error(err.response?.data?.Error || "Failed to execute action.");
    } finally {
      setProcessingIds(prev => {
        const next = new Set(prev);
        next.delete(id);
        return next;
      });
    }
  };

  const handleViewDetails = (action: AiActionDto) => {
    setSelectedAction(action);
    setIsDrawerOpen(true);
  };

  if (error) {
    return (
      <div className="flex items-center justify-center h-full p-8">
        <div className="text-center text-rose-500">
          <h2 className="text-2xl font-bold mb-2">Error loading actions</h2>
          <p>Please try refreshing the page.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto p-4 md:p-6 lg:p-8 space-y-8 max-w-7xl">
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">AI Action Center</h1>
          <p className="text-muted-foreground mt-1">
            Review and execute AI-generated business recommendations.
          </p>
        </div>
      </div>

      <BusinessImpactDashboard summary={summary} isLoading={isLoading} />

      <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
        <div className="lg:col-span-3 space-y-6">
          <ActionFilters 
            onSearchChange={setSearchQuery}
            onPriorityChange={setPriorityFilter}
            onStatusChange={setStatusFilter}
          />
          
          <div>
            <h2 className="text-xl font-semibold mb-4">Suggested Actions</h2>
            <AiActionList
              actions={filteredActions}
              isLoading={isLoading}
              onApprove={handleApprove}
              onReject={handleReject}
              onExecute={handleExecute}
              onViewDetails={handleViewDetails}
              processingIds={processingIds}
            />
          </div>
        </div>
        
        <div className="lg:col-span-1">
          <div className="sticky top-6">
            <ExecutionTimeline actions={actions} isLoading={isLoading} />
          </div>
        </div>
      </div>

      <ActionDetailsDrawer
        action={selectedAction}
        isOpen={isDrawerOpen}
        onClose={() => setIsDrawerOpen(false)}
      />
    </div>
  );
}
