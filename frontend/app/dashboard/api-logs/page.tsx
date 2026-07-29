"use client";

import { Terminal, Download, Search, Filter } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";

export default function ApiLogsPage() {
  return (
    <div className="mx-auto max-w-6xl space-y-6 p-6 lg:p-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
            <Terminal className="h-6 w-6 text-purple-500" /> API Logs
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Audit trail of incoming requests to your organization. Logs are retained for 7 days.
          </p>
        </div>
        
        <Button variant="outline">
          <Download className="mr-2 h-4 w-4" />
          Export Logs
        </Button>
      </div>

      <Card>
        <CardHeader className="pb-4">
          <div className="flex items-center justify-between">
            <CardTitle>Recent Traffic</CardTitle>
            <div className="flex gap-2">
              <div className="relative">
                <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                <Input placeholder="Search endpoints or status..." className="h-9 w-64 pl-9" />
              </div>
              <Button variant="outline" size="sm" className="h-9">
                <Filter className="mr-2 h-4 w-4" /> Filter
              </Button>
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <div className="flex h-64 flex-col items-center justify-center rounded-lg border border-dashed text-center">
            <Terminal className="mb-4 h-8 w-8 text-muted-foreground/40" />
            <h3 className="mb-1 text-lg font-medium">No API traffic recorded</h3>
            <p className="mb-4 text-sm text-muted-foreground">Make requests to the API using an API key to see them appear here.</p>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
