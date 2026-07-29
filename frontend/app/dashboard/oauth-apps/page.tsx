"use client";

import { useState } from "react";
import { ShieldCheck, Plus, Copy, MoreHorizontal, Settings, Trash2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { toast } from "sonner";
import { CopyToClipboard } from "@/components/ui/copy-to-clipboard";

export default function OAuthAppsPage() {
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [newAppCreds, setNewAppCreds] = useState<{ clientId: string; clientSecret: string } | null>(null);

  const handleCreate = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget);
    const name = formData.get("name") as string;
    
    if (!name) return;

    // Mock API call
    setNewAppCreds({
      clientId: `client_${crypto.randomUUID().replace(/-/g, "").substring(0, 16)}`,
      clientSecret: `secret_${crypto.randomUUID().replace(/-/g, "")}`,
    });
    toast.success("OAuth application registered successfully.");
  };

  const closeDialog = () => {
    setCreateDialogOpen(false);
    setNewAppCreds(null);
  };

  return (
    <div className="mx-auto max-w-5xl space-y-6 p-6 lg:p-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground flex items-center gap-2">
            <ShieldCheck className="h-6 w-6 text-primary" /> OAuth Apps
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Register your own applications to use BusinessOS AI as an identity provider or for accessing customer data.
          </p>
        </div>
        
        <Dialog open={createDialogOpen} onOpenChange={(open) => !open && closeDialog()}>
          <Button onClick={() => setCreateDialogOpen(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Register App
          </Button>
          <DialogContent className="sm:max-w-[425px]">
            {newAppCreds ? (
              <>
                <DialogHeader>
                  <DialogTitle>Application Credentials</DialogTitle>
                  <DialogDescription className="text-destructive font-medium mt-2">
                    Save your Client Secret now. It will never be shown again!
                  </DialogDescription>
                </DialogHeader>
                <div className="py-4 space-y-4">
                  <div className="space-y-2">
                    <Label>Client ID</Label>
                    <div className="flex items-center gap-2 p-3 bg-muted rounded-md font-mono text-sm">
                      <span className="flex-1 truncate">{newAppCreds.clientId}</span>
                      <CopyToClipboard text={newAppCreds.clientId} />
                    </div>
                  </div>
                  <div className="space-y-2">
                    <Label>Client Secret</Label>
                    <div className="flex items-center gap-2 p-3 bg-muted rounded-md font-mono text-sm">
                      <span className="flex-1 truncate">{newAppCreds.clientSecret}</span>
                      <CopyToClipboard text={newAppCreds.clientSecret} />
                    </div>
                  </div>
                </div>
                <DialogFooter>
                  <Button onClick={closeDialog} className="w-full">I've saved these securely</Button>
                </DialogFooter>
              </>
            ) : (
              <>
                <DialogHeader>
                  <DialogTitle>Register OAuth App</DialogTitle>
                  <DialogDescription>
                    Create a new OAuth 2.0 application.
                  </DialogDescription>
                </DialogHeader>
                <form onSubmit={handleCreate} className="space-y-4 py-4">
                  <div className="space-y-2">
                    <Label htmlFor="name">Application Name</Label>
                    <Input id="name" name="name" placeholder="e.g. My Internal Dashboard" required />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="callback">Callback URL</Label>
                    <Input id="callback" name="callback" placeholder="https://myapp.com/oauth/callback" required />
                  </div>
                  <DialogFooter className="pt-4">
                    <Button type="button" variant="outline" onClick={closeDialog}>Cancel</Button>
                    <Button type="submit">Register</Button>
                  </DialogFooter>
                </form>
              </>
            )}
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Registered Applications</CardTitle>
          <CardDescription>Applications you have registered to authenticate via OAuth.</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex h-48 flex-col items-center justify-center rounded-lg border border-dashed text-center">
            <ShieldCheck className="mb-4 h-8 w-8 text-muted-foreground/40" />
            <h3 className="mb-1 text-lg font-medium">No OAuth applications</h3>
            <p className="mb-4 text-sm text-muted-foreground">Register an app to get your Client ID and Secret.</p>
            <Button onClick={() => setCreateDialogOpen(true)} variant="outline">Register App</Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
