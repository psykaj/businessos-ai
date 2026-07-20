"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  ArrowLeft,
  Calendar,
  DollarSign,
  TrendingUp
} from "lucide-react";
import Link from "next/link";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { use } from "react";
import { formatCurrency, formatDate } from "@/lib/utils";

export default function DealDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  
  const { data: deal, isLoading } = useQuery({
    queryKey: ['crm-deal', resolvedParams.id],
    queryFn: () => crmService.getDeal(resolvedParams.id)
  });

  if (isLoading) {
    return (
      <div className="p-8 space-y-6">
        <Skeleton className="h-8 w-40" />
        <Card>
          <CardContent className="p-6">
            <div className="flex gap-6 items-center">
              <div className="space-y-3 flex-1">
                <Skeleton className="h-8 w-1/3" />
                <Skeleton className="h-4 w-1/4" />
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  if (!deal) {
    return <div className="p-8">Deal not found.</div>;
  }

  return (
    <div className="p-8 space-y-6">
      <Link href="/dashboard/deals" className={buttonVariants({ variant: "ghost", size: "sm" }) + " -ml-3 mb-2 text-muted-foreground"}>
        <ArrowLeft className="mr-2 h-4 w-4" /> Back to Pipeline
      </Link>

      {/* Header */}
      <Card className="border-t-4 border-t-primary">
        <CardContent className="p-6">
          <div className="flex flex-col md:flex-row gap-6 items-start md:items-center justify-between">
            <div className="flex-1 space-y-1.5">
              <h1 className="text-3xl font-bold">{deal.title}</h1>
              <p className="text-muted-foreground">{deal.description || "No description provided."}</p>
            </div>
            <div className="flex flex-col gap-2 items-end">
              <div className="text-2xl font-bold text-green-600 bg-green-50 px-4 py-1 rounded-lg">
                {formatCurrency(deal.amount)}
              </div>
              <div className="text-sm font-medium text-muted-foreground">
                Probability: {deal.probability}%
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Details & Activity Tabs */}
      <Tabs defaultValue="overview" className="w-full">
        <TabsList className="mb-4">
          <TabsTrigger value="overview">Overview</TabsTrigger>
          <TabsTrigger value="activities">Activities</TabsTrigger>
          <TabsTrigger value="notes">Notes</TabsTrigger>
        </TabsList>
        
        <TabsContent value="overview">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <Card className="md:col-span-2">
              <CardHeader>
                <CardTitle>Deal Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-2 gap-6">
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Expected Close Date</p>
                    <p className="text-sm flex items-center gap-2 font-medium">
                      <Calendar className="h-4 w-4 text-primary" />
                      {deal.expectedCloseDate ? formatDate(deal.expectedCloseDate) : "Not set"}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Pipeline Stage</p>
                    <p className="text-sm flex items-center gap-2 font-medium">
                      <TrendingUp className="h-4 w-4 text-primary" />
                      Stage {deal.pipelineStage}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Created</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(deal.createdAt)}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Last Updated</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(deal.updatedAt)}
                    </p>
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Actions</CardTitle>
              </CardHeader>
              <CardContent className="space-y-2">
                <Button variant="outline" className="w-full justify-start">
                  Edit Deal
                </Button>
                <Button variant="outline" className="w-full justify-start">
                  Log Activity
                </Button>
              </CardContent>
            </Card>
          </div>
        </TabsContent>
        
        <TabsContent value="activities">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              Activities feed will be populated here.
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="notes">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              Notes feature coming soon.
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
