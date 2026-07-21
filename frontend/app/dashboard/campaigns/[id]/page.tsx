"use client";

import { useCampaign } from "@/hooks/use-campaigns";
import { useParams } from "next/navigation";
import { Loader2, ArrowLeft, TrendingUp, Users, Target, Activity } from "lucide-react";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { format } from "date-fns";

export default function CampaignDetailsPage() {
  const { id } = useParams() as { id: string };
  const { data: campaign, isLoading } = useCampaign(id);

  if (isLoading) {
    return (
      <div className="flex h-[calc(100vh-4rem)] items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  if (!campaign) {
    return <div className="p-8">Campaign not found.</div>;
  }

  return (
    <div className="flex flex-col gap-8 p-8 max-w-6xl mx-auto w-full">
      <div className="flex flex-col gap-4">
        <Link href="/dashboard/campaigns">
          <Button variant="ghost" size="sm" className="-ml-4 text-muted-foreground">
            <ArrowLeft className="mr-2 h-4 w-4" />
            Back to Campaigns
          </Button>
        </Link>
        <div className="flex items-start justify-between">
          <div>
            <div className="flex items-center gap-3">
              <h1 className="text-3xl font-bold tracking-tight">{campaign.name}</h1>
              <Badge variant={campaign.status === "Running" ? "default" : "secondary"}>
                {campaign.status}
              </Badge>
            </div>
            <p className="text-muted-foreground mt-1">
              {campaign.campaignType} Campaign • {format(new Date(campaign.startDate), "MMM d, yyyy")} - {campaign.endDate ? format(new Date(campaign.endDate), "MMM d, yyyy") : "Ongoing"}
            </p>
          </div>
          <div className="flex gap-2">
            <Button variant="outline">Edit</Button>
            <Button variant="secondary">Pause</Button>
          </div>
        </div>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Budget</CardTitle>
            <Activity className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">${campaign.budget?.toLocaleString() || 0}</div>
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Leads Generated</CardTitle>
            <Users className="h-4 w-4 text-primary" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">142</div>
            <p className="text-xs text-muted-foreground mt-1">From {campaign.source || "all sources"}</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Conversion Rate</CardTitle>
            <Target className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">4.2%</div>
            <p className="text-xs text-emerald-500 mt-1 flex items-center">
              <TrendingUp className="mr-1 h-3 w-3" /> Industry avg is 2.1%
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Est. Revenue</CardTitle>
            <TrendingUp className="h-4 w-4 text-emerald-500" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">$12,450</div>
            <p className="text-xs text-muted-foreground mt-1">Based on closed won deals</p>
          </CardContent>
        </Card>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Attribution Settings</CardTitle>
            <CardDescription>UTM parameters used for this campaign</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex justify-between py-2 border-b">
              <span className="text-muted-foreground text-sm">utm_source</span>
              <span className="font-mono text-sm bg-muted px-2 py-0.5 rounded">{campaign.source || "N/A"}</span>
            </div>
            <div className="flex justify-between py-2 border-b">
              <span className="text-muted-foreground text-sm">utm_medium</span>
              <span className="font-mono text-sm bg-muted px-2 py-0.5 rounded">{campaign.medium || "N/A"}</span>
            </div>
            <div className="flex justify-between py-2 border-b">
              <span className="text-muted-foreground text-sm">utm_campaign</span>
              <span className="font-mono text-sm bg-muted px-2 py-0.5 rounded">{campaign.name.toLowerCase().replace(/\s+/g, '-')}</span>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Recent Leads</CardTitle>
            <CardDescription>Latest contacts captured via this campaign</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="flex flex-col items-center justify-center py-8 text-center">
              <Users className="h-8 w-8 text-muted-foreground/50 mb-4" />
              <p className="text-sm text-muted-foreground">Lead tracking is active. As leads submit forms using this campaign's UTM tags, they will appear here.</p>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
