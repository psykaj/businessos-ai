"use client";

import Link from "next/link";
import { Plus, Settings2, Zap } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { useAutomationWorkflows } from "@/hooks/useAutomationStudio";

export default function WorkflowsPage() {
  const { data: workflows = [], isLoading } = useAutomationWorkflows();

  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Workflows</h1>
          <p className="text-muted-foreground">Manage your automated business processes.</p>
        </div>
        <Link href="/dashboard/automation/workflows/new/edit">
          <Button className="gap-2">
            <Plus className="h-4 w-4" />
            Create Workflow
          </Button>
        </Link>
      </div>

      {isLoading ? (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map(i => (
            <Card key={i} className="animate-pulse h-40 bg-muted/20" />
          ))}
        </div>
      ) : workflows.length === 0 ? (
        <Card>
          <CardContent className="p-12 text-center text-muted-foreground flex flex-col items-center gap-4">
            <Zap className="h-12 w-12 text-muted-foreground/30" />
            <div>
              <h3 className="font-semibold text-lg text-foreground mb-1">No workflows found</h3>
              <p>Create your first automation workflow to start saving time.</p>
            </div>
            <Link href="/dashboard/automation/workflows/new/edit">
              <Button variant="outline" className="mt-2">Create Workflow</Button>
            </Link>
          </CardContent>
        </Card>
      ) : (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {workflows.map(rule => (
            <Card key={rule.id} className="flex flex-col hover:border-primary/50 transition-colors">
              <CardHeader className="pb-3">
                <div className="flex justify-between items-start mb-2">
                  <Badge variant={rule.status === 'Active' ? "default" : "secondary"}>
                    {rule.status}
                  </Badge>
                  <Button variant="ghost" size="icon" className="h-8 w-8 text-muted-foreground">
                    <Settings2 className="h-4 w-4" />
                  </Button>
                </div>
                <CardTitle className="text-lg">{rule.name}</CardTitle>
                <CardDescription className="truncate text-xs mt-1">
                  {rule.description || "No description"}
                </CardDescription>
              </CardHeader>
              <CardContent className="mt-auto pt-4 border-t text-xs text-muted-foreground flex justify-between items-center">
                <span>Created {new Date(rule.createdAt).toLocaleDateString()}</span>
                <Link href={`/dashboard/automation/workflows/${rule.id}/edit`} className="text-primary hover:underline">
                  Edit Builder
                </Link>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
