"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  ArrowLeft,
  Building,
  Globe,
  MapPin,
  Calendar,
  Users,
  DollarSign
} from "lucide-react";
import Link from "next/link";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { use } from "react";
import { formatCurrency, formatDate } from "@/lib/utils";

export default function CompanyDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  
  const { data: company, isLoading } = useQuery({
    queryKey: ['crm-company', resolvedParams.id],
    queryFn: () => crmService.getCompany(resolvedParams.id)
  });

  if (isLoading) {
    return (
      <div className="p-8 space-y-6">
        <Skeleton className="h-8 w-40" />
        <Card>
          <CardContent className="p-6">
            <div className="flex gap-6 items-center">
              <Skeleton className="h-20 w-20 rounded-md" />
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

  if (!company) {
    return <div className="p-8">Company not found.</div>;
  }

  return (
    <div className="p-8 space-y-6">
      <Link href="/dashboard/companies" className={buttonVariants({ variant: "ghost", size: "sm" }) + " -ml-3 mb-2 text-muted-foreground"}>
        <ArrowLeft className="mr-2 h-4 w-4" /> Back to Companies
      </Link>

      {/* Profile Header */}
      <Card className="border-t-4 border-t-primary">
        <CardContent className="p-6">
          <div className="flex flex-col md:flex-row gap-6 items-start md:items-center">
            <div className="flex h-20 w-20 items-center justify-center rounded-xl bg-primary/10 text-primary border border-primary/20 shadow-sm">
              <Building className="h-10 w-10" />
            </div>
            <div className="flex-1 space-y-1.5">
              <h1 className="text-3xl font-bold">{company.companyName}</h1>
              <div className="flex flex-wrap items-center gap-4 text-sm text-muted-foreground">
                {company.industry && (
                  <span className="flex items-center gap-1.5 font-medium text-foreground">
                    <Building className="h-4 w-4" />
                    {company.industry}
                  </span>
                )}
                {company.website && (
                  <a href={`https://${company.website}`} target="_blank" rel="noreferrer" className="flex items-center gap-1.5 text-blue-500 hover:underline">
                    <Globe className="h-4 w-4" />
                    {company.website}
                  </a>
                )}
                {company.address && (
                  <span className="flex items-center gap-1.5">
                    <MapPin className="h-4 w-4" />
                    {company.address}
                  </span>
                )}
              </div>
            </div>
            <div className="flex gap-2">
              <Button variant="outline">Edit</Button>
              <Button>Log Activity</Button>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Details & Activity Tabs */}
      <Tabs defaultValue="overview" className="w-full">
        <TabsList className="mb-4">
          <TabsTrigger value="overview">Overview</TabsTrigger>
          <TabsTrigger value="contacts">Contacts</TabsTrigger>
          <TabsTrigger value="deals">Deals</TabsTrigger>
          <TabsTrigger value="activities">Activities</TabsTrigger>
        </TabsList>
        
        <TabsContent value="overview">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <Card className="md:col-span-2">
              <CardHeader>
                <CardTitle>Company Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-2 gap-6">
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Annual Revenue</p>
                    <p className="text-lg font-semibold flex items-center gap-2">
                      <DollarSign className="h-5 w-5 text-green-600" />
                      {formatCurrency(company.annualRevenue)}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Employees</p>
                    <p className="text-lg font-semibold flex items-center gap-2">
                      <Users className="h-5 w-5 text-blue-600" />
                      {company.employeeCount}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Created</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(company.createdAt)}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Last Updated</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(company.updatedAt)}
                    </p>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>
        
        <TabsContent value="contacts">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              Employees linked to this company will appear here.
            </CardContent>
          </Card>
        </TabsContent>
        
        <TabsContent value="deals">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              Associated deals will appear here.
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="activities">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              Activities feed will be populated here.
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
