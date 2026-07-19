"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { Globe, Plus, ShieldCheck, ShieldAlert, Star, Trash2, Settings, ExternalLink, RefreshCw } from "lucide-react";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { StatusBadge } from "@/components/ui/status-badge";
import { Skeleton } from "@/components/ui/skeleton";
import { CopyToClipboard } from "@/components/ui/copy-to-clipboard";

import { DomainsService, CustomDomainDto, CreateCustomDomainDto } from "@/lib/domains-service";

const domainSchema = z.object({
  domain: z.string().regex(/^(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9]$/i, "Please enter a valid domain name (e.g., example.com)"),
});

export default function DomainsPage() {
  const queryClient = useQueryClient();
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [selectedDomain, setSelectedDomain] = useState<string | null>(null);

  const { data: domains, isLoading } = useQuery({
    queryKey: ["domains"],
    queryFn: DomainsService.getDomains,
  });

  const { data: dnsInstructions, isFetching: isFetchingDns } = useQuery({
    queryKey: ["dns", selectedDomain],
    queryFn: () => DomainsService.getDnsInstructions(selectedDomain!),
    enabled: !!selectedDomain,
  });

  const addMutation = useMutation({
    mutationFn: (data: CreateCustomDomainDto) => DomainsService.addDomain(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["domains"] });
      toast.success("Domain added successfully");
      setIsAddOpen(false);
      form.reset();
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to add domain");
    },
  });

  const verifyMutation = useMutation({
    mutationFn: (id: string) => DomainsService.verifyDomain(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["domains"] });
      toast.success("Domain verification successful");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Verification failed");
    },
  });

  const primaryMutation = useMutation({
    mutationFn: (id: string) => DomainsService.setPrimary(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["domains"] });
      toast.success("Primary domain updated");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to set primary domain");
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => DomainsService.deleteDomain(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["domains"] });
      toast.success("Domain removed");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to remove domain");
    },
  });

  const form = useForm<z.infer<typeof domainSchema>>({
    resolver: zodResolver(domainSchema),
    defaultValues: {
      domain: "",
    },
  });

  const onSubmit = (values: z.infer<typeof domainSchema>) => {
    addMutation.mutate(values);
  };

  return (
    <div className="flex flex-col gap-6 max-w-[1200px] mx-auto">
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Custom Domains</h1>
          <p className="text-muted-foreground mt-1">Connect your own domain to your white-label platform.</p>
        </div>
        <Dialog open={isAddOpen} onOpenChange={setIsAddOpen}>
          <DialogTrigger render={<Button><Plus className="mr-2 h-4 w-4" />Add Domain</Button>} />
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Add Custom Domain</DialogTitle>
              <DialogDescription>
                Enter the domain you want to connect. You will need to add a TXT record to your DNS settings for verification.
              </DialogDescription>
            </DialogHeader>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="domain">Domain Name</Label>
                <Input id="domain" placeholder="e.g. app.yourcompany.com" {...form.register("domain")} />
                {form.formState.errors.domain && (
                  <p className="text-xs text-destructive">{form.formState.errors.domain.message}</p>
                )}
              </div>
              <DialogFooter>
                <Button type="button" variant="outline" onClick={() => setIsAddOpen(false)}>Cancel</Button>
                <Button type="submit" disabled={addMutation.isPending}>
                  {addMutation.isPending ? "Adding..." : "Add Domain"}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Globe className="h-5 w-5 text-primary" />
            Connected Domains
          </CardTitle>
          <CardDescription>Manage your verified and pending domains.</CardDescription>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-2">
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-12 w-full" />
              <Skeleton className="h-12 w-full" />
            </div>
          ) : domains && domains.length > 0 ? (
            <div className="rounded-md border overflow-x-auto">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Domain</TableHead>
                    <TableHead>Verification</TableHead>
                    <TableHead>SSL Status</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {domains.map((domain) => (
                    <TableRow key={domain.id}>
                      <TableCell>
                        <div className="flex items-center gap-2">
                          <span className="font-medium">{domain.domain}</span>
                          {domain.isPrimary && (
                            <div className="flex items-center text-xs font-medium text-amber-500 bg-amber-500/10 px-2 py-0.5 rounded-full border border-amber-500/20">
                              <Star className="h-3 w-3 mr-1 fill-amber-500" />
                              Primary
                            </div>
                          )}
                        </div>
                      </TableCell>
                      <TableCell>
                        {domain.verificationStatus === "Verified" ? (
                          <StatusBadge status="Verified" />
                        ) : domain.verificationStatus === "Failed" ? (
                          <StatusBadge status="Failed" />
                        ) : (
                          <StatusBadge status="Pending" />
                        )}
                      </TableCell>
                      <TableCell>
                        {domain.sslStatus === "Active" ? (
                          <div className="flex items-center text-emerald-600 text-sm">
                            <ShieldCheck className="h-4 w-4 mr-1" /> Active
                          </div>
                        ) : (
                          <div className="flex items-center text-muted-foreground text-sm">
                            <ShieldAlert className="h-4 w-4 mr-1" /> Pending
                          </div>
                        )}
                      </TableCell>
                      <TableCell className="text-right">
                        <div className="flex justify-end items-center gap-2">
                          {domain.verificationStatus !== "Verified" ? (
                            <>
                              <Dialog onOpenChange={(open) => {
                                if (open) setSelectedDomain(domain.id);
                                else setSelectedDomain(null);
                              }}>
                                <DialogTrigger render={<Button variant="outline" size="sm"><Settings className="h-4 w-4 mr-2" />Setup</Button>} />
                                <DialogContent>
                                  <DialogHeader>
                                    <DialogTitle>DNS Configuration</DialogTitle>
                                    <DialogDescription>
                                      Add the following TXT record to your domain's DNS settings to verify ownership.
                                    </DialogDescription>
                                  </DialogHeader>
                                  {isFetchingDns || !dnsInstructions ? (
                                    <div className="py-8 flex justify-center"><RefreshCw className="h-6 w-6 animate-spin text-muted-foreground" /></div>
                                  ) : (
                                    <div className="space-y-4">
                                      <div className="grid grid-cols-[100px_1fr] gap-2 p-4 border rounded-lg bg-muted/20 text-sm">
                                        <span className="font-semibold text-muted-foreground">Type:</span>
                                        <span className="font-mono">{dnsInstructions.recordType}</span>
                                        
                                        <span className="font-semibold text-muted-foreground">Name:</span>
                                        <div className="flex items-center justify-between gap-2">
                                          <span className="font-mono break-all">{dnsInstructions.name}</span>
                                          <CopyToClipboard text={dnsInstructions.name} />
                                        </div>
                                        
                                        <span className="font-semibold text-muted-foreground">Value:</span>
                                        <div className="flex items-center justify-between gap-2">
                                          <span className="font-mono break-all">{dnsInstructions.value}</span>
                                          <CopyToClipboard text={dnsInstructions.value} />
                                        </div>
                                      </div>
                                      <p className="text-xs text-muted-foreground">
                                        DNS changes may take up to 24 hours to propagate, though it usually happens much faster.
                                      </p>
                                    </div>
                                  )}
                                  <DialogFooter>
                                    <Button 
                                      onClick={() => verifyMutation.mutate(domain.id)}
                                      disabled={verifyMutation.isPending}
                                    >
                                      {verifyMutation.isPending ? "Verifying..." : "Verify Now"}
                                    </Button>
                                  </DialogFooter>
                                </DialogContent>
                              </Dialog>
                            </>
                          ) : (
                            <a href={`https://${domain.domain}`} target="_blank" rel="noopener noreferrer" className={buttonVariants({ variant: "ghost", size: "sm" })}>
                              <ExternalLink className="h-4 w-4 mr-2" /> Visit
                            </a>
                          )}

                          {!domain.isPrimary && domain.verificationStatus === "Verified" && (
                            <Button 
                              variant="outline" 
                              size="sm"
                              onClick={() => primaryMutation.mutate(domain.id)}
                              disabled={primaryMutation.isPending}
                            >
                              Make Primary
                            </Button>
                          )}
                          
                          <Button 
                            variant="ghost" 
                            size="icon" 
                            className="text-destructive hover:bg-destructive/10"
                            onClick={() => {
                              if (window.confirm("Are you sure you want to remove this domain?")) {
                                deleteMutation.mutate(domain.id);
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
              <Globe className="h-12 w-12 mx-auto text-muted-foreground/50 mb-4" />
              <h3 className="text-lg font-medium">No custom domains</h3>
              <p className="text-sm text-muted-foreground mt-1 mb-4">
                Connect your own domain to serve landing pages and dynamic links.
              </p>
              <Button onClick={() => setIsAddOpen(true)}>
                <Plus className="mr-2 h-4 w-4" /> Add your first domain
              </Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
