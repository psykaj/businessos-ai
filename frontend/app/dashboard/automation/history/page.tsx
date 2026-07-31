"use client";

import { useAutomationExecutions } from "@/hooks/useAutomationStudio";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { RotateCcw } from "lucide-react";
import { formatDistanceToNow } from "date-fns";

export default function HistoryPage() {
  const { data: executions = [], isLoading } = useAutomationExecutions();

  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Execution History</h1>
        <p className="text-muted-foreground">Monitor workflow runs and troubleshoot failures.</p>
      </div>

      <div className="border rounded-md bg-card">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Workflow</TableHead>
              <TableHead>Trigger</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Started</TableHead>
              <TableHead>Error Details</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {isLoading ? (
              <TableRow>
                <TableCell colSpan={6} className="text-center h-24 text-muted-foreground">Loading executions...</TableCell>
              </TableRow>
            ) : executions.length === 0 ? (
              <TableRow>
                <TableCell colSpan={6} className="text-center h-24 text-muted-foreground">No executions found.</TableCell>
              </TableRow>
            ) : (
              executions.map((ex) => (
                <TableRow key={ex.id}>
                  <TableCell className="font-medium">{ex.workflowName}</TableCell>
                  <TableCell className="text-muted-foreground text-xs">{ex.triggerSource}</TableCell>
                  <TableCell>
                    <Badge variant={ex.status === 'Completed' ? 'default' : ex.status === 'Failed' ? 'destructive' : 'secondary'}>
                      {ex.status}
                    </Badge>
                  </TableCell>
                  <TableCell className="text-xs">
                    {formatDistanceToNow(new Date(ex.startedAt), { addSuffix: true })}
                  </TableCell>
                  <TableCell className="text-xs text-destructive max-w-[200px] truncate">
                    {ex.errorDetails || "-"}
                  </TableCell>
                  <TableCell className="text-right">
                    {ex.status === 'Failed' && (
                      <Button variant="ghost" size="sm" className="gap-2 text-xs">
                        <RotateCcw className="h-3 w-3" /> Retry
                      </Button>
                    )}
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
