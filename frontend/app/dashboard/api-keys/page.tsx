"use client";

import { useState } from "react";
import { useAuth } from "@/contexts/auth-context";
import { useApiKeys, useCreateApiKey, useRevokeApiKey, useRotateApiKey } from "@/hooks/use-apikeys";
import { ApiKey, CreateApiKeyRequest } from "@/types/apikeys";
import { toast } from "sonner";
import { format } from "date-fns";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";

import { Button } from "@/components/ui/button";
import { DataTable } from "@/components/ui/data-table";
import { StatusBadge } from "@/components/ui/status-badge";
import { CopyToClipboard } from "@/components/ui/copy-to-clipboard";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { KeyRound, MoreHorizontal, Plus, RefreshCw, Trash2, AlertTriangle, ShieldAlert } from "lucide-react";

const createKeySchema = z.object({
  name: z.string().min(2, "Name must be at least 2 characters."),
  expiresInDays: z.number().min(0).optional(),
});

type CreateKeyFormValues = z.infer<typeof createKeySchema>;

export default function ApiKeysPage() {
  const { user } = useAuth();
  const orgId = user?.organizationId;
  const { data: apiKeys, isLoading } = useApiKeys(orgId);
  const createKey = useCreateApiKey();
  const revokeKey = useRevokeApiKey();
  const rotateKey = useRotateApiKey();

  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [newSecretKey, setNewSecretKey] = useState<string | null>(null);

  const form = useForm<CreateKeyFormValues>({
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    resolver: zodResolver(createKeySchema) as any,
    defaultValues: {
      name: "",
      expiresInDays: 0,
    },
  });

  const onSubmit = async (data: CreateKeyFormValues) => {
    if (!orgId) return;
    try {
      const response = await createKey.mutateAsync({ 
        orgId, 
        data: { name: data.name, expiresInDays: data.expiresInDays || undefined } 
      });
      setNewSecretKey(response.plainTextKey);
      toast.success("API key created successfully.");
      form.reset();
    } catch (error) {
      toast.error("Failed to create API key.");
    }
  };

  const handleRevoke = async (keyId: string) => {
    if (!orgId) return;
    if (confirm("Are you sure you want to revoke this API key? This action cannot be undone and any applications using it will lose access immediately.")) {
      try {
        await revokeKey.mutateAsync({ orgId, keyId });
        toast.success("API key revoked");
      } catch (e) {
        toast.error("Failed to revoke API key");
      }
    }
  };

  const handleRotate = async (keyId: string) => {
    if (!orgId) return;
    if (confirm("Rotate this API key? The old key will be revoked immediately and a new secret will be generated.")) {
      try {
        const response = await rotateKey.mutateAsync({ orgId, keyId });
        setNewSecretKey(response.plainTextKey);
        setCreateDialogOpen(true); // Open dialog to show new secret
        toast.success("API key rotated");
      } catch (e) {
        toast.error("Failed to rotate API key");
      }
    }
  };

  const closeDialog = () => {
    setCreateDialogOpen(false);
    setNewSecretKey(null);
    form.reset();
  };

  const columns = [
    {
      header: "Name",
      cell: (item: ApiKey) => (
        <div className="flex items-center gap-2">
          <KeyRound className="h-4 w-4 text-muted-foreground" />
          <span className="font-medium">{item.name}</span>
        </div>
      )
    },
    {
      header: "Key Prefix",
      cell: (item: ApiKey) => <code className="rounded bg-muted px-2 py-1 font-mono text-xs">{item.keyPrefix}...</code>
    },
    {
      header: "Status",
      cell: (item: ApiKey) => <StatusBadge status={item.isActive ? "Active" : "Revoked"} />
    },
    {
      header: "Created",
      cell: (item: ApiKey) => (
        <span className="text-sm text-muted-foreground">
          {format(new Date(item.createdAt), "MMM d, yyyy")}
        </span>
      )
    },
    {
      header: "Last Used",
      cell: (item: ApiKey) => (
        <span className="text-sm text-muted-foreground">
          {item.lastUsedAt ? format(new Date(item.lastUsedAt), "MMM d, yyyy") : "Never"}
        </span>
      )
    },
    {
      header: "",
      cell: (item: ApiKey) => (
        <DropdownMenu>
          <DropdownMenuTrigger render={
            <Button variant="ghost" className="h-8 w-8 p-0" disabled={!item.isActive}>
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          } />
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuItem onClick={() => handleRotate(item.id)}>
              <RefreshCw className="mr-2 h-4 w-4" />
              Rotate Key
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem onClick={() => handleRevoke(item.id)} className="text-destructive">
              <Trash2 className="mr-2 h-4 w-4" />
              Revoke Key
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    }
  ];

  return (
    <div className="mx-auto max-w-5xl space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground">API Keys</h1>
          <p className="mt-1 text-sm text-muted-foreground">Manage API keys for programmatic access to the Simplify API.</p>
        </div>
        
        <Dialog open={createDialogOpen} onOpenChange={(open) => !open && closeDialog()}>
          <DialogTrigger render={
            <Button onClick={() => setCreateDialogOpen(true)}>
              <Plus className="mr-2 h-4 w-4" />
              Generate Key
            </Button>
          } />
          <DialogContent className="sm:max-w-[425px]">
            {newSecretKey ? (
              <>
                <DialogHeader>
                  <DialogTitle>Save your secret key</DialogTitle>
                  <DialogDescription className="text-destructive font-medium flex items-center gap-2 mt-2">
                    <ShieldAlert className="h-4 w-4" />
                    This key will only be shown once.
                  </DialogDescription>
                </DialogHeader>
                <div className="py-4 space-y-4">
                  <p className="text-sm">Please copy this secret key and store it securely. You will not be able to see it again.</p>
                  <div className="flex items-center gap-2 p-3 bg-muted rounded-md font-mono text-sm break-all">
                    <span className="flex-1 select-all">{newSecretKey}</span>
                    <CopyToClipboard text={newSecretKey} />
                  </div>
                </div>
                <DialogFooter>
                  <Button onClick={closeDialog} className="w-full">I&apos;ve saved it securely</Button>
                </DialogFooter>
              </>
            ) : (
              <>
                <DialogHeader>
                  <DialogTitle>Generate API Key</DialogTitle>
                  <DialogDescription>
                    Create a new API key for integrating with Simplify.
                  </DialogDescription>
                </DialogHeader>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 py-4">
                  <div className="space-y-2">
                    <Label htmlFor="name">Key Name</Label>
                    <Input id="name" placeholder="e.g. Production Server, Zapier Integration" {...form.register("name")} />
                    {form.formState.errors.name && (
                      <p className="text-sm text-destructive">{form.formState.errors.name.message}</p>
                    )}
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="expiresInDays">Expiration (Days, optional)</Label>
                    <Input id="expiresInDays" type="number" placeholder="Leave blank for no expiration" {...form.register("expiresInDays", { setValueAs: (v) => v === "" || v === null || v === undefined ? undefined : Number(v) })} />
                  </div>
                  <DialogFooter className="pt-4">
                    <Button type="button" variant="outline" onClick={closeDialog}>Cancel</Button>
                    <Button type="submit" disabled={createKey.isPending}>
                      Generate
                    </Button>
                  </DialogFooter>
                </form>
              </>
            )}
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Organization API Keys</CardTitle>
          <CardDescription>Active and revoked keys for your organization.</CardDescription>
        </CardHeader>
        <CardContent>
          {apiKeys?.length === 0 ? (
            <div className="flex h-48 flex-col items-center justify-center rounded-lg border border-dashed text-center">
              <KeyRound className="mb-4 h-8 w-8 text-muted-foreground/40" />
              <h3 className="mb-1 text-lg font-medium">No API keys yet</h3>
              <p className="mb-4 text-sm text-muted-foreground">Generate a key to start integrating your applications.</p>
              <Button onClick={() => setCreateDialogOpen(true)} variant="outline">Generate Key</Button>
            </div>
          ) : (
            <DataTable
              data={apiKeys || []}
              columns={columns}
              isLoading={isLoading}
            />
          )}
        </CardContent>
      </Card>
    </div>
  );
}
