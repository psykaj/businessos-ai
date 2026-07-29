"use client";

import { useState } from "react";
import { Webhook, Plus, MoreHorizontal, Activity, RefreshCw, Trash2, Edit2, PlayCircle } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { DataTable } from "@/components/ui/data-table";
import { StatusBadge } from "@/components/ui/status-badge";
import { Badge } from "@/components/ui/badge";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useWebhooks, useCreateWebhook, useDeleteWebhook, useTestWebhook, WebhookEndpoint } from "@/hooks/use-webhooks";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { format } from "date-fns";
import { toast } from "sonner";

const webhookSchema = z.object({
  url: z.string().url("Must be a valid HTTPS URL.").startsWith("https://", "URL must use HTTPS"),
  description: z.string().optional(),
  eventTypes: z.string(), // We'll split this by comma
});

export default function WebhooksPage() {
  const { data: webhooks, isLoading } = useWebhooks();
  const createWebhook = useCreateWebhook();
  const deleteWebhook = useDeleteWebhook();
  const testWebhook = useTestWebhook();

  const [createDialogOpen, setCreateDialogOpen] = useState(false);

  const form = useForm<z.infer<typeof webhookSchema>>({
    resolver: zodResolver(webhookSchema) as any,
    defaultValues: {
      url: "",
      description: "",
      eventTypes: "*",
    },
  });

  const onSubmit = async (data: z.infer<typeof webhookSchema>) => {
    createWebhook.mutate({
      url: data.url,
      description: data.description,
      eventTypes: data.eventTypes.split(",").map((s) => s.trim()),
      isActive: true,
      secret: crypto.randomUUID().replace(/-/g, ""), // Generate a random secret for them
    }, {
      onSuccess: () => {
        setCreateDialogOpen(false);
        form.reset();
      }
    });
  };

  const columns = [
    {
      header: "Endpoint URL",
      cell: (item: WebhookEndpoint) => (
        <div className="flex flex-col gap-1">
          <span className="font-medium truncate max-w-[250px]" title={item.url}>{item.url}</span>
          {item.description && <span className="text-xs text-muted-foreground">{item.description}</span>}
        </div>
      )
    },
    {
      header: "Status",
      cell: (item: WebhookEndpoint) => <StatusBadge status={item.isActive ? "Active" : "Inactive"} />
    },
    {
      header: "Events",
      cell: (item: WebhookEndpoint) => (
        <div className="flex flex-wrap gap-1 max-w-[200px]">
          {item.eventTypes.map(e => (
            <Badge key={e} variant="secondary" className="text-[10px]">{e}</Badge>
          ))}
        </div>
      )
    },
    {
      header: "Last Delivery",
      cell: (item: WebhookEndpoint) => (
        <div className="flex flex-col gap-1">
          {item.lastDeliveryAt ? (
            <>
              <span className="text-sm">{format(new Date(item.lastDeliveryAt), "MMM d, yyyy HH:mm")}</span>
              <span className={`text-xs ${item.lastDeliveryStatus === "Success" ? "text-emerald-500" : "text-destructive"}`}>
                {item.lastDeliveryStatus || "Unknown"}
              </span>
            </>
          ) : (
            <span className="text-sm text-muted-foreground">Never</span>
          )}
        </div>
      )
    },
    {
      header: "",
      cell: (item: WebhookEndpoint) => (
        <DropdownMenu>
          <DropdownMenuTrigger render={
            <Button variant="ghost" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          } />
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuItem onClick={() => testWebhook.mutate(item.id)}>
              <PlayCircle className="mr-2 h-4 w-4" />
              Test Endpoint
            </DropdownMenuItem>
            <DropdownMenuItem onClick={() => toast.info("View Logs coming soon")}>
              <Activity className="mr-2 h-4 w-4" />
              View Logs
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem 
              onClick={() => {
                if (confirm("Delete this webhook? Deliveries will stop immediately.")) {
                  deleteWebhook.mutate(item.id);
                }
              }} 
              className="text-destructive"
            >
              <Trash2 className="mr-2 h-4 w-4" />
              Delete Webhook
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    }
  ];

  return (
    <div className="mx-auto max-w-5xl space-y-6 p-6 lg:p-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
            <Webhook className="h-6 w-6 text-emerald-500" /> Webhooks
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Configure HTTP callbacks to receive real-time updates when events occur in your organization.
          </p>
        </div>
        
        <Dialog open={createDialogOpen} onOpenChange={setCreateDialogOpen}>
          <Button onClick={() => setCreateDialogOpen(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Add Endpoint
          </Button>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Add Webhook Endpoint</DialogTitle>
              <DialogDescription>
                We will send POST requests to this URL when events occur.
              </DialogDescription>
            </DialogHeader>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 py-4">
              <div className="space-y-2">
                <Label htmlFor="url">Endpoint URL</Label>
                <Input id="url" placeholder="https://api.yourdomain.com/webhooks" {...form.register("url")} />
                {form.formState.errors.url && <p className="text-xs text-destructive">{form.formState.errors.url.message as string}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="description">Description (Optional)</Label>
                <Input id="description" placeholder="e.g. Production Billing Sync" {...form.register("description")} />
              </div>
              <div className="space-y-2">
                <Label htmlFor="eventTypes">Event Types</Label>
                <Input id="eventTypes" placeholder="customer.created, invoice.paid" {...form.register("eventTypes")} />
                <p className="text-xs text-muted-foreground">Comma separated list of events, or * for all events.</p>
              </div>
              <DialogFooter className="pt-4">
                <Button type="button" variant="outline" onClick={() => setCreateDialogOpen(false)}>Cancel</Button>
                <Button type="submit" disabled={createWebhook.isPending}>Add Endpoint</Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Configured Endpoints</CardTitle>
          <CardDescription>All webhooks registered to receive events for this organization.</CardDescription>
        </CardHeader>
        <CardContent>
          {webhooks?.length === 0 ? (
            <div className="flex h-48 flex-col items-center justify-center rounded-lg border border-dashed text-center">
              <Webhook className="mb-4 h-8 w-8 text-muted-foreground/40" />
              <h3 className="mb-1 text-lg font-medium">No webhooks configured</h3>
              <p className="mb-4 text-sm text-muted-foreground">Create an endpoint to start receiving real-time events.</p>
              <Button onClick={() => setCreateDialogOpen(true)} variant="outline">Add Endpoint</Button>
            </div>
          ) : (
            <DataTable
              data={webhooks || []}
              columns={columns}
              isLoading={isLoading}
            />
          )}
        </CardContent>
      </Card>
    </div>
  );
}
