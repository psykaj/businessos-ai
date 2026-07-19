"use client";

import Link from "next/link";
import { Mail, LayoutTemplate, Plus, Send, Inbox, FileText, Settings } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

export default function EmailDashboardPage() {
  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Email Center</h1>
          <p className="text-muted-foreground">Manage your campaigns, templates, and communications.</p>
        </div>
        <div className="flex gap-2">
          <Link href="/dashboard/email/templates">
            <Button variant="outline" className="gap-2">
              <LayoutTemplate className="h-4 w-4" />
              Templates
            </Button>
          </Link>
          <Button className="gap-2">
            <Plus className="h-4 w-4" />
            Compose
          </Button>
        </div>
      </div>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Sent Mails</CardTitle>
            <Send className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">1,234</div>
            <p className="text-xs text-muted-foreground">+15% from last month</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Open Rate</CardTitle>
            <Mail className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">42.3%</div>
            <p className="text-xs text-muted-foreground">+2.1% from last month</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Drafts</CardTitle>
            <FileText className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">5</div>
            <p className="text-xs text-muted-foreground">Requires attention</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Templates</CardTitle>
            <LayoutTemplate className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">12</div>
            <p className="text-xs text-muted-foreground">3 active templates</p>
          </CardContent>
        </Card>
      </div>

      <Tabs defaultValue="inbox" className="w-full">
        <TabsList>
          <TabsTrigger value="inbox" className="gap-2"><Inbox className="h-4 w-4"/> Inbox</TabsTrigger>
          <TabsTrigger value="sent" className="gap-2"><Send className="h-4 w-4"/> Sent</TabsTrigger>
          <TabsTrigger value="drafts" className="gap-2"><FileText className="h-4 w-4"/> Drafts</TabsTrigger>
        </TabsList>
        <TabsContent value="inbox" className="mt-6">
          <Card>
            <CardContent className="p-12 text-center text-muted-foreground">
              <Mail className="h-10 w-10 mx-auto mb-4 text-muted-foreground/30" />
              <p>Your inbox is empty.</p>
            </CardContent>
          </Card>
        </TabsContent>
        <TabsContent value="sent" className="mt-6">
          <Card>
            <CardContent className="p-12 text-center text-muted-foreground">
              No sent emails yet.
            </CardContent>
          </Card>
        </TabsContent>
        <TabsContent value="drafts" className="mt-6">
          <Card>
            <CardContent className="p-12 text-center text-muted-foreground">
              No drafts available.
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
