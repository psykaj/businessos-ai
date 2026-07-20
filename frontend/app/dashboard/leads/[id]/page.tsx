"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  ArrowLeft,
  Mail, 
  Phone, 
  Building,
  Calendar,
  Briefcase,
  DollarSign
} from "lucide-react";
import Link from "next/link";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { use } from "react";
import { formatCurrency, formatDate } from "@/lib/utils";
import { LeadStatus } from "@/types/crm";
import { Badge } from "@/components/ui/badge";

const getStatusBadge = (status: LeadStatus) => {
  switch (status) {
    case LeadStatus.New:
      return <Badge variant="outline" className="bg-blue-50 text-blue-700 border-blue-200">New</Badge>;
    case LeadStatus.Contacted:
      return <Badge variant="outline" className="bg-yellow-50 text-yellow-700 border-yellow-200">Contacted</Badge>;
    case LeadStatus.Qualified:
      return <Badge variant="outline" className="bg-green-50 text-green-700 border-green-200">Qualified</Badge>;
    case LeadStatus.Unqualified:
      return <Badge variant="outline" className="bg-red-50 text-red-700 border-red-200">Unqualified</Badge>;
    case LeadStatus.Customer:
      return <Badge variant="outline" className="bg-purple-50 text-purple-700 border-purple-200">Customer</Badge>;
    default:
      return <Badge variant="outline">Unknown</Badge>;
  }
};

export default function LeadDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  
  const { data: lead, isLoading } = useQuery({
    queryKey: ['crm-lead', resolvedParams.id],
    queryFn: () => crmService.getLead(resolvedParams.id)
  });

  if (isLoading) {
    return (
      <div className="p-8 space-y-6">
        <Skeleton className="h-8 w-40" />
        <Card>
          <CardContent className="p-6">
            <div className="flex gap-6">
              <Skeleton className="h-24 w-24 rounded-full" />
              <div className="space-y-3 flex-1">
                <Skeleton className="h-8 w-1/3" />
                <Skeleton className="h-4 w-1/4" />
                <Skeleton className="h-4 w-1/2" />
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  if (!lead) {
    return <div className="p-8">Lead not found.</div>;
  }

  return (
    <div className="p-8 space-y-6">
      <Link href="/dashboard/leads" className={buttonVariants({ variant: "ghost", size: "sm" }) + " -ml-3 mb-2 text-muted-foreground"}>
        <ArrowLeft className="mr-2 h-4 w-4" /> Back to Leads
      </Link>

      {/* Profile Header */}
      <Card className="border-t-4 border-t-primary">
        <CardContent className="p-6">
          <div className="flex flex-col md:flex-row gap-6 items-start md:items-center">
            <Avatar className="h-24 w-24 border-4 border-background shadow-sm">
              <AvatarFallback className="bg-primary/10 text-primary text-3xl font-bold">
                {lead.firstName.charAt(0)}{lead.lastName.charAt(0)}
              </AvatarFallback>
            </Avatar>
            <div className="flex-1 space-y-1.5">
              <div className="flex items-center gap-3">
                <h1 className="text-3xl font-bold">{lead.firstName} {lead.lastName}</h1>
                {getStatusBadge(lead.status)}
              </div>
              <div className="flex flex-wrap items-center gap-4 text-sm text-muted-foreground mt-2">
                {lead.jobTitle && (
                  <span className="flex items-center gap-1.5 font-medium text-foreground">
                    <Briefcase className="h-4 w-4" />
                    {lead.jobTitle}
                  </span>
                )}
                <span className="flex items-center gap-1.5">
                  <Building className="h-4 w-4" />
                  {lead.companyName}
                </span>
                <span className="flex items-center gap-1.5">
                  <Mail className="h-4 w-4" />
                  {lead.email}
                </span>
                {lead.phoneNumber && (
                  <span className="flex items-center gap-1.5">
                    <Phone className="h-4 w-4" />
                    {lead.phoneNumber}
                  </span>
                )}
              </div>
            </div>
            <div className="flex gap-2">
              <Button variant="outline">Edit</Button>
              <Button>Convert to Deal</Button>
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
                <CardTitle>Lead Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Estimated Value</p>
                    <p className="text-lg font-semibold flex items-center gap-2">
                      <DollarSign className="h-4 w-4 text-green-600" />
                      {formatCurrency(lead.estimatedValue)}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Lead Source</p>
                    <p className="text-sm font-medium">{lead.source || 'Unknown'}</p>
                  </div>
                  <div className="space-y-1 mt-4">
                    <p className="text-sm font-medium text-muted-foreground">Created</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(lead.createdAt)}
                    </p>
                  </div>
                  <div className="space-y-1 mt-4">
                    <p className="text-sm font-medium text-muted-foreground">Last Updated</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(lead.updatedAt)}
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
                  <Mail className="mr-2 h-4 w-4" /> Send Email
                </Button>
                <Button variant="outline" className="w-full justify-start">
                  <Phone className="mr-2 h-4 w-4" /> Log Call
                </Button>
                <Button variant="outline" className="w-full justify-start">
                  <Calendar className="mr-2 h-4 w-4" /> Schedule Meeting
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
