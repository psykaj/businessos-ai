"use client";

import { useFormSubmissions } from "@/hooks/use-forms";
import { useSearchParams } from "next/navigation";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ExternalLink, Search, Download } from "lucide-react";
import Link from "next/link";
import { format } from "date-fns";
import { useState } from "react";

export default function SubmissionsDashboard() {
  const searchParams = useSearchParams();
  const formId = searchParams?.get("formId") || undefined;
  
  const { data: submissions, isLoading } = useFormSubmissions(formId);
  const [searchTerm, setSearchTerm] = useState("");

  const filteredSubmissions = submissions?.filter((sub: any) => {
    try {
      const payload = JSON.parse(sub.payload);
      const searchString = Object.values(payload).join(" ").toLowerCase();
      return searchString.includes(searchTerm.toLowerCase());
    } catch {
      return true;
    }
  });

  return (
    <div className="flex flex-col gap-6 p-8 h-full">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Form Submissions</h1>
          <p className="text-muted-foreground mt-1">Review captured leads and form data.</p>
        </div>
        <Button variant="outline">
          <Download className="mr-2 h-4 w-4" />
          Export CSV
        </Button>
      </div>

      <div className="flex items-center gap-2">
        <div className="relative flex-1 max-w-sm">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input 
            placeholder="Search submissions..." 
            className="pl-9"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
      </div>

      <div className="rounded-xl border bg-card">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Date</TableHead>
              <TableHead>Name / Email</TableHead>
              <TableHead>Source</TableHead>
              <TableHead className="text-right">Action</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {isLoading ? (
              <TableRow>
                <TableCell colSpan={4} className="h-24 text-center">Loading submissions...</TableCell>
              </TableRow>
            ) : filteredSubmissions?.length === 0 ? (
              <TableRow>
                <TableCell colSpan={4} className="h-24 text-center text-muted-foreground">
                  No submissions found.
                </TableCell>
              </TableRow>
            ) : (
              filteredSubmissions?.map((sub: any) => {
                let payload: Record<string, string> = {};
                try {
                  payload = JSON.parse(sub.payload);
                } catch {}

                return (
                  <TableRow key={sub.id}>
                    <TableCell className="whitespace-nowrap">
                      {format(new Date(sub.submittedAt), "MMM d, yyyy h:mm a")}
                    </TableCell>
                    <TableCell>
                      <div className="font-medium">{payload.firstName || payload.name || "Unknown"} {payload.lastName || ""}</div>
                      <div className="text-sm text-muted-foreground">{payload.email || "No email"}</div>
                    </TableCell>
                    <TableCell>
                      <div className="text-sm">{sub.source || "Direct"}</div>
                    </TableCell>
                    <TableCell className="text-right">
                      {sub.leadId && (
                        <Link href={`/dashboard/crm/leads/${sub.leadId}`}>
                          <Button variant="ghost" size="sm" className="h-8">
                            View CRM Lead
                            <ExternalLink className="ml-2 h-3 w-3" />
                          </Button>
                        </Link>
                      )}
                    </TableCell>
                  </TableRow>
                );
              })
            )}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
