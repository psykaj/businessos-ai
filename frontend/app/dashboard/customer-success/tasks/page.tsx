"use client";

import React, { useState } from "react";
import {
  useSuccessTasksPaged,
  useCreateSuccessTask,
  useUpdateSuccessTask,
  useAutoGenerateTasks,
} from "@/hooks/use-customer-success";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogFooter } from "@/components/ui/dialog";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { CheckSquare, Plus, RefreshCw, Clock, AlertTriangle, CheckCircle2, User, Calendar, ShieldAlert } from "lucide-react";

export default function CustomerSuccessTasksPage() {
  const [statusTab, setStatusTab] = useState<string>("Pending");
  const [priorityFilter, setPriorityFilter] = useState<string>("");

  const { data: tasksData, isLoading } = useSuccessTasksPaged({
    status: statusTab !== "All" ? statusTab : undefined,
    priority: priorityFilter || undefined,
    pageSize: 20,
  });

  // Create Modal State
  const [createOpen, setCreateOpen] = useState(false);
  const [targetCustomerId, setTargetCustomerId] = useState("");
  const [taskTitle, setTaskTitle] = useState("");
  const [taskDesc, setTaskDesc] = useState("");
  const [taskType, setTaskType] = useState("FollowUpInactive");
  const [priority, setPriority] = useState("High");

  const createTaskMutation = useCreateSuccessTask();
  const updateTaskMutation = useUpdateSuccessTask();
  const autoGenerateMutation = useAutoGenerateTasks();

  const handleCreateTask = (e: React.FormEvent) => {
    e.preventDefault();
    if (!targetCustomerId || !taskTitle) return;
    createTaskMutation.mutate(
      {
        customerId: targetCustomerId,
        taskType,
        title: taskTitle,
        description: taskDesc,
        priority,
      },
      {
        onSuccess: () => {
          setCreateOpen(false);
          setTargetCustomerId("");
          setTaskTitle("");
          setTaskDesc("");
        },
      }
    );
  };

  const handleMarkCompleted = (id: string, currentTitle: string, currentPriority: string) => {
    updateTaskMutation.mutate({
      id,
      data: {
        title: currentTitle,
        priority: currentPriority,
        status: "Completed",
      },
    });
  };

  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      {/* Top Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <CheckSquare className="h-8 w-8 text-purple-600" />
            Customer Success Tasks
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Automated task assignment for low CSAT alerts, high-risk churn follow-ups, and VIP milestones.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button
            variant="outline"
            size="sm"
            onClick={() => autoGenerateMutation.mutate()}
            disabled={autoGenerateMutation.isPending}
            className="gap-2"
          >
            <RefreshCw className={`h-4 w-4 ${autoGenerateMutation.isPending ? "animate-spin" : ""}`} />
            Auto-Generate Retention Tasks
          </Button>

          {/* Dialog: Create Success Task */}
          <Dialog open={createOpen} onOpenChange={setCreateOpen}>
            <DialogTrigger
              render={
                <Button size="sm" className="gap-2 bg-purple-600 hover:bg-purple-700 text-white">
                  <Plus className="h-4 w-4" /> Create Success Task
                </Button>
              }
            />

            <DialogContent className="sm:max-w-[460px]">
              <DialogHeader>
                <DialogTitle>Create Success Task</DialogTitle>
              </DialogHeader>
              <form onSubmit={handleCreateTask} className="space-y-4 py-2">
                <div className="space-y-2">
                  <Label htmlFor="cId">Customer ID (Guid)</Label>
                  <Input
                    id="cId"
                    placeholder="Paste Customer Guid..."
                    value={targetCustomerId}
                    onChange={(e) => setTargetCustomerId(e.target.value)}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="titleVal">Task Title</Label>
                  <Input
                    id="titleVal"
                    placeholder="e.g. Outreach to dissatisfied customer"
                    value={taskTitle}
                    onChange={(e) => setTaskTitle(e.target.value)}
                    required
                  />
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="tType">Task Type</Label>
                    <Input
                      id="tType"
                      placeholder="FollowUpInactive, ContactDissatisfied"
                      value={taskType}
                      onChange={(e) => setTaskType(e.target.value)}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="prio">Priority</Label>
                    <Input
                      id="prio"
                      placeholder="Low, Medium, High, Critical"
                      value={priority}
                      onChange={(e) => setPriority(e.target.value)}
                    />
                  </div>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="descVal">Description</Label>
                  <Input
                    id="descVal"
                    placeholder="Outreach details and action plan..."
                    value={taskDesc}
                    onChange={(e) => setTaskDesc(e.target.value)}
                  />
                </div>
                <DialogFooter className="pt-2">
                  <Button type="submit" disabled={createTaskMutation.isPending} className="w-full bg-purple-600 text-white">
                    Save Task
                  </Button>
                </DialogFooter>
              </form>
            </DialogContent>
          </Dialog>
        </div>
      </div>

      {/* Tabs & Priority Filter */}
      <Tabs defaultValue="Pending" onValueChange={setStatusTab} className="space-y-4">
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <TabsList className="bg-slate-100 dark:bg-slate-900 border border-slate-200 dark:border-slate-800">
            <TabsTrigger value="Pending">Pending</TabsTrigger>
            <TabsTrigger value="InProgress">In Progress</TabsTrigger>
            <TabsTrigger value="Completed">Completed</TabsTrigger>
            <TabsTrigger value="All">All Tasks</TabsTrigger>
          </TabsList>

          <div className="flex items-center gap-2">
            <Button
              variant={priorityFilter === "" ? "default" : "outline"}
              size="sm"
              onClick={() => setPriorityFilter("")}
              className="text-xs"
            >
              All Priorities
            </Button>
            {["High", "Critical"].map((p) => (
              <Button
                key={p}
                variant={priorityFilter === p ? "default" : "outline"}
                size="sm"
                onClick={() => setPriorityFilter(p)}
                className="text-xs"
              >
                {p} Only
              </Button>
            ))}
          </div>
        </div>

        {/* Task List Table */}
        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-0">
            <Table>
              <TableHeader>
                <TableRow className="border-slate-200 dark:border-slate-800">
                  <TableHead>Task Title</TableHead>
                  <TableHead>Customer</TableHead>
                  <TableHead>Type</TableHead>
                  <TableHead>Priority</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead className="text-right">Action</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {tasksData?.items.length ? (
                  tasksData.items.map((task) => (
                    <TableRow key={task.id} className="border-slate-100 dark:border-slate-800/60">
                      <TableCell className="font-semibold text-slate-900 dark:text-white">
                        <div>
                          <p>{task.title}</p>
                          <p className="text-xs text-slate-500 font-normal">{task.description}</p>
                        </div>
                      </TableCell>
                      <TableCell className="text-sm">{task.customerName || "Customer"}</TableCell>
                      <TableCell>
                        <Badge variant="outline" className="text-xs">{task.taskType}</Badge>
                      </TableCell>
                      <TableCell>
                        <Badge
                          variant="outline"
                          className={
                            task.priority === "Critical" || task.priority === "High"
                              ? "bg-rose-500/10 text-rose-600 border-rose-500/30"
                              : "bg-purple-500/10 text-purple-600 border-purple-500/30"
                          }
                        >
                          {task.priority}
                        </Badge>
                      </TableCell>
                      <TableCell>
                        <Badge
                          variant="outline"
                          className={
                            task.status === "Completed"
                              ? "bg-emerald-500/10 text-emerald-600 border-emerald-500/30"
                              : "bg-amber-500/10 text-amber-600 border-amber-500/30"
                          }
                        >
                          {task.status}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-right">
                        {task.status !== "Completed" && (
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => handleMarkCompleted(task.id, task.title, task.priority)}
                            className="text-xs text-emerald-600 hover:text-emerald-700 gap-1"
                          >
                            <CheckCircle2 className="h-3.5 w-3.5" /> Mark Done
                          </Button>
                        )}
                      </TableCell>
                    </TableRow>
                  ))
                ) : (
                  <TableRow>
                    <TableCell colSpan={6} className="text-center py-6 text-slate-500 text-sm">
                      No success tasks found matching tab filter.
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </CardContent>
        </Card>
      </Tabs>
    </div>
  );
}
