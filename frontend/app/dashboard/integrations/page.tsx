"use client";

import React, { useState } from "react";
import { Globe, RefreshCw, Activity, Search, CheckCircle2, AlertCircle, PlayCircle, Unplug } from "lucide-react";
import { useIntegrations, useDeleteIntegration, useTestIntegration } from "@/hooks/use-integrations";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { formatDistanceToNow } from "date-fns";

export default function IntegrationsPage() {
  const [search, setSearch] = useState("");
  const { data: activeIntegrations, isLoading } = useIntegrations();
  const deleteIntegration = useDeleteIntegration();
  const testIntegration = useTestIntegration();

  const filteredIntegrations = activeIntegrations?.filter(
    (i) => i.displayName.toLowerCase().includes(search.toLowerCase()) || 
           i.provider.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8">
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <div className="flex items-center gap-2">
            <h1 className="text-2xl font-bold tracking-tight text-foreground lg:text-3xl flex items-center gap-2">
              <Globe className="h-8 w-8 text-primary" /> Integration Center
            </h1>
            <span className="inline-flex items-center gap-1 rounded-full bg-emerald-500/10 px-2.5 py-0.5 text-xs font-semibold text-emerald-500">
              <CheckCircle2 className="h-3.5 w-3.5" />
              AES-256
            </span>
          </div>
          <p className="mt-1 text-sm text-muted-foreground max-w-2xl">
            Monitor and manage your active application connections, sync history, and health status.
          </p>
        </div>

        <div className="relative w-full max-w-xs">
          <Search className="absolute left-3.5 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <input
            type="text"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Search active connections..."
            className="w-full rounded-xl border border-border bg-card pl-10 pr-4 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 shadow-sm"
          />
        </div>
      </div>

      {isLoading ? (
        <div className="p-12 text-center text-sm text-muted-foreground">Loading active integrations...</div>
      ) : filteredIntegrations?.length === 0 ? (
        <div className="flex flex-col items-center justify-center rounded-xl border border-dashed p-12 text-center">
          <Unplug className="mb-4 h-10 w-10 text-muted-foreground/50" />
          <h3 className="text-lg font-medium text-foreground">No active integrations</h3>
          <p className="mt-1 text-sm text-muted-foreground max-w-md">
            You haven't connected any apps yet. Head over to the Connector Marketplace to discover and install applications.
          </p>
          <a href="/dashboard/connectors">
            <Button className="mt-6">
              Go to Marketplace
            </Button>
          </a>
        </div>
      ) : (
        <div className="grid gap-6 sm:grid-cols-2 xl:grid-cols-3">
          {filteredIntegrations?.map((integration) => (
            <Card key={integration.id} className="flex flex-col">
              <CardHeader className="pb-4">
                <div className="flex items-start justify-between">
                  <div className="flex items-center gap-3">
                    <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-muted border">
                      <span className="font-bold text-primary text-sm">{integration.provider.substring(0, 2).toUpperCase()}</span>
                    </div>
                    <div>
                      <CardTitle className="text-base">{integration.displayName}</CardTitle>
                      <CardDescription>{integration.provider}</CardDescription>
                    </div>
                  </div>
                  <Badge variant={integration.status === "Active" ? "default" : "destructive"} className={integration.status === "Active" ? "bg-emerald-500 hover:bg-emerald-600" : ""}>
                    {integration.status}
                  </Badge>
                </div>
              </CardHeader>
              
              <CardContent className="space-y-4 text-sm mt-2">
                <div className="flex items-center justify-between rounded-md bg-muted/50 px-3 py-2">
                  <div className="flex items-center gap-2 text-muted-foreground">
                    <Activity className="h-4 w-4" />
                    <span>Health Status</span>
                  </div>
                  <span className="font-medium flex items-center gap-1.5 text-emerald-600">
                    <div className="h-2 w-2 rounded-full bg-emerald-500"></div> Healthy
                  </span>
                </div>
                
                <div className="flex justify-between text-muted-foreground">
                  <span>Connected</span>
                  <span className="text-foreground">{formatDistanceToNow(new Date(integration.createdAt), { addSuffix: true })}</span>
                </div>
              </CardContent>
              
              <CardFooter className="flex gap-2 pt-4 mt-auto border-t">
                <Button 
                  variant="outline" 
                  className="flex-1"
                  onClick={() => testIntegration.mutate(integration.id)}
                  disabled={testIntegration.isPending}
                >
                  <RefreshCw className={`mr-2 h-4 w-4 ${testIntegration.isPending ? "animate-spin" : ""}`} />
                  Test Sync
                </Button>
                <Button 
                  variant="destructive" 
                  className="flex-none px-3"
                  onClick={() => {
                    if (confirm("Disconnect this integration? Data will no longer sync.")) {
                      deleteIntegration.mutate(integration.id);
                    }
                  }}
                  disabled={deleteIntegration.isPending}
                  title="Disconnect"
                >
                  <Unplug className="h-4 w-4" />
                </Button>
              </CardFooter>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
