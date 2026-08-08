"use client";

import Link from "next/link";
import { 
  Zap, Plus, CheckCircle2, PlayCircle, Settings2, 
  History, Clock, AlertTriangle, TrendingUp, DollarSign 
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { useAiWorkflows, useAiExecutions } from "@/hooks/useAiAutomation";
import { AutomationTemplates } from "./automation-templates";
import { Workflow, WorkflowExecution } from "@/types/automation";

export function AiAutomationDashboard() {
  const { data: workflows = [], isLoading: loadingWorkflows } = useAiWorkflows();
  const { data: executions = [], isLoading: loadingExecutions } = useAiExecutions(10);

  const activeWorkflows = workflows.filter((w: Workflow) => w.isActive);
  const successRate = executions.length > 0 
    ? Math.round((executions.filter((e: WorkflowExecution) => e.status === 'Completed').length / executions.length) * 100)
    : 0;

  // Mocking some business impact numbers since this is the first version 
  // and we don't have historical ML predictions ready yet.
  const timeSaved = activeWorkflows.length * 2.5; // Hours
  const revenueOps = activeWorkflows.length * 15000; // currency

  return (
    <div className="space-y-6 max-w-7xl mx-auto py-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">AI Automation</h1>
          <p className="text-muted-foreground mt-1">
            Let BusinessOS AI handle repetitive work automatically.
          </p>
        </div>
        <Link href="/automation/create">
          <Button className="gap-2 shadow-lg" size="lg">
            <Plus className="h-4 w-4" />
            Create Automation
          </Button>
        </Link>
      </div>

      <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
        <Card className="bg-gradient-to-br from-indigo-500/10 to-transparent border-indigo-500/20">
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Active Automations</CardTitle>
            <Zap className="h-4 w-4 text-indigo-500" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-indigo-700 dark:text-indigo-400">
              {activeWorkflows.length}
            </div>
            <p className="text-xs text-muted-foreground mt-1">Working for you 24/7</p>
          </CardContent>
        </Card>

        <Card className="bg-gradient-to-br from-emerald-500/10 to-transparent border-emerald-500/20">
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Time Saved (Est.)</CardTitle>
            <Clock className="h-4 w-4 text-emerald-500" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-emerald-700 dark:text-emerald-400">
              {timeSaved} hrs
            </div>
            <p className="text-xs text-muted-foreground mt-1">This month</p>
          </CardContent>
        </Card>

        <Card className="bg-gradient-to-br from-amber-500/10 to-transparent border-amber-500/20">
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Revenue Opportunities</CardTitle>
            <DollarSign className="h-4 w-4 text-amber-500" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-amber-700 dark:text-amber-400">
              ₹{revenueOps.toLocaleString()}
            </div>
            <p className="text-xs text-muted-foreground mt-1">Recovered or protected</p>
          </CardContent>
        </Card>

        <Card className="bg-gradient-to-br from-blue-500/10 to-transparent border-blue-500/20">
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Success Rate</CardTitle>
            <CheckCircle2 className="h-4 w-4 text-blue-500" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-blue-700 dark:text-blue-400">
              {successRate}%
            </div>
            <p className="text-xs text-muted-foreground mt-1">Across all executions</p>
          </CardContent>
        </Card>
      </div>

      <div className="grid md:grid-cols-2 gap-6 mt-8">
        <div>
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-xl font-semibold">Your Automations</h2>
          </div>
          
          {loadingWorkflows ? (
            <div className="space-y-3">
              {[1,2,3].map(i => <Card key={i} className="h-20 animate-pulse bg-muted/20" />)}
            </div>
          ) : workflows.length === 0 ? (
            <Card className="border-dashed shadow-none bg-muted/30">
              <CardContent className="flex flex-col items-center justify-center p-8 text-center">
                <Zap className="h-10 w-10 text-muted-foreground mb-3 opacity-20" />
                <h3 className="text-lg font-medium">Automate your first repetitive task.</h3>
                <p className="text-sm text-muted-foreground mb-4 max-w-sm mt-1">
                  BusinessOS AI can monitor your business and handle routine work automatically.
                </p>
                <Link href="/automation/create">
                  <Button variant="outline">Create Automation</Button>
                </Link>
              </CardContent>
            </Card>
          ) : (
            <div className="space-y-3">
              {workflows.map((wf: Workflow) => (
                <Link key={wf.id} href={`/automation/${wf.id}`}>
                  <Card className="hover:border-primary/50 transition-colors cursor-pointer group">
                    <CardContent className="p-4 flex items-center justify-between">
                      <div className="flex items-center gap-4">
                        <div className={`p-3 rounded-full ${wf.isActive ? 'bg-indigo-100 text-indigo-600 dark:bg-indigo-900/30' : 'bg-muted text-muted-foreground'}`}>
                          <Zap className="h-5 w-5" />
                        </div>
                        <div>
                          <p className="font-semibold">{wf.name}</p>
                          <p className="text-xs text-muted-foreground capitalize mt-0.5">
                            Trigger: {wf.triggerType.replace(/([A-Z])/g, ' $1').trim()}
                          </p>
                        </div>
                      </div>
                      <div className="flex flex-col items-end gap-2">
                        <Badge variant={wf.isActive ? 'default' : 'secondary'}>
                          {wf.isActive ? 'Active' : 'Paused'}
                        </Badge>
                        <span className="text-[10px] text-muted-foreground opacity-0 group-hover:opacity-100 transition-opacity">
                          View details →
                        </span>
                      </div>
                    </CardContent>
                  </Card>
                </Link>
              ))}
            </div>
          )}
        </div>

        <div>
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-xl font-semibold">Recent Actions</h2>
          </div>
          
          {loadingExecutions ? (
            <div className="space-y-3">
              {[1,2,3].map(i => <Card key={i} className="h-16 animate-pulse bg-muted/20" />)}
            </div>
          ) : executions.length === 0 ? (
            <Card className="border-dashed shadow-none bg-muted/30">
              <CardContent className="flex flex-col items-center justify-center p-8 text-center">
                <History className="h-10 w-10 text-muted-foreground mb-3 opacity-20" />
                <p className="text-sm text-muted-foreground">No recent actions executed.</p>
              </CardContent>
            </Card>
          ) : (
            <div className="space-y-3">
              {executions.map((ex: WorkflowExecution) => (
                <Card key={ex.id} className="shadow-sm">
                  <CardContent className="p-4 flex items-center justify-between">
                    <div className="flex flex-col">
                      <p className="font-medium text-sm">
                        {ex.workflow?.name || 'Unknown Workflow'}
                      </p>
                      <p className="text-xs text-muted-foreground mt-1">
                        {new Date(ex.startedAt).toLocaleString(undefined, {
                          month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit'
                        })}
                      </p>
                    </div>
                    <Badge 
                      variant={
                        ex.status === 'Completed' ? 'default' : 
                        ex.status === 'Failed' ? 'destructive' : 
                        ex.status === 'WaitingForApproval' ? 'outline' : 'secondary'
                      }
                      className={ex.status === 'Completed' ? 'bg-emerald-500 hover:bg-emerald-600' : ''}
                    >
                      {ex.status.replace(/([A-Z])/g, ' $1').trim()}
                    </Badge>
                  </CardContent>
                </Card>
              ))}
            </div>
          )}
        </div>
      </div>
      
      <div className="mt-8">
        <AutomationTemplates />
      </div>

      <div className="mt-10 bg-blue-50/50 dark:bg-blue-900/10 border border-blue-100 dark:border-blue-900/30 rounded-xl p-6">
        <div className="flex items-start gap-4">
          <div className="p-2 bg-blue-100 dark:bg-blue-800 text-blue-600 dark:text-blue-300 rounded-lg">
            <TrendingUp className="h-5 w-5" />
          </div>
          <div>
            <h3 className="font-semibold text-blue-900 dark:text-blue-100 mb-1">BusinessOS AI Recommendations</h3>
            <ul className="space-y-2 mt-3">
              <li className="flex items-center gap-2 text-sm text-blue-800 dark:text-blue-200">
                <div className="h-1.5 w-1.5 rounded-full bg-blue-500" />
                You manually sent 5 payment reminders this week. Consider automating this process.
              </li>
              <li className="flex items-center gap-2 text-sm text-blue-800 dark:text-blue-200">
                <div className="h-1.5 w-1.5 rounded-full bg-blue-500" />
                Setting up "New Lead Follow-up" could save approximately 2 hours per week.
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}
