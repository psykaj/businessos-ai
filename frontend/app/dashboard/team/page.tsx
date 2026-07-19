"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "@/contexts/auth-context";
import { useTeamMembers, useRemoveMember, useDeactivateMember, useReactivateMember } from "@/hooks/use-team";
import { TeamMember } from "@/types/team";
import { toast } from "sonner";
import { format } from "date-fns";

import { Button } from "@/components/ui/button";
import { DataTable } from "@/components/ui/data-table";
import { StatusBadge } from "@/components/ui/status-badge";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { MoreHorizontal, Plus, ShieldAlert, Trash2, UserX, UserCheck } from "lucide-react";

export default function TeamPage() {
  const router = useRouter();
  const { user } = useAuth();
  const orgId = user?.organizationId;
  const { data: teamMembers, isLoading } = useTeamMembers(orgId);
  const removeMember = useRemoveMember();
  const deactivateMember = useDeactivateMember();
  const reactivateMember = useReactivateMember();

  const handleRemove = async (memberId: string) => {
    if (!orgId) return;
    if (confirm("Are you sure you want to remove this member from the organization?")) {
      try {
        await removeMember.mutateAsync({ orgId, memberId });
        toast.success("Member removed successfully");
      } catch (e) {
        toast.error("Failed to remove member");
      }
    }
  };

  const handleDeactivate = async (memberId: string) => {
    if (!orgId) return;
    try {
      await deactivateMember.mutateAsync({ orgId, memberId });
      toast.success("Member deactivated");
    } catch (e) {
      toast.error("Failed to deactivate member");
    }
  };

  const handleReactivate = async (memberId: string) => {
    if (!orgId) return;
    try {
      await reactivateMember.mutateAsync({ orgId, memberId });
      toast.success("Member reactivated");
    } catch (e) {
      toast.error("Failed to reactivate member");
    }
  };

  const columns = [
    {
      header: "Member",
      cell: (item: TeamMember) => (
        <div className="flex items-center gap-3">
          <div className="flex h-9 w-9 items-center justify-center rounded-full bg-primary/10 text-primary font-semibold">
            {item.fullName.charAt(0).toUpperCase()}
          </div>
          <div>
            <p className="font-medium">{item.fullName}</p>
            <p className="text-xs text-muted-foreground">{item.email}</p>
          </div>
        </div>
      )
    },
    {
      header: "Role",
      cell: (item: TeamMember) => <span className="font-medium">{item.role}</span>
    },
    {
      header: "Status",
      cell: (item: TeamMember) => <StatusBadge status={item.isActive} />
    },
    {
      header: "Joined",
      cell: (item: TeamMember) => (
        <span className="text-sm text-muted-foreground">
          {format(new Date(item.joinedAt), "MMM d, yyyy")}
        </span>
      )
    },
    {
      header: "",
      cell: (item: TeamMember) => (
        <DropdownMenu>
          <DropdownMenuTrigger render={<Button variant="ghost" className="h-8 w-8 p-0" />}>
            <span className="sr-only">Open menu</span>
            <MoreHorizontal className="h-4 w-4" />
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuItem onClick={() => alert("Edit role modal coming soon")}>
              <ShieldAlert className="mr-2 h-4 w-4" />
              Edit Role
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            {item.isActive ? (
              <DropdownMenuItem onClick={() => handleDeactivate(item.id)}>
                <UserX className="mr-2 h-4 w-4" />
                Deactivate
              </DropdownMenuItem>
            ) : (
              <DropdownMenuItem onClick={() => handleReactivate(item.id)}>
                <UserCheck className="mr-2 h-4 w-4" />
                Reactivate
              </DropdownMenuItem>
            )}
            <DropdownMenuItem onClick={() => handleRemove(item.id)} className="text-destructive">
              <Trash2 className="mr-2 h-4 w-4" />
              Remove Member
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
          <h1 className="text-2xl font-bold tracking-tight text-foreground">Team Management</h1>
          <p className="mt-1 text-sm text-muted-foreground">Manage your team members and their access roles.</p>
        </div>
        <Button onClick={() => router.push("/dashboard/team/invite")}>
          <Plus className="mr-2 h-4 w-4" />
          Invite Member
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Active Team</CardTitle>
          <CardDescription>People with access to this organization.</CardDescription>
        </CardHeader>
        <CardContent>
          <DataTable
            data={teamMembers || []}
            columns={columns}
            isLoading={isLoading}
          />
        </CardContent>
      </Card>
    </div>
  );
}
