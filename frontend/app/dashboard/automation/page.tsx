"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import { Zap, Plus, Activity, CheckCircle2, PlayCircle, Settings2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { getAutomationRules, AutomationRule } from "@/lib/api/automation";

export default function AutomationDashboardPage() {
  const [rules, setRules] = useState<AutomationRule[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchRules = async () => {
      try {
        const data = await getAutomationRules();
        setRules(data);
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    };
    fetchRules();
  }, []);

  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Automation Engine</h1>
          <p className="text-muted-foreground">Build and manage event-driven workflows.</p>
        </div>
        <Link href="/dashboard/automation/create">
          <Button className="gap-2">
            <Plus className="h-4 w-4" />
            Create Workflow
          </Button>
        </Link>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Active Workflows</CardTitle>
            <Activity className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{rules.filter(r => r.isEnabled).length}</div>
            <p className="text-xs text-muted-foreground">Out of {rules.length} total</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Executions (24h)</CardTitle>
            <PlayCircle className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">1,245</div>
            <p className="text-xs text-emerald-500">All successful</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Success Rate</CardTitle>
            <CheckCircle2 className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">99.8%</div>
            <p className="text-xs text-muted-foreground">+0.2% from last week</p>
          </CardContent>
        </Card>
      </div>

      <h2 className="text-xl font-semibold mt-8 mb-4">Your Workflows</h2>

      {loading ? (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map(i => (
            <Card key={i} className="animate-pulse h-40 bg-muted/20" />
          ))}
        </div>
      ) : rules.length === 0 ? (
        <Card>
          <CardContent className="p-12 text-center text-muted-foreground flex flex-col items-center gap-4">
            <Zap className="h-12 w-12 text-muted-foreground/30" />
            <div>
              <h3 className="font-semibold text-lg text-foreground mb-1">No workflows found</h3>
              <p>Create your first automation workflow to start saving time.</p>
            </div>
            <Link href="/dashboard/automation/create">
              <Button variant="outline" className="mt-2">Create Workflow</Button>
            </Link>
          </CardContent>
        </Card>
      ) : (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {rules.map(rule => (
            <Card key={rule.id} className="flex flex-col hover:border-primary/50 transition-colors">
              <CardHeader className="pb-3">
                <div className="flex justify-between items-start mb-2">
                  <Badge variant={rule.isEnabled ? "default" : "secondary"}>
                    {rule.isEnabled ? "Active" : "Paused"}
                  </Badge>
                  <Button variant="ghost" size="icon" className="h-8 w-8 text-muted-foreground"><Settings2 className="h-4 w-4" /></Button>
                </div>
                <CardTitle className="text-lg">{rule.name}</CardTitle>
                <CardDescription className="truncate font-mono text-xs mt-1">
                  Trigger: {rule.trigger}
                </CardDescription>
              </CardHeader>
              <CardContent className="mt-auto pt-4 border-t text-xs text-muted-foreground flex justify-between items-center">
                <span>Created {new Date(rule.createdAt).toLocaleDateString()}</span>
                <Link href={`/dashboard/automation/${rule.id}/logs`} className="hover:text-primary transition-colors">
                  View Logs
                </Link>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
