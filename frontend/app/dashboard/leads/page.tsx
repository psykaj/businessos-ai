"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableHead, 
  TableHeader, 
  TableRow 
} from "@/components/ui/table";
import { Plus, Target, Mail, Phone, TrendingUp } from "lucide-react";
import Link from "next/link";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { formatCurrency } from "@/lib/utils";
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

export default function LeadsPage() {
  const { data: leads, isLoading } = useQuery({
    queryKey: ['crm-leads'],
    queryFn: () => crmService.getLeads()
  });

  return (
    <div className="p-8 space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Leads</h1>
          <p className="text-muted-foreground">Track and qualify potential opportunities.</p>
        </div>
        <Button>
          <Plus className="mr-2 h-4 w-4" /> Add Lead
        </Button>
      </div>

      <Card>
        <CardHeader className="pb-3">
          <CardTitle>All Leads</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-4">
              {[1, 2, 3, 4, 5].map(i => <Skeleton key={i} className="h-12 w-full" />)}
            </div>
          ) : leads && leads.length > 0 ? (
            <div className="rounded-md border">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Lead</TableHead>
                    <TableHead>Company</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Est. Value</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {leads.map((lead) => (
                    <TableRow key={lead.id}>
                      <TableCell>
                        <div className="flex items-center gap-3">
                          <Avatar className="h-9 w-9">
                            <AvatarFallback className="bg-primary/10 text-primary font-medium">
                              {lead.firstName.charAt(0)}{lead.lastName.charAt(0)}
                            </AvatarFallback>
                          </Avatar>
                          <div className="flex flex-col">
                            <Link href={`/dashboard/leads/${lead.id}`} className="font-medium hover:underline">
                              {lead.firstName} {lead.lastName}
                            </Link>
                            <span className="text-xs text-muted-foreground">{lead.email}</span>
                          </div>
                        </div>
                      </TableCell>
                      <TableCell>
                        <span className="font-medium">{lead.companyName}</span>
                      </TableCell>
                      <TableCell>
                        {getStatusBadge(lead.status)}
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2 font-medium">
                          {formatCurrency(lead.estimatedValue)}
                        </div>
                      </TableCell>
                      <TableCell className="text-right">
                        <Link href={`/dashboard/leads/${lead.id}`} className={buttonVariants({ variant: "ghost", size: "sm" })}>
                          View
                        </Link>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          ) : (
            <div className="text-center py-10 text-muted-foreground">
              <Target className="h-10 w-10 mx-auto text-muted-foreground/50 mb-3" />
              <p>No leads found.</p>
              <Button variant="link" className="mt-2">Create your first lead</Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
