"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { Plus, LayoutTemplate, Settings, Trash2, Edit, ExternalLink, Globe } from "lucide-react";
import { toast } from "sonner";
import { format } from "date-fns";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { StatusBadge } from "@/components/ui/status-badge";
import { Skeleton } from "@/components/ui/skeleton";

import { LandingPagesService } from "@/lib/landing-pages-service";
import { DomainsService } from "@/lib/domains-service";

export default function LandingPagesList() {
  const queryClient = useQueryClient();
  const router = useRouter();

  const { data: pages, isLoading } = useQuery({
    queryKey: ["landing-pages"],
    queryFn: LandingPagesService.getPages,
  });

  const { data: domains } = useQuery({
    queryKey: ["domains"],
    queryFn: DomainsService.getDomains,
  });

  const primaryDomain = domains?.find((d) => d.isPrimary)?.domain || "yourdomain.com";

  const deleteMutation = useMutation({
    mutationFn: (id: string) => LandingPagesService.deletePage(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["landing-pages"] });
      toast.success("Page deleted successfully");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to delete page");
    },
  });

  return (
    <div className="flex flex-col gap-6 max-w-[1200px] mx-auto">
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Landing Pages</h1>
          <p className="text-muted-foreground mt-1">Build and manage your custom landing pages.</p>
        </div>
        <Link href="/dashboard/landing-pages/create" className={buttonVariants()}>
          <Plus className="mr-2 h-4 w-4" />
          Create Page
        </Link>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <LayoutTemplate className="h-5 w-5 text-primary" />
            Your Pages
          </CardTitle>
          <CardDescription>Manage your landing pages and track their status.</CardDescription>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-2">
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-12 w-full" />
              <Skeleton className="h-12 w-full" />
            </div>
          ) : pages && pages.length > 0 ? (
            <div className="rounded-md border overflow-x-auto">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Title</TableHead>
                    <TableHead>URL Path</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Updated</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {pages.map((page) => (
                    <TableRow key={page.id}>
                      <TableCell className="font-medium">{page.title}</TableCell>
                      <TableCell>
                        <div className="flex items-center text-sm text-muted-foreground">
                          <Globe className="h-3 w-3 mr-1" />
                          {primaryDomain}/{page.slug}
                        </div>
                      </TableCell>
                      <TableCell>
                        {page.status === "Published" ? (
                          <StatusBadge status="Published" />
                        ) : (
                          <StatusBadge status="Draft" />
                        )}
                      </TableCell>
                      <TableCell className="text-muted-foreground">
                        {format(new Date(page.updatedAt), "MMM d, yyyy")}
                      </TableCell>
                      <TableCell className="text-right">
                        <div className="flex justify-end items-center gap-2">
                          {page.status === "Published" && (
                            <a href={`https://${primaryDomain}/${page.slug}`} target="_blank" rel="noopener noreferrer" className={buttonVariants({ variant: "ghost", size: "sm" })}>
                              <ExternalLink className="h-4 w-4 mr-2" /> View
                            </a>
                          )}
                          <Link href={`/dashboard/landing-pages/edit/${page.id}`} className={buttonVariants({ variant: "outline", size: "sm" })}>
                            <Edit className="h-4 w-4 mr-2" /> Edit
                          </Link>
                          <Button 
                            variant="ghost" 
                            size="icon" 
                            className="text-destructive hover:bg-destructive/10"
                            onClick={() => {
                              if (window.confirm("Are you sure you want to delete this page?")) {
                                deleteMutation.mutate(page.id);
                              }
                            }}
                            disabled={deleteMutation.isPending}
                          >
                            <Trash2 className="h-4 w-4" />
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          ) : (
            <div className="text-center py-12 border-2 border-dashed rounded-lg border-muted">
              <LayoutTemplate className="h-12 w-12 mx-auto text-muted-foreground/50 mb-4" />
              <h3 className="text-lg font-medium">No pages created</h3>
              <p className="text-sm text-muted-foreground mt-1 mb-4">
                Build your first landing page using the modular page builder.
              </p>
              <Link href="/dashboard/landing-pages/create" className={buttonVariants()}>
                <Plus className="mr-2 h-4 w-4" /> Create your first page
              </Link>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
