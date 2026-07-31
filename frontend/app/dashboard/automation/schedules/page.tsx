"use client";

import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Clock } from "lucide-react";

export default function SchedulesPage() {
  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Schedules</h1>
        <p className="text-muted-foreground">Manage your cron-based workflow triggers.</p>
      </div>

      <Card>
        <CardContent className="p-12 text-center text-muted-foreground flex flex-col items-center gap-4">
          <Clock className="h-12 w-12 text-muted-foreground/30" />
          <div>
            <h3 className="font-semibold text-lg text-foreground mb-1">Coming Soon</h3>
            <p>A centralized dashboard for managing all scheduled cron triggers will be available in the next release.</p>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
