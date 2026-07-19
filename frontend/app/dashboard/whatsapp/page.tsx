"use client";

import { MessageCircle, Settings, Plus, LayoutTemplate, Send, Users, Activity } from "lucide-react";
import Link from "next/link";
import { useQuery } from "@tanstack/react-query";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { WhatsAppService } from "@/lib/whatsapp-service";
import { Badge } from "@/components/ui/badge";

export default function WhatsAppDashboard() {
  const { data: campaigns, isLoading } = useQuery({
    queryKey: ["campaigns"],
    queryFn: WhatsAppService.getCampaigns,
  });

  return (
    <div className="flex flex-col gap-6 max-w-[1200px] mx-auto">
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">WhatsApp Campaigns</h1>
          <p className="text-muted-foreground mt-1">Manage bulk messaging, templates, and WhatsApp integration.</p>
        </div>
        <div className="flex items-center gap-2">
          <Link href="/dashboard/whatsapp/settings">
            <Button variant="outline">
              <Settings className="mr-2 h-4 w-4" />
              Settings
            </Button>
          </Link>
          <Link href="/dashboard/whatsapp/campaigns/create">
            <Button>
              <Plus className="mr-2 h-4 w-4" />
              New Campaign
            </Button>
          </Link>
        </div>
      </div>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Campaigns</CardTitle>
            <Send className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{campaigns?.length || 0}</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Messages Sent</CardTitle>
            <Activity className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {campaigns?.reduce((acc, curr) => acc + curr.sentMessages, 0) || 0}
            </div>
          </CardContent>
        </Card>
        <Link href="/dashboard/whatsapp/templates" className="block">
          <Card className="cursor-pointer hover:bg-muted/50 transition-colors h-full">
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Templates</CardTitle>
              <LayoutTemplate className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-sm text-muted-foreground mt-2">Manage message templates synced from Meta.</div>
            </CardContent>
          </Card>
        </Link>
        <Link href="/dashboard/whatsapp/contacts" className="block">
          <Card className="cursor-pointer hover:bg-muted/50 transition-colors h-full">
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Contacts</CardTitle>
              <Users className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-sm text-muted-foreground mt-2">Manage contacts and lists for bulk messaging.</div>
            </CardContent>
          </Card>
        </Link>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Recent Campaigns</CardTitle>
          <CardDescription>Overview of your WhatsApp message campaigns.</CardDescription>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="py-8 text-center text-muted-foreground">Loading campaigns...</div>
          ) : campaigns?.length === 0 ? (
            <div className="flex flex-col items-center justify-center py-12 text-center border rounded-lg border-dashed bg-muted/20">
              <MessageCircle className="h-12 w-12 text-muted-foreground/40 mb-4" />
              <h3 className="text-lg font-medium">No campaigns yet</h3>
              <p className="text-sm text-muted-foreground mt-1 max-w-sm">
                Create your first WhatsApp campaign to start messaging your customers in bulk.
              </p>
              <Link href="/dashboard/whatsapp/campaigns/create">
                <Button className="mt-4">Create Campaign</Button>
              </Link>
            </div>
          ) : (
            <div className="space-y-4">
              {campaigns?.map(c => (
                <div key={c.id} className="flex items-center justify-between p-4 border rounded-lg">
                  <div>
                    <div className="flex items-center gap-2">
                      <p className="font-medium">{c.name}</p>
                      <Badge variant={c.status === "Completed" ? "default" : "secondary"}>{c.status}</Badge>
                    </div>
                    <p className="text-xs text-muted-foreground mt-1">
                      {c.totalMessages} total recipients • {new Date(c.createdAt || "").toLocaleDateString()}
                    </p>
                  </div>
                  <div className="flex gap-4 text-sm text-muted-foreground">
                    <div className="flex flex-col items-end">
                      <span className="font-medium text-foreground">{c.sentMessages}</span>
                      <span className="text-xs">Sent</span>
                    </div>
                    <div className="flex flex-col items-end">
                      <span className="font-medium text-emerald-500">{c.deliveredMessages}</span>
                      <span className="text-xs">Delivered</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
