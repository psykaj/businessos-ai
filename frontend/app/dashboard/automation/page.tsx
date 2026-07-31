"use client";

import Link from "next/link";
import { 
  Zap, Plus, Activity, CheckCircle2, PlayCircle, Settings2, 
  LayoutTemplate, History, Clock, FileText
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { useAutomationWorkflows, useAutomationExecutions } from "@/hooks/useAutomationStudio";

export default function AutomationDashboardPage() {
  const { data: workflows = [], isLoading: loadingWorkflows } = useAutomationWorkflows();
  const { data: executions = [], isLoading: loadingExecutions } = useAutomationExecutions();

  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">AI Automation Studio</h1>
          <p className="text-muted-foreground">Build, manage, and monitor your event-driven workflows.</p>
        </div>
        <Link href="/dashboard/automation/workflows/new/edit">
          <Button className="gap-2 shadow-lg">
            <Plus className="h-4 w-4" />
            Create Workflow
          </Button>
        </Link>
      </div>

      <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
        {[
          { label: "Workflows", href: "/dashboard/automation/workflows", icon: Zap, color: "text-amber-500" },
          { label: "Templates", href: "/dashboard/automation/templates", icon: LayoutTemplate, color: "text-blue-500" },
          { label: "Execution History", href: "/dashboard/automation/history", icon: History, color: "text-emerald-500" },
          { label: "Schedules", href: "/dashboard/automation/schedules", icon: Clock, color: "text-indigo-500" },
        ].map((nav) => (
          <Link key={nav.label} href={nav.href}>
            <Card className="hover:border-primary transition-colors cursor-pointer group">
              <CardContent className="p-4 flex flex-col items-center justify-center gap-2 text-center h-28">
                <nav.icon className={`h-8 w-8 ${nav.color} group-hover:scale-110 transition-transform`} />
                <span className="font-medium text-sm">{nav.label}</span>
              </CardContent>
            </Card>
          </Link>
        ))}
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Active Workflows</CardTitle>
            <Activity className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{workflows.filter(r => r.status === 'Active').length}</div>
            <p className="text-xs text-muted-foreground">Out of {workflows.length} total</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Executions (24h)</CardTitle>
            <PlayCircle className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{executions.length}</div>
            <p className="text-xs text-emerald-500">{executions.filter(e => e.status === 'Completed').length} successful</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Success Rate</CardTitle>
            <CheckCircle2 className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {executions.length ? Math.round((executions.filter(e => e.status === 'Completed').length / executions.length) * 100) : 0}%
            </div>
            <p className="text-xs text-muted-foreground">Last 24 hours</p>
          </CardContent>
        </Card>
      </div>

      <div className="grid md:grid-cols-2 gap-6 mt-8">
        <div>
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-semibold">Recent Workflows</h2>
            <Link href="/dashboard/automation/workflows">
              <Button variant="ghost" size="sm">View All</Button>
            </Link>
          </div>
          
          {loadingWorkflows ? (
            <div className="space-y-3">
              {[1,2,3].map(i => <Card key={i} className="h-16 animate-pulse bg-muted/20" />)}
            </div>
          ) : (
            <div className="space-y-3">
              {workflows.slice(0, 3).map((wf) => (
                <div key={wf.id} className="flex items-center justify-between p-3 border rounded-lg bg-card">
                  <div className="flex items-center gap-3">
                    <div className="p-2 bg-primary/10 rounded-md">
                      <Zap className="h-4 w-4 text-primary" />
                    </div>
                    <div>
                      <p className="font-medium text-sm">{wf.name}</p>
                      <p className="text-xs text-muted-foreground">{wf.status}</p>
                    </div>
                  </div>
                  <Link href={`/dashboard/automation/workflows/${wf.id}/edit`}>
                    <Button variant="outline" size="sm">Edit</Button>
                  </Link>
                </div>
              ))}
            </div>
          )}
        </div>

        <div>
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-semibold">Recent Executions</h2>
            <Link href="/dashboard/automation/history">
              <Button variant="ghost" size="sm">View All</Button>
            </Link>
          </div>
          
          {loadingExecutions ? (
            <div className="space-y-3">
              {[1,2,3].map(i => <Card key={i} className="h-16 animate-pulse bg-muted/20" />)}
            </div>
          ) : (
            <div className="space-y-3">
              {executions.slice(0, 3).map((ex) => (
                <div key={ex.id} className="flex items-center justify-between p-3 border rounded-lg bg-card">
                  <div className="flex flex-col">
                    <p className="font-medium text-sm">{ex.workflowName}</p>
                    <p className="text-xs text-muted-foreground">{new Date(ex.startedAt).toLocaleString()}</p>
                  </div>
                  <Badge variant={ex.status === 'Completed' ? 'default' : ex.status === 'Failed' ? 'destructive' : 'secondary'}>
                    {ex.status}
                  </Badge>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
