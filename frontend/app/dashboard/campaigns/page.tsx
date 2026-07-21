"use client";

import { useCampaigns } from "@/hooks/use-campaigns";
import { Button } from "@/components/ui/button";
import { Plus, Target, TrendingUp, DollarSign, Users } from "lucide-react";
import Link from "next/link";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

export default function CampaignsPage() {
  const { data: campaigns, isLoading } = useCampaigns();

  if (isLoading) {
    return <div className="p-8">Loading campaigns...</div>;
  }

  // Aggregate stats
  const totalBudget = campaigns?.reduce((acc: number, c: any) => acc + (c.budget || 0), 0) || 0;
  const activeCampaigns = campaigns?.filter((c: any) => c.status === "Running").length || 0;

  return (
    <div className="flex flex-col gap-8 p-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Campaigns</h1>
          <p className="text-muted-foreground mt-1">Manage marketing initiatives and track ROI.</p>
        </div>
        <Link href="/dashboard/campaigns/create">
          <Button>
            <Plus className="mr-2 h-4 w-4" />
            Create Campaign
          </Button>
        </Link>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
        <Card className="bg-primary/5 border-primary/20">
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Spend</CardTitle>
            <DollarSign className="h-4 w-4 text-primary" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">${totalBudget.toLocaleString()}</div>
            <p className="text-xs text-muted-foreground mt-1">Across all campaigns</p>
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Active Campaigns</CardTitle>
            <Target className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{activeCampaigns}</div>
            <p className="text-xs text-muted-foreground mt-1">Currently running</p>
          </CardContent>
        </Card>
        
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Leads Generated</CardTitle>
            <Users className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">1,245</div>
            <p className="text-xs text-emerald-500 mt-1 flex items-center">
              <TrendingUp className="mr-1 h-3 w-3" /> +14% from last month
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Avg. Cost Per Lead</CardTitle>
            <TrendingUp className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">$12.50</div>
            <p className="text-xs text-emerald-500 mt-1 flex items-center">
              -5% from last month
            </p>
          </CardContent>
        </Card>
      </div>

      <div className="space-y-4">
        <h3 className="text-lg font-semibold">All Campaigns</h3>
        {campaigns && campaigns.length === 0 ? (
          <Card className="flex flex-col items-center justify-center p-12 text-center border-dashed">
            <Target className="h-8 w-8 text-muted-foreground mb-4" />
            <h3 className="text-lg font-semibold">No campaigns found</h3>
            <p className="text-muted-foreground mt-1 mb-4">Start tracking your marketing efforts today.</p>
            <Link href="/dashboard/campaigns/create">
              <Button variant="outline">Create Campaign</Button>
            </Link>
          </Card>
        ) : (
          <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
            {campaigns?.map((campaign: any) => (
              <Link key={campaign.id} href={`/dashboard/campaigns/${campaign.id}`}>
                <Card className="overflow-hidden group hover:shadow-md transition-all h-full flex flex-col cursor-pointer border-border/50 hover:border-primary/50">
                  <CardHeader className="pb-3 border-b border-border/10 bg-muted/10">
                    <div className="flex items-start justify-between">
                      <div className="space-y-1">
                        <CardTitle className="text-lg line-clamp-1 group-hover:text-primary transition-colors">
                          {campaign.name}
                        </CardTitle>
                        <CardDescription>{campaign.campaignType}</CardDescription>
                      </div>
                      <Badge variant={
                        campaign.status === "Running" ? "default" :
                        campaign.status === "Completed" ? "secondary" : "outline"
                      }>
                        {campaign.status}
                      </Badge>
                    </div>
                  </CardHeader>
                  <CardContent className="p-4 flex-1 flex flex-col justify-end">
                    <div className="grid grid-cols-2 gap-4 mt-2">
                      <div className="space-y-1">
                        <p className="text-xs text-muted-foreground uppercase font-medium">Budget</p>
                        <p className="font-semibold">${campaign.budget?.toLocaleString() || 0}</p>
                      </div>
                      <div className="space-y-1">
                        <p className="text-xs text-muted-foreground uppercase font-medium">Source</p>
                        <p className="font-medium truncate">{campaign.source || "N/A"}</p>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              </Link>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
