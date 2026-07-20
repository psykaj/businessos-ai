"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableHead, 
  TableHeader, 
  TableRow 
} from "@/components/ui/table";
import { Plus, CheckSquare, Calendar as CalendarIcon, AlertCircle } from "lucide-react";
import { formatDate } from "@/lib/utils";
import { CrmTaskStatus, TaskPriority } from "@/types/crm";
import { Badge } from "@/components/ui/badge";

const getStatusBadge = (status: CrmTaskStatus) => {
  switch (status) {
    case CrmTaskStatus.Pending: return <Badge variant="outline" className="bg-slate-50">Pending</Badge>;
    case CrmTaskStatus.InProgress: return <Badge variant="outline" className="bg-blue-50 text-blue-700">In Progress</Badge>;
    case CrmTaskStatus.Completed: return <Badge variant="outline" className="bg-green-50 text-green-700">Completed</Badge>;
    case CrmTaskStatus.Cancelled: return <Badge variant="outline" className="bg-red-50 text-red-700">Cancelled</Badge>;
    default: return <Badge variant="outline">Unknown</Badge>;
  }
};

const getPriorityBadge = (priority: TaskPriority) => {
  switch (priority) {
    case TaskPriority.Low: return <span className="text-slate-500 font-medium">Low</span>;
    case TaskPriority.Medium: return <span className="text-blue-600 font-medium">Medium</span>;
    case TaskPriority.High: return <span className="text-orange-600 font-medium">High</span>;
    case TaskPriority.Urgent: return <span className="text-red-600 font-bold flex items-center gap-1"><AlertCircle className="h-3 w-3" /> Urgent</span>;
    default: return <span>Unknown</span>;
  }
};

export default function TasksPage() {
  const { data: tasks, isLoading } = useQuery({
    queryKey: ['crm-tasks'],
    queryFn: () => crmService.getTasks()
  });

  return (
    <div className="p-8 space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Tasks</h1>
          <p className="text-muted-foreground">Manage your follow-ups and to-dos.</p>
        </div>
        <Button>
          <Plus className="mr-2 h-4 w-4" /> Add Task
        </Button>
      </div>

      <Card>
        <CardHeader className="pb-3">
          <CardTitle>All Tasks</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-4">
              {[1, 2, 3, 4, 5].map(i => <Skeleton key={i} className="h-12 w-full" />)}
            </div>
          ) : tasks && tasks.length > 0 ? (
            <div className="rounded-md border">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Task</TableHead>
                    <TableHead>Related To</TableHead>
                    <TableHead>Priority</TableHead>
                    <TableHead>Due Date</TableHead>
                    <TableHead>Status</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {tasks.map((task) => (
                    <TableRow key={task.id}>
                      <TableCell>
                        <div className="flex flex-col">
                          <span className="font-medium">{task.title}</span>
                          {task.description && (
                            <span className="text-xs text-muted-foreground line-clamp-1">{task.description}</span>
                          )}
                        </div>
                      </TableCell>
                      <TableCell>
                        <span className="text-sm">
                          {task.relatedEntity}
                        </span>
                      </TableCell>
                      <TableCell>{getPriorityBadge(task.priority)}</TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2 text-sm">
                          <CalendarIcon className="h-4 w-4 text-muted-foreground" />
                          {task.dueDate ? formatDate(task.dueDate) : '-'}
                        </div>
                      </TableCell>
                      <TableCell>{getStatusBadge(task.status)}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          ) : (
            <div className="text-center py-10 text-muted-foreground">
              <CheckSquare className="h-10 w-10 mx-auto text-muted-foreground/50 mb-3" />
              <p>No tasks found.</p>
              <Button variant="link" className="mt-2">Create your first task</Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
