"use client";

import { use } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { 
  ArrowLeft, Activity, Play, Square, Trash2, Calendar, Clock, AlertTriangle, ExternalLink
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { useAiWorkflow, useActivateAiWorkflow, useDeactivateAiWorkflow, useDeleteAiWorkflow } from "@/hooks/useAiAutomation";
import { WorkflowPreview } from "@/components/automation/workflow-preview";
import { toast } from "sonner";

export default function WorkflowDetailsPage({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  const router = useRouter();
  const { data: workflow, isLoading } = useAiWorkflow(resolvedParams.id);
  const { mutate: activate, isPending: isActivating } = useActivateAiWorkflow();
  const { mutate: deactivate, isPending: isDeactivating } = useDeactivateAiWorkflow();
  const { mutateAsync: deleteWorkflow, isPending: isDeleting } = useDeleteAiWorkflow();

  if (isLoading) {
    return <div className="p-8 animate-pulse space-y-6">
      <div className="h-10 bg-muted/20 w-1/3 rounded-lg" />
      <div className="h-64 bg-muted/20 w-full rounded-lg" />
    </div>;
  }

  if (!workflow) {
    return <div className="p-8 text-center text-muted-foreground">Automation not found.</div>;
  }

  const handleDelete = async () => {
    if (confirm("Are you sure you want to delete this automation? This cannot be undone.")) {
      try {
        await deleteWorkflow(workflow.id);
        router.push("/automation");
      } catch (e) {
        // Handled by hook
      }
    }
  };

  const handleTest = () => {
    toast.success("Test execution started. Check recent actions in a few moments.");
    // In a real implementation, we'd call the test API
  };

  return (
    <div className="space-y-6 max-w-7xl mx-auto py-6">
      <div className="flex items-center gap-4 text-sm text-muted-foreground mb-4">
        <Link href="/automation" className="hover:text-foreground flex items-center gap-1 transition-colors">
          <ArrowLeft className="h-4 w-4" /> Automations
        </Link>
        <span>/</span>
        <span className="text-foreground font-medium">{workflow.name}</span>
      </div>

      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">{workflow.name}</h1>
          <div className="flex items-center gap-3 mt-2">
            <Badge variant={workflow.isActive ? 'default' : 'secondary'} className={workflow.isActive ? 'bg-indigo-500' : ''}>
              {workflow.isActive ? 'Active' : 'Paused'}
            </Badge>
            <span className="text-sm text-muted-foreground flex items-center gap-1">
              <Calendar className="h-3 w-3" /> Created {new Date(workflow.createdAt).toLocaleDateString()}
            </span>
          </div>
        </div>
        
        <div className="flex items-center gap-2">
          <Button variant="outline" size="sm" onClick={handleTest}>
            <Play className="h-4 w-4 mr-2" /> Test Run
          </Button>
          
          {workflow.isActive ? (
            <Button variant="secondary" size="sm" onClick={() => deactivate(workflow.id)} disabled={isDeactivating}>
              <Square className="h-4 w-4 mr-2" /> Pause
            </Button>
          ) : (
            <Button size="sm" onClick={() => activate(workflow.id)} disabled={isActivating}>
              <Play className="h-4 w-4 mr-2" /> Activate
            </Button>
          )}

          <Button variant="destructive" size="sm" onClick={handleDelete} disabled={isDeleting}>
            <Trash2 className="h-4 w-4" />
          </Button>
        </div>
      </div>

      <div className="grid lg:grid-cols-3 gap-6 mt-6">
        <div className="lg:col-span-2 space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Automation Flow</CardTitle>
              <CardDescription>Visual representation of the workflow</CardDescription>
            </CardHeader>
            <CardContent className="bg-slate-50/50 dark:bg-slate-900/10 rounded-b-xl border-t flex justify-center py-8">
              <WorkflowPreview workflow={workflow} />
            </CardContent>
          </Card>
          
          {workflow.requiresApproval && (
            <div className="bg-amber-50 dark:bg-amber-900/20 border border-amber-200 dark:border-amber-900/50 p-4 rounded-xl flex gap-3 text-amber-800 dark:text-amber-200">
              <AlertTriangle className="h-5 w-5 flex-shrink-0 mt-0.5" />
              <div>
                <h3 className="font-semibold text-sm">Approval Required</h3>
                <p className="text-sm mt-1">This workflow will pause and wait for manual approval before executing its actions. Approvals can be managed in the Action Center.</p>
                <Link href="/dashboard/action-center">
                  <Button variant="outline" size="sm" className="mt-3 bg-white dark:bg-black text-amber-700 dark:text-amber-300 border-amber-200 dark:border-amber-800 hover:bg-amber-100 dark:hover:bg-amber-900/50">
                    Go to Action Center <ExternalLink className="h-3 w-3 ml-2" />
                  </Button>
                </Link>
              </div>
            </div>
          )}
        </div>

        <div className="space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Business Impact</CardTitle>
              <CardDescription>Estimated value from this automation</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex justify-between items-center pb-3 border-b border-dashed">
                <div className="flex items-center gap-2 text-muted-foreground">
                  <Clock className="h-4 w-4" /> Time Saved
                </div>
                <div className="font-semibold text-emerald-600 dark:text-emerald-400">~2.5 hrs/week</div>
              </div>
              <div className="flex justify-between items-center pb-3 border-b border-dashed">
                <div className="flex items-center gap-2 text-muted-foreground">
                  <Activity className="h-4 w-4" /> Frequency
                </div>
                <div className="font-semibold">~12 runs/week</div>
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Recent Executions</CardTitle>
            </CardHeader>
            <CardContent>
              {/* Simplified for demo, normally we'd fetch specific executions for this workflow */}
              <div className="text-sm text-center text-muted-foreground py-4 border border-dashed rounded-lg">
                Executions will appear here after the automation is triggered.
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
