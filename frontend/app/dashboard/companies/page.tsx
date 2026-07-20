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
import { Plus, Building, Globe, Users } from "lucide-react";
import Link from "next/link";
import { formatCurrency } from "@/lib/utils";

export default function CompaniesPage() {
  const { data: companies, isLoading } = useQuery({
    queryKey: ['crm-companies'],
    queryFn: () => crmService.getCompanies()
  });

  return (
    <div className="p-8 space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Companies</h1>
          <p className="text-muted-foreground">Manage accounts and B2B organizations.</p>
        </div>
        <Button>
          <Plus className="mr-2 h-4 w-4" /> Add Company
        </Button>
      </div>

      <Card>
        <CardHeader className="pb-3">
          <CardTitle>All Companies</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-4">
              {[1, 2, 3, 4, 5].map(i => <Skeleton key={i} className="h-12 w-full" />)}
            </div>
          ) : companies && companies.length > 0 ? (
            <div className="rounded-md border">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Company Name</TableHead>
                    <TableHead>Industry</TableHead>
                    <TableHead>Employees</TableHead>
                    <TableHead>Annual Rev.</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {companies.map((company) => (
                    <TableRow key={company.id}>
                      <TableCell>
                        <div className="flex items-center gap-3">
                          <div className="flex h-9 w-9 items-center justify-center rounded-md bg-primary/10 text-primary">
                            <Building className="h-5 w-5" />
                          </div>
                          <div className="flex flex-col">
                            <Link href={`/dashboard/companies/${company.id}`} className="font-medium hover:underline">
                              {company.companyName}
                            </Link>
                            {company.website && (
                              <a href={`https://${company.website}`} target="_blank" rel="noreferrer" className="text-xs text-blue-500 hover:underline flex items-center gap-1">
                                <Globe className="h-3 w-3" /> {company.website}
                              </a>
                            )}
                          </div>
                        </div>
                      </TableCell>
                      <TableCell>{company.industry || '-'}</TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2">
                          <Users className="h-4 w-4 text-muted-foreground" />
                          {company.employeeCount}
                        </div>
                      </TableCell>
                      <TableCell>{formatCurrency(company.annualRevenue)}</TableCell>
                      <TableCell className="text-right">
                        <Link href={`/dashboard/companies/${company.id}`} className={buttonVariants({ variant: "ghost", size: "sm" })}>
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
              <Building className="h-10 w-10 mx-auto text-muted-foreground/50 mb-3" />
              <p>No companies found.</p>
              <Button variant="link" className="mt-2">Create your first company</Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
