"use client";

import { useState } from "react";
import { Plug, Search, CheckCircle2, Download, ArrowRight } from "lucide-react";
import { useConnectors, useInstallConnector } from "@/hooks/use-connectors";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

export default function ConnectorsPage() {
  const [search, setSearch] = useState("");
  const { data: connectors, isLoading } = useConnectors();
  const installConnector = useInstallConnector();

  const filteredConnectors = connectors?.filter(
    (c) => c.name.toLowerCase().includes(search.toLowerCase()) || 
           c.category.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8">
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <div className="flex items-center gap-2">
            <h1 className="text-2xl font-bold tracking-tight text-foreground lg:text-3xl flex items-center gap-2">
              <Plug className="h-8 w-8 text-primary" /> Connector Marketplace
            </h1>
          </div>
          <p className="mt-1 text-sm text-muted-foreground max-w-2xl">
            Discover and install third-party apps to extend your workspace capabilities. Connectors allow seamless data sync and automated workflows.
          </p>
        </div>

        <div className="relative w-full max-w-xs">
          <Search className="absolute left-3.5 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <input
            type="text"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Search apps or categories..."
            className="w-full rounded-xl border border-border bg-card pl-10 pr-4 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 shadow-sm"
          />
        </div>
      </div>

      {isLoading ? (
        <div className="p-12 text-center text-sm text-muted-foreground">Loading marketplace apps...</div>
      ) : (
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {filteredConnectors?.map((connector) => (
            <Card key={connector.id} className="flex flex-col overflow-hidden transition-all hover:shadow-md">
              <CardHeader className="pb-4">
                <div className="flex items-start justify-between">
                  <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-muted/50 border">
                    {/* Placeholder for actual logos */}
                    <span className="font-bold text-lg text-primary">{connector.name.charAt(0)}</span>
                  </div>
                  <Badge variant={connector.isInstalled ? "default" : "secondary"}>
                    {connector.isInstalled ? "Installed" : connector.category}
                  </Badge>
                </div>
                <CardTitle className="mt-4 text-lg">{connector.name}</CardTitle>
                <CardDescription className="line-clamp-2 mt-1 min-h-[40px]">
                  {connector.description}
                </CardDescription>
              </CardHeader>
              <CardContent className="mt-auto pb-4">
                {/* Additional metadata could go here */}
              </CardContent>
              <CardFooter className="pt-0">
                {connector.isInstalled ? (
                  <Button variant="outline" className="w-full text-emerald-600 bg-emerald-50 hover:bg-emerald-100 hover:text-emerald-700 border-emerald-200">
                    <CheckCircle2 className="mr-2 h-4 w-4" />
                    Configure App
                  </Button>
                ) : (
                  <Button 
                    variant="default" 
                    className="w-full"
                    onClick={() => installConnector.mutate(connector.id)}
                    disabled={installConnector.isPending}
                  >
                    <Download className="mr-2 h-4 w-4" />
                    Install App
                  </Button>
                )}
              </CardFooter>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
