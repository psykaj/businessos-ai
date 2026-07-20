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
  MapPin,
  Calendar,
  Briefcase
} from "lucide-react";
import Link from "next/link";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { use } from "react";
import { formatDate } from "@/lib/utils";

export default function ContactDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  
  const { data: contact, isLoading } = useQuery({
    queryKey: ['crm-contact', resolvedParams.id],
    queryFn: () => crmService.getContact(resolvedParams.id)
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

  if (!contact) {
    return <div className="p-8">Contact not found.</div>;
  }

  return (
    <div className="p-8 space-y-6">
      <Link href="/dashboard/contacts" className={buttonVariants({ variant: "ghost", size: "sm" }) + " -ml-3 mb-2 text-muted-foreground"}>
        <ArrowLeft className="mr-2 h-4 w-4" /> Back to Contacts
      </Link>

      {/* Profile Header */}
      <Card className="border-t-4 border-t-primary">
        <CardContent className="p-6">
          <div className="flex flex-col md:flex-row gap-6 items-start md:items-center">
            <Avatar className="h-24 w-24 border-4 border-background shadow-sm">
              <AvatarFallback className="bg-primary/10 text-primary text-3xl font-bold">
                {contact.firstName.charAt(0)}{contact.lastName.charAt(0)}
              </AvatarFallback>
            </Avatar>
            <div className="flex-1 space-y-1.5">
              <h1 className="text-3xl font-bold">{contact.firstName} {contact.lastName}</h1>
              <div className="flex flex-wrap items-center gap-4 text-sm text-muted-foreground">
                {contact.jobTitle && (
                  <span className="flex items-center gap-1.5 font-medium text-foreground">
                    <Briefcase className="h-4 w-4" />
                    {contact.jobTitle}
                  </span>
                )}
                {contact.companyName && (
                  <span className="flex items-center gap-1.5">
                    <Building className="h-4 w-4" />
                    {contact.companyName}
                  </span>
                )}
                <span className="flex items-center gap-1.5">
                  <Mail className="h-4 w-4" />
                  {contact.email}
                </span>
                {contact.phoneNumber && (
                  <span className="flex items-center gap-1.5">
                    <Phone className="h-4 w-4" />
                    {contact.phoneNumber}
                  </span>
                )}
                {contact.address && (
                  <span className="flex items-center gap-1.5">
                    <MapPin className="h-4 w-4" />
                    {contact.address}
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
          <TabsTrigger value="activities">Activities</TabsTrigger>
          <TabsTrigger value="deals">Deals</TabsTrigger>
          <TabsTrigger value="notes">Notes</TabsTrigger>
        </TabsList>
        
        <TabsContent value="overview">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <Card className="md:col-span-2">
              <CardHeader>
                <CardTitle>About</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Created</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(contact.createdAt)}
                    </p>
                  </div>
                  <div className="space-y-1">
                    <p className="text-sm font-medium text-muted-foreground">Last Updated</p>
                    <p className="text-sm flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      {formatDate(contact.updatedAt)}
                    </p>
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Tags</CardTitle>
              </CardHeader>
              <CardContent>
                {contact.tags ? (
                  <div className="flex flex-wrap gap-2">
                    {contact.tags.split(',').map((tag, i) => (
                      <span key={i} className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-primary/10 text-primary">
                        {tag.trim()}
                      </span>
                    ))}
                  </div>
                ) : (
                  <p className="text-sm text-muted-foreground">No tags assigned.</p>
                )}
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
        
        <TabsContent value="deals">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              Associated deals will appear here.
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
