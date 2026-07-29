"use client";

import { Code2, ArrowUpRight, Terminal, Book, FileJson, Activity, Lock } from "lucide-react";
import Link from "next/link";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function DeveloperPortalPage() {
  return (
    <div className="mx-auto max-w-6xl space-y-8 p-6 lg:p-8">
      <div>
        <h1 className="text-3xl font-bold tracking-tight text-foreground flex items-center gap-2">
          <Code2 className="h-8 w-8 text-primary" /> Developer Portal
        </h1>
        <p className="mt-2 text-muted-foreground">
          Build powerful integrations, manage your API lifecycle, and monitor real-time event deliveries.
        </p>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <Card className="flex flex-col">
          <CardHeader>
            <Lock className="h-8 w-8 text-blue-500 mb-2" />
            <CardTitle>API Keys</CardTitle>
            <CardDescription>Manage authentication credentials for programmatic access.</CardDescription>
          </CardHeader>
          <CardContent className="mt-auto">
            <Link href="/dashboard/api-keys">
              <Button variant="outline" className="w-full">
                Manage Keys <ArrowUpRight className="ml-2 h-4 w-4" />
              </Button>
            </Link>
          </CardContent>
        </Card>

        <Card className="flex flex-col">
          <CardHeader>
            <Activity className="h-8 w-8 text-emerald-500 mb-2" />
            <CardTitle>Webhooks</CardTitle>
            <CardDescription>Configure endpoints to receive real-time event notifications.</CardDescription>
          </CardHeader>
          <CardContent className="mt-auto">
            <Link href="/dashboard/webhooks">
              <Button variant="outline" className="w-full">
                Manage Webhooks <ArrowUpRight className="ml-2 h-4 w-4" />
              </Button>
            </Link>
          </CardContent>
        </Card>

        <Card className="flex flex-col">
          <CardHeader>
            <Terminal className="h-8 w-8 text-purple-500 mb-2" />
            <CardTitle>API Logs</CardTitle>
            <CardDescription>View an audit trail of incoming requests and responses.</CardDescription>
          </CardHeader>
          <CardContent className="mt-auto">
            <Link href="/dashboard/api-logs">
              <Button variant="outline" className="w-full">
                View Logs <ArrowUpRight className="ml-2 h-4 w-4" />
              </Button>
            </Link>
          </CardContent>
        </Card>
      </div>

      <h2 className="text-xl font-semibold tracking-tight mt-12 mb-6">Documentation & Resources</h2>
      
      <div className="grid gap-4 md:grid-cols-2">
        <a href="#" className="flex items-center gap-4 rounded-xl border bg-card p-4 transition-colors hover:bg-muted/50">
          <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-primary/10">
            <Book className="h-6 w-6 text-primary" />
          </div>
          <div>
            <h3 className="font-semibold text-foreground">API Reference</h3>
            <p className="text-sm text-muted-foreground">Detailed endpoint specifications and schemas.</p>
          </div>
        </a>

        <a href="#" className="flex items-center gap-4 rounded-xl border bg-card p-4 transition-colors hover:bg-muted/50">
          <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-primary/10">
            <FileJson className="h-6 w-6 text-primary" />
          </div>
          <div>
            <h3 className="font-semibold text-foreground">SDKs & Client Libraries</h3>
            <p className="text-sm text-muted-foreground">Official wrappers for Node.js, Python, and .NET.</p>
          </div>
        </a>
      </div>
    </div>
  );
}
