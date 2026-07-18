'use client';

import { useQuery } from '@tanstack/react-query';
import { billingService } from '@/lib/billing-service';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Download, FileText } from 'lucide-react';
import { format } from 'date-fns';
import { Skeleton } from '@/components/ui/skeleton';
import { cn } from '@/lib/utils';

export default function InvoicesPage() {
  const { data: invoices, isLoading } = useQuery({
    queryKey: ['invoices'],
    queryFn: billingService.getInvoices,
  });

  return (
    <div className="relative min-h-full p-6 sm:p-10 max-w-6xl mx-auto space-y-10">
      {/* Background Decor */}
      <div className="absolute top-20 right-20 w-[400px] h-[400px] bg-primary/5 rounded-full blur-[120px] -z-10" />

      <div>
        <h1 className="text-4xl font-extrabold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-foreground to-foreground/70">
          Invoices
        </h1>
        <p className="text-muted-foreground mt-2 text-lg">Download and view your past billing invoices.</p>
      </div>

      <div className="rounded-3xl border border-border/50 bg-background/60 backdrop-blur-xl shadow-xl overflow-hidden">
        <div className="overflow-x-auto">
          <Table>
            <TableHeader>
              <TableRow className="bg-muted/30 hover:bg-muted/30 border-b border-border/50">
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Invoice Number</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Date</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Amount</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Status</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {isLoading ? (
                [1, 2, 3].map(i => (
                  <TableRow key={i} className="border-b border-border/30">
                    <TableCell className="px-6 py-4"><Skeleton className="h-4 w-24 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4"><Skeleton className="h-4 w-24 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4"><Skeleton className="h-4 w-12 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4"><Skeleton className="h-6 w-16 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4 text-right"><Skeleton className="h-8 w-24 ml-auto rounded-lg" /></TableCell>
                  </TableRow>
                ))
              ) : invoices?.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={5} className="h-48 text-center">
                    <div className="flex flex-col items-center justify-center text-muted-foreground space-y-3">
                      <div className="w-12 h-12 rounded-full bg-muted/50 flex items-center justify-center">
                        <FileText className="w-6 h-6 opacity-50" />
                      </div>
                      <p className="text-sm font-medium">No invoices found.</p>
                    </div>
                  </TableCell>
                </TableRow>
              ) : (
                invoices?.map((invoice) => (
                  <TableRow key={invoice.id} className="border-b border-border/30 hover:bg-muted/20 transition-colors group">
                    <TableCell className="px-6 py-4 font-semibold text-foreground">{invoice.number}</TableCell>
                    <TableCell className="px-6 py-4 text-muted-foreground">{format(new Date(invoice.createdAt), 'MMM dd, yyyy')}</TableCell>
                    <TableCell className="px-6 py-4 font-mono font-medium">${invoice.amount.toFixed(2)}</TableCell>
                    <TableCell className="px-6 py-4">
                      <Badge variant={invoice.status === 'paid' ? 'default' : 'secondary'} className={cn(
                        "px-2.5 py-0.5 rounded-full text-xs font-bold tracking-wider shadow-sm",
                        invoice.status === 'paid' 
                          ? 'bg-green-500/10 text-green-600 hover:bg-green-500/20 border-green-500/20' 
                          : 'bg-muted text-muted-foreground'
                      )}>
                        {invoice.status.toUpperCase()}
                      </Badge>
                    </TableCell>
                    <TableCell className="px-6 py-4 text-right">
                      <Button variant="outline" size="sm" className="rounded-lg shadow-sm hover:shadow-md transition-all opacity-0 group-hover:opacity-100 focus:opacity-100 border-border/60">
                        <Download className="w-4 h-4 mr-2 text-primary" />
                        Download
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </div>
      </div>
    </div>
  );
}
