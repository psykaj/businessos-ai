"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { useAuth } from "@/contexts/auth-context";
import { useInviteMember, useInvitations, useCancelInvitation } from "@/hooks/use-team";
import { Invitation } from "@/types/team";
import { format } from "date-fns";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { DataTable } from "@/components/ui/data-table";
import { StatusBadge } from "@/components/ui/status-badge";
import { Loader2, ArrowLeft, MailX, Send } from "lucide-react";

const inviteSchema = z.object({
  email: z.string().email("Please enter a valid email address."),
  role: z.string().min(1, "Please select a role."),
});

type InviteFormValues = z.infer<typeof inviteSchema>;

export default function InvitePage() {
  const router = useRouter();
  const { user } = useAuth();
  const orgId = user?.organizationId;
  const { data: invitations, isLoading: isInvitesLoading } = useInvitations(orgId);
  const inviteMember = useInviteMember();
  const cancelInvitation = useCancelInvitation();

  const form = useForm<InviteFormValues>({
    resolver: zodResolver(inviteSchema),
    defaultValues: {
      email: "",
      role: "Member",
    },
  });

  const onSubmit = async (data: InviteFormValues) => {
    if (!orgId) return;
    try {
      await inviteMember.mutateAsync({ orgId, data });
      toast.success("Invitation sent successfully.");
      form.reset();
    } catch (error) {
      toast.error("Failed to send invitation.");
    }
  };

  const handleCancel = async (id: string) => {
    if (!orgId) return;
    try {
      await cancelInvitation.mutateAsync({ orgId, id });
      toast.success("Invitation cancelled.");
    } catch (e) {
      toast.error("Failed to cancel invitation.");
    }
  };

  const pendingInvites = invitations?.filter(i => i.status === "Pending") || [];

  const columns = [
    {
      header: "Email",
      accessorKey: "email" as keyof Invitation
    },
    {
      header: "Role",
      accessorKey: "role" as keyof Invitation
    },
    {
      header: "Status",
      cell: (item: Invitation) => <StatusBadge status={item.status} />
    },
    {
      header: "Sent At",
      cell: (item: Invitation) => (
        <span className="text-sm text-muted-foreground">
          {format(new Date(item.createdAt), "MMM d, yyyy")}
        </span>
      )
    },
    {
      header: "",
      cell: (item: Invitation) => (
        <Button variant="ghost" size="sm" onClick={() => handleCancel(item.id)} className="text-destructive hover:text-destructive/90 hover:bg-destructive/10">
          <MailX className="mr-2 h-4 w-4" />
          Cancel
        </Button>
      )
    }
  ];

  return (
    <div className="mx-auto max-w-4xl space-y-6">
      <div className="flex items-center gap-4">
        <Button variant="outline" size="icon" onClick={() => router.back()}>
          <ArrowLeft className="h-4 w-4" />
        </Button>
        <div>
          <h1 className="text-2xl font-bold tracking-tight text-foreground">Invite to Team</h1>
          <p className="mt-1 text-sm text-muted-foreground">Send an email invitation to add someone to this organization.</p>
        </div>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Send Invitation</CardTitle>
            <CardDescription>They will receive a secure token to join.</CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="email">Email Address</Label>
                <Input id="email" type="email" placeholder="colleague@company.com" {...form.register("email")} />
                {form.formState.errors.email && (
                  <p className="text-sm text-destructive">{form.formState.errors.email.message}</p>
                )}
              </div>
              
              <div className="space-y-2">
                <Label htmlFor="role">Assign Role</Label>
                <Select onValueChange={(val) => form.setValue("role", val as string)} defaultValue={form.getValues("role")}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select a role" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Admin">Admin</SelectItem>
                    <SelectItem value="Manager">Manager</SelectItem>
                    <SelectItem value="Member">Member</SelectItem>
                    <SelectItem value="Viewer">Viewer</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="pt-4">
                <Button type="submit" className="w-full" disabled={inviteMember.isPending}>
                  {inviteMember.isPending ? (
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  ) : (
                    <Send className="mr-2 h-4 w-4" />
                  )}
                  Send Invitation
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Pending Invitations</CardTitle>
            <CardDescription>Invitations awaiting acceptance.</CardDescription>
          </CardHeader>
          <CardContent>
            {isInvitesLoading ? (
              <div className="flex h-32 items-center justify-center">
                <Loader2 className="h-6 w-6 animate-spin text-muted-foreground" />
              </div>
            ) : pendingInvites.length === 0 ? (
              <div className="flex h-32 flex-col items-center justify-center rounded-lg border border-dashed text-center">
                <p className="text-sm text-muted-foreground">No pending invitations.</p>
              </div>
            ) : (
              <div className="rounded-md border">
                <DataTable data={pendingInvites} columns={columns} />
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
